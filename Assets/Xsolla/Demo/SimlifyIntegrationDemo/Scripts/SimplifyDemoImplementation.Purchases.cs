using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public partial class SimplifyDemoImplementation : 
		MonoSingleton<SimplifyDemoImplementation>,
		IStoreDemoImplementation
	{
		private const float LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN = 10.0F;

		private bool _isUserNotified;

		private void InitPurchases()
		{
			LoadTransactions();
			// Check status for all loaded transactions 
			GetAllTransactions().ForEach(t =>
			{
				var timeoutCoroutine = CheckOldTransaction(t);
				HandleTransaction(t, timeoutCoroutine, catalogItem => StoreDemoPopup.ShowSuccess());
			});
		}

		public void PurchaseForRealMoney(CatalogItemModel item, [CanBeNull] Action<CatalogItemModel> onSuccess = null,
			[CanBeNull] Action<Error> onError = null)
		{
			var transaction = CreateTransaction(item);
			var accessData = CreateAccessData(XsollaSettings.SimplifyProjectId, transaction.transactionId, item);

			PayStation.PayStationHelper.OpenPurchaseByAccessData(accessData, XsollaSettings.IsSandbox);
			var timeoutCoroutine = FailedTransactionNotification(transaction);
			HandleTransaction(transaction, timeoutCoroutine, catalogItem =>
			{
				StoreDemoPopup.ShowSuccess();
				Destroy(BrowserHelper.Instance);
				onSuccess?.Invoke(catalogItem);
			}, onError);
		}

		private TransactionCache CreateTransaction(CatalogItemModel item)
		{
			var transactionId = Guid.NewGuid().ToString();
			return TransactionCaching(XsollaSettings.SimplifyProjectId.ToString(), transactionId, item);
		}

		private void HandleTransaction(
			TransactionCache transaction,
			IEnumerator timeoutCoroutine,
			[CanBeNull] Action<CatalogItemModel> onSuccess = null,
			[CanBeNull] Action<Error> onError = null)
		{
			PurchaseHelper.Instance.ProcessOrder(transaction.projectId, transaction.transactionId, () =>
			{
				StopCoroutine(timeoutCoroutine);
				PutItemToInventory(transaction.item);
				ClearTransactionCache(transaction);
				UserInventory.Instance.Refresh();
				onSuccess?.Invoke(transaction.item);
			}, error =>
			{
				if (error.IsNetworkError)
				{
					StartCoroutine(LostConnectionHandler(transaction, timeoutCoroutine, onSuccess, onError));
				}
				else
				{
					StopCoroutine(timeoutCoroutine);
					ClearTransactionCache(transaction);
					GetErrorCallback(onError)?.Invoke(error);
				}
			});
			StartCoroutine(timeoutCoroutine);
		}

		private IEnumerator LostConnectionHandler(
			TransactionCache transaction,
			IEnumerator timeoutCoroutine,
			[CanBeNull] Action<CatalogItemModel> onSuccess = null,
			[CanBeNull] Action<Error> onError = null)
		{
			StopCoroutine(timeoutCoroutine);
			yield return new WaitForSeconds(10.0F);
			HandleTransaction(transaction, timeoutCoroutine, onSuccess, onError);
		}

		private IEnumerator FailedTransactionNotification(TransactionCache transactionCache,
			Action<TransactionCache> callback = null)
		{
			// Wait while user complete purchase flow
			yield return new WaitUntil(() =>
				PurchaseHelper.Instance.IsUserCompletePurchaseFlow(transactionCache.transactionId));
			// Wait timeout
			yield return new WaitUntil(() => IsLostTimeoutExpired(transactionCache));
			// Stop server polling
			PurchaseHelper.Instance.StopProccessing(transactionCache.transactionId);
			ClearTransactionCache(transactionCache);
			ShowTransactionErrorMessage();
			callback?.Invoke(transactionCache);
		}

		private IEnumerator CheckOldTransaction(TransactionCache transactionCache,
			Action<TransactionCache> callback = null)
		{
			// Wait status update
			yield return new WaitForSeconds(5.0F);
			if (IsLostTimeoutExpired(transactionCache))
			{
				// Stop server polling
				PurchaseHelper.Instance.StopProccessing(transactionCache.transactionId);
				// If user complete purchase flow and status still `In progress`, then something went wrong
				if (!_isUserNotified &&
				    PurchaseHelper.Instance.IsUserCompletePurchaseFlow(transactionCache.transactionId))
				{
					ShowTransactionErrorMessage();
					ClearTransactionCache(transactionCache);
					_isUserNotified = true;
				}

				callback?.Invoke(transactionCache);
			}
			else yield return StartCoroutine(FailedTransactionNotification(transactionCache, callback));
		}

		private void ShowTransactionErrorMessage()
		{
			StoreDemoPopup.ShowError(new Error
			{
				errorMessage =
					"Some transactions you've made recently were not finished. " +
					"Please, contact the support team."
			}).SetTitle("");
		}

		private bool IsLostTimeoutExpired(TransactionCache transactionCache)
		{
			// Calculate wait transaction time
			var elapsedDateTime = transactionCache.dateTime.AddMinutes(LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN);
			// LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN timeout elapsed?
			return DateTime.Now.CompareTo(elapsedDateTime) > 0;
		}

		public void PurchaseForVirtualCurrency(CatalogItemModel item,
			[CanBeNull] Action<CatalogItemModel> onSuccess = null, [CanBeNull] Action<Error> onError = null)
		{
			throw new NotImplementedException("Purchase for virtual currency not implemented in simplify demo.");
		}
	}
}