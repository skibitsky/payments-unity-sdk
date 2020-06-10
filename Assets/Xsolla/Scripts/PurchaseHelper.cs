using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Playfab;
using Playfab.Catalog;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Store
{
    public class PurchaseHelper : MonoSingleton<PurchaseHelper>
    {
        private const string URL_PAYSTATION_UI = "https://secure.xsolla.com/paystation2/?access_token=";
        private const string URL_PAYSTATION_UI_IN_SANDBOX_MODE = "https://sandbox-secure.xsolla.com/paystation2/?access_token=";
        
        /// <summary>
        /// Buys a single item with real money.
        /// </summary>
        /// <param name="item">Entity of the item from the title catalog.</param>
        /// <param name="onSuccess">Success operation callback.</param>
        /// <param name="onError">Failed operation callback.</param>
        public void PurchasePlayfabItemForRealMoney(CatalogItemEntity item, [NotNull] Action<string> onSuccess, [CanBeNull]Action<Error> onError = null)
        {
            PlayfabApi.Purchases.ItemPurchase(item.ItemId, response => { 
                OpenPurchaseUi(response.ProviderToken);
                ProcessOrder(response.OrderId, () => onSuccess?.Invoke(item.ItemId));
            }, onError);
        }

        /// <summary>
        /// Buys a single item with virtual currency.
        /// </summary>
        /// <param name="item">Entity of the item from the title catalog.</param>
        /// <param name="onSuccess">Success operation callback.</param>
        /// <param name="onError">Failed operation callback.</param>
        public void PurchasePlayfabItemForVirtualCurrency(CatalogItemEntity item, [NotNull] Action<string> onSuccess, [CanBeNull] Action<Error> onError = null)
        {
            var price = GetVirtualPrice(item);
            PlayfabApi.Purchases.ItemPurchaseForVirtualCurrency(
                item.ItemId, price.Key, price.Value,
                () => onSuccess?.Invoke(item.DisplayName),
                onError);
        }
        
        public KeyValuePair<string, uint> GetVirtualPrice(CatalogItemEntity item)
        {
            IEnumerable<KeyValuePair<string, uint>> prices = item.GetPricesForVirtualCurrency();
            return prices.First();
        }
        
        /// <summary>
        /// Open Paystation in the browser with retrieved Paystation Token.
        /// </summary>
        /// <see cref="https://developers.xsolla.com/doc/pay-station"/>
        /// <param name="purchaseData">Contains Paystation Token for the purchase.</param>
        /// <param name="token">Paystation token</param>
        /// <seealso cref="BrowserHelper"/>
        public void OpenPurchaseUi(string token)
        {
            var url = (XsollaSettings.IsSandbox) ? URL_PAYSTATION_UI_IN_SANDBOX_MODE : URL_PAYSTATION_UI;
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