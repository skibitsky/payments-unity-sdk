using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;
using Xsolla.PayStation.Api.Playfab;

namespace Xsolla.Demo.Store
{
	public class PurchaseHelper : MonoSingleton<PurchaseHelper>
	{
		/// <summary>
		/// Buys a single item with real money.
		/// </summary>
		/// <param name="virtualItem">Entity of the item from the title catalog.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void PurchasePlayfabItemForRealMoney(CatalogItemModel virtualItem, [NotNull] Action<string> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			PlayfabApi.Purchases.ItemPurchase(virtualItem.Sku, response =>
			{
				PayStation.PayStationHelper.OpenPurchaseByAccessToken(response.ProviderToken, XsollaSettings.IsSandbox);
				ProcessOrder(response.OrderId, () => onSuccess?.Invoke(virtualItem.Sku));
			}, onError);
		}

		/// <summary>
		/// Buys a single item with virtual currency.
		/// </summary>
		/// <param name="virtualItem">Entity of the item from the title catalog.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void PurchasePlayfabItemForVirtualCurrency(CatalogItemModel virtualItem,
			[NotNull] Action<CatalogItemModel> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var price = virtualItem.VirtualPrice;
			if (price.HasValue)
			{
				var pricePair = price.Value;
				PlayfabApi.Purchases.ItemPurchaseForVirtualCurrency(
					virtualItem.Sku, pricePair.Key, pricePair.Value,
					() => onSuccess?.Invoke(virtualItem),
					onError);
			}
			else
			{
				Debug.LogError($"You try buy item {virtualItem.Name} without virtual price!");
			}
		}

		/// <summary>
		/// Polls Playfab every seconds to know when payment finished.
		/// </summary>
		/// <param name="orderId">Unique identifier of created order.</param>
		/// <param name="onSuccess">Success payment callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void ProcessOrder(string orderId, [NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(CheckOrderStatus(orderId, onSuccess, onError));
		}

		IEnumerator CheckOrderStatus(string orderId, [NotNull] Action onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			// Wait 1 second before API polling
			yield return new WaitForSeconds(1.0f);

			PlayfabApi.Purchases.CheckOrderStatus(orderId, status =>
			{
				Debug.Log($"Order `{orderId}` status is `{status.TransactionStatus}`!");
				if (!status.TransactionStatus.Equals("Succeeded"))
				{
					if (IsTheInGameBrowserOpen())
						StartCoroutine(CheckOrderStatus(orderId, onSuccess, onError));
				}
				else
				{
					onSuccess?.Invoke();
				}
			}, onError);
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