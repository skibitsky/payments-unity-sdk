using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public class PurchaseHelper : MonoSingleton<PurchaseHelper>
	{
		private const string URL_PAYSTATION_UI_BY_ACCESS_DATA = "https://secure.xsolla.com/paystation2/?access_data=";
		private const string URL_PAYSTATION_UI_IN_SANDBOX_MODE_BY_ACCESS_DATA = "https://sandbox-secure.xsolla.com/paystation2/?access_data=";
		
		private readonly List<string> _purchaseFlowComplete = new List<string>();
		private readonly List<string> _stoppedTransactions = new List<string>();
		
		/// <summary>
		/// Open Paystation in the browser with retrieved Paystation Token.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/doc/pay-station"/>
		/// <param name="accessData">Paystation access data</param>
		/// <seealso cref="BrowserHelper"/>
		public void OpenPurchaseUi(string accessData)
		{
			var url = XsollaSettings.IsSandbox
				? URL_PAYSTATION_UI_IN_SANDBOX_MODE_BY_ACCESS_DATA
				: URL_PAYSTATION_UI_BY_ACCESS_DATA;
				
			BrowserHelper.Instance.OpenPurchase(url, accessData, XsollaSettings.IsSandbox, XsollaSettings.InAppBrowserEnabled);
		}
		
		/// <summary>
		/// Polls Playfab every seconds to know when payment finished.
		/// </summary>
		/// <param name="transactionId">Unique identifier of created transaction.</param>
		/// <param name="onSuccess">Success payment callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void ProcessOrder(string projectId, string transactionId, [NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(CheckOrderStatus(projectId, transactionId, onSuccess, onError));
		}

		public void StopProccessing(string transactionId)
		{
			_stoppedTransactions.Add(transactionId);
		}

		IEnumerator CheckOrderStatus(string projectId, string transactionId, [NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			// Wait 1 second before API polling
			yield return new WaitForSeconds(1.0f);

			SimplifyIntegrationApi.GetTransactionStatus(projectId, transactionId, response =>
			{
				var status = response.status;
				Debug.Log($"Order `{transactionId}` status is `{status}`!");
				CheckUserFlow(transactionId, status);
				if (TransactionStatus.IsInProgress(status)) {
					if (NeedToRequestStatusAgain(transactionId))
						StartCoroutine(CheckOrderStatus(projectId,transactionId, onSuccess, onError));
				}
				else {
					if (TransactionStatus.IsSuccess(status))
						onSuccess?.Invoke();
					else
					{
						if (TransactionStatus.IsFailed(status))
							onError?.Invoke(new Error {errorMessage = $"Payment status: {status}"});
						else
						{
							Debug.LogError($"Get unknown transaction status = `{status}`!");
						}
					}
				}
			}, onError);
		}

		private void CheckUserFlow(string transactionId, string status)
		{
			if(IsUserCompletePurchaseFlow(transactionId)) return;
			if(string.IsNullOrEmpty(status)) return;
			_purchaseFlowComplete.Add(transactionId);
		}

		public bool IsUserCompletePurchaseFlow(string transactionId)
		{
			return _purchaseFlowComplete.Contains(transactionId);
		}

		private bool NeedToRequestStatusAgain(string transactionId)
		{
			return !TransactionIsStopped(transactionId) && 
			       (IsTheInGameBrowserOpen() || IsUserCompletePurchaseFlow(transactionId));
		}
		
		private bool TransactionIsStopped(string transactionId)
		{
			if (!_stoppedTransactions.Contains(transactionId)) return false;
			_stoppedTransactions.Remove(transactionId);
			return true;
		}

		private bool IsTheInGameBrowserOpen()
		{	
#if UNITY_STANDALONE
			// If external browser is used, then we don't know - is the browser open.
			if (!XsollaSettings.InAppBrowserEnabled) return true;
			return BrowserHelper.Instance.GetLastBrowser() != null;
#else
			return true;
#endif
		}
	}
}