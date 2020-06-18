using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public partial class SimplifyDemoImplementation : MonoSingleton<SimplifyDemoImplementation>, IStoreDemoImplementation
	{
		private const float SLOW_TRANSACTION_NOTIFICATION_TIMEOUT_SEC = 30.0F;
		private const float LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN = 10.0F;

		private bool _isUserNotificated;

		private void InitPurchases()
		{
			LoadTransactions();
			// Check status for all loaded transactions 
			GetAllTransactions().ForEach(t =>
			{
				var timeoutCoroutine = GetTimeoutCoroutineForOldTransaction(t);
				HandleTransaction(t, catalogItem =>
				{
					StopCoroutine(timeoutCoroutine);
					StoreDemoPopup.ShowSuccess();
				}, _ => StopCoroutine(timeoutCoroutine));
			});
		}

		private Coroutine GetTimeoutCoroutineForOldTransaction(TransactionCache transactionCache)
		{
			return StartCoroutine(
				DateTime.Now.Subtract(transactionCache.dateTime).TotalMinutes < LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN 
					? LostTransactionNotification(transactionCache, ClearTransactionCache) 
					: CheckOldFailedTransaction(transactionCache, ClearTransactionCache));
		}

		public void PurchaseForRealMoney(CatalogItemModel item, [CanBeNull] Action<CatalogItemModel> onSuccess = null, [CanBeNull] Action<Error> onError = null)
		{
			var transaction = CreateTransaction(item);
			var accessData = CreateAccessData(XsollaSettings.SimplifyProjectId, transaction.transactionId, item);
			
			PurchaseHelper.Instance.OpenPurchaseUi(accessData);
			Coroutine timeoutCoroutine = StartCoroutine(SlowTransactionNotification(transaction, t =>
			{
				timeoutCoroutine = StartCoroutine(LostTransactionNotification(t, ClearTransactionCache));
			}));
			HandleTransaction(transaction, catalogItem =>
			{
				StopCoroutine(timeoutCoroutine);
				StoreDemoPopup.ShowSuccess();
				Destroy(BrowserHelper.Instance);
				onSuccess?.Invoke(catalogItem);
			}, error =>
			{
				StopCoroutine(timeoutCoroutine);
				onError?.Invoke(error);
			});
		}

		private TransactionCache CreateTransaction(CatalogItemModel item)
		{
			var transactionId = Guid.NewGuid().ToString();
			return TransactionCaching(XsollaSettings.SimplifyProjectId.ToString(), transactionId, item);
		}

		private void HandleTransaction(TransactionCache transaction, [CanBeNull] Action<CatalogItemModel> onSuccess = null, [CanBeNull] Action<Error> onError = null)
		{
			PurchaseHelper.Instance.ProcessOrder(transaction.projectId, transaction.transactionId, () =>
			{
				PutItemToInventory(transaction.item);
				ClearTransactionCache(transaction);
				UserInventory.Instance.Refresh();
				onSuccess?.Invoke(transaction.item);
			}, error =>
			{
				ClearTransactionCache(transaction);
				GetErrorCallback(onError)?.Invoke(error);
			});
		}

		private IEnumerator SlowTransactionNotification(TransactionCache transactionCache, Action<TransactionCache> callback = null)
		{   // Wait while user complete purchase flow
			yield return new WaitUntil(() => 
				PurchaseHelper.Instance.IsUserCompletePurchaseFlow(transactionCache.transactionId));
			yield return new WaitForSeconds(SLOW_TRANSACTION_NOTIFICATION_TIMEOUT_SEC);
			StoreDemoPopup.ShowError(new Error{errorMessage = 
				"The transaction is in progress. " +
				"The order will be added to your inventory after completion. " +
				"You can close this window and continue to play."});
			callback?.Invoke(transactionCache);
		}

		private IEnumerator LostTransactionNotification(TransactionCache transactionCache, Action<TransactionCache> callback = null)
		{
			// Wait first 5 seconds for status update
			yield return new WaitForSeconds(5.0f);
			if (PurchaseHelper.Instance.IsUserCompletePurchaseFlow(transactionCache.transactionId))
			{
				// Calculate wait transaction time
				var elapsed = transactionCache.dateTime.AddMinutes(LOST_TRANSACTION_NOTIFICATION_TIMEOUT_MIN);
				yield return new WaitWhile(() => elapsed.CompareTo(DateTime.Now) > 0);
				// Stop server polling
				PurchaseHelper.Instance.StopProccessing(transactionCache.transactionId);
				// Show message
				StoreDemoPopup.ShowError(new Error{errorMessage = 
					"Order processing time exceeded, but the transaction is not finished. " +
					"Please, contact the support team."});
			}
			callback?.Invoke(transactionCache);
		}

		private IEnumerator CheckOldFailedTransaction(TransactionCache transactionCache, Action<TransactionCache> callback = null)
		{   // Wait status update
			yield return new WaitForSeconds(5.0F);
			// Stop server polling
			PurchaseHelper.Instance.StopProccessing(transactionCache.transactionId);
			if (PurchaseHelper.Instance.IsUserCompletePurchaseFlow(transactionCache.transactionId))
			{
				if (_isUserNotificated) yield break;
				StoreDemoPopup.ShowError(new Error{errorMessage = 
					"Some transactions you've made recently were not finished. " + 
					"Please, contact the support team."});
				_isUserNotificated = true;
			}
			callback?.Invoke(transactionCache);
		}

		public void PurchaseForVirtualCurrency(CatalogItemModel item, [CanBeNull] Action<CatalogItemModel> onSuccess = null, [CanBeNull] Action<Error> onError = null)
		{
			throw new NotImplementedException("Purchase for virtual currency not implemented in simplify demo.");
		}
	}
}