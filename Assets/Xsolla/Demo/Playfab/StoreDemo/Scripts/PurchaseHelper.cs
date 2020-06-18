using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;
using Xsolla.ThirdParty.Playfab.Api;

namespace Xsolla.Demo.Store
{
    public class PurchaseHelper : MonoSingleton<PurchaseHelper>
    {
        private const string URL_PAYSTATION_UI = "https://secure.xsolla.com/paystation2/?access_token=";
        private const string URL_PAYSTATION_UI_IN_SANDBOX_MODE = "https://sandbox-secure.xsolla.com/paystation2/?access_token=";

        /// <summary>
        /// Buys a single item with real money.
        /// </summary>
        /// <param name="virtualItem">Entity of the item from the title catalog.</param>
        /// <param name="onSuccess">Success operation callback.</param>
        /// <param name="onError">Failed operation callback.</param>
        public void PurchasePlayfabItemForRealMoney(CatalogItemModel virtualItem, [NotNull] Action<string> onSuccess, [CanBeNull] Action<Error> onError = null)
        {
            PlayfabApi.Purchases.ItemPurchase(virtualItem.Sku, response => {
                OpenPurchaseUi(response.ProviderToken);
                ProcessOrder(response.OrderId, () => onSuccess?.Invoke(virtualItem.Sku));
            }, onError);
        }

        /// <summary>
        /// Buys a single item with virtual currency.
        /// </summary>
        /// <param name="virtualItem">Entity of the item from the title catalog.</param>
        /// <param name="onSuccess">Success operation callback.</param>
        /// <param name="onError">Failed operation callback.</param>
        public void PurchasePlayfabItemForVirtualCurrency(CatalogItemModel virtualItem, [NotNull] Action<CatalogItemModel> onSuccess, [CanBeNull] Action<Error> onError = null)
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
            else {
                Debug.LogError($"You try buy item {virtualItem.Name} without virtual price!");
            }
        }
        
        /// <summary>
        /// Open Paystation in the browser with retrieved Paystation Token.
        /// </summary>
        /// <see cref="https://developers.xsolla.com/doc/pay-station"/>
        /// <param name="token">Paystation token</param>
        /// <seealso cref="BrowserHelper"/>
        public void OpenPurchaseUi(string token)
        {
            var url = XsollaSettings.IsSandbox ? URL_PAYSTATION_UI_IN_SANDBOX_MODE : URL_PAYSTATION_UI;
            BrowserHelper.Instance.OpenPurchase(url, token, XsollaSettings.IsSandbox, XsollaSettings.InAppBrowserEnabled);
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

        IEnumerator CheckOrderStatus(string orderId, [NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
        {
            // Wait 1 second before API polling
            yield return new WaitForSeconds(1.0f);
		
            PlayfabApi.Purchases.CheckOrderStatus(orderId, status =>
            {
                Debug.Log($"Order `{orderId}` status is `{status.TransactionStatus}`!");
                if (!status.TransactionStatus.Equals("Succeeded")) {
                    if (IsTheInGameBrowserOpen())
                        StartCoroutine(CheckOrderStatus(orderId, onSuccess, onError));
                }
                else {
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