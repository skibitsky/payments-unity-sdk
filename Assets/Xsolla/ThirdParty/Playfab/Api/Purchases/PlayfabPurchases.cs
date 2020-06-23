using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Xsolla.Core;

namespace Xsolla.ThirdParty.Playfab.Api.Purchases
{
	public class PlayfabPurchases
	{
		private const string URL_CLOUD_SCRIPT = "https://{0}.playfabapi.com/Client/ExecuteCloudScript";
		private const string CLOUD_SCRIPT_DEMO_METHOD = "CreatePaystationToken";
		private const uint ITEMS_QUANTITY_FOR_CLOUD_SCRIPT = 1;
		private const string URL_START_PURCHASE = "https://{0}.playfabapi.com/Client/StartPurchase";
		private const string URL_BUY_ITEM_FOR_VC = "https://{0}.playfabapi.com/Client/PurchaseItem";
		private const string URL_GET_PURCHASE_STATUS = "https://{0}.playfabapi.com/Client/GetPurchase";

		/// <summary>
		/// Buys a single item with real money.
		/// </summary>
		/// <param name="itemId">Unique identifier of the item to purchase.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="ItemPurchaseForVirtualCurrency"/>
		/// <seealso cref="CheckOrderStatus"/>
		public void ItemPurchase(string itemId, [NotNull] Action<PurchaseTransactionEntity> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			StartPurchase(itemId, result => ExecuteCloudScript(itemId, result.OrderId, script =>
			{
				var entity = script.FunctionResult;
				if (entity != null)
					onSuccess?.Invoke(new PurchaseTransactionEntity
					{
						OrderId = result.OrderId,
						ProviderToken = entity.token
					});
				else
					onError?.Invoke(new Error
					{
						code = 422,
						error = "Invalid token",
						errorMessage = $"Result from cloud script is `{script.FunctionResult.SerializeToJson()}`"
					});
			}, onError), onError);
		}

		/// <summary>
		/// Creates an order for a item from the title catalog.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/player-item-management/startpurchase?view=playfab-rest"/>
		/// <param name="itemId">Unique identifier of the item to purchase.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		private void StartPurchase(string itemId, [NotNull] Action<StartPurchaseResultEntity> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_START_PURCHASE);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(
				url, new StartPurchaseRequestEntity(itemId, ITEMS_QUANTITY_FOR_CLOUD_SCRIPT), headers,
				(StartPurchaseResponseEntity response) => onSuccess?.Invoke(response.data), onError);
		}

		/// <summary>
		/// Executes a CloudScript function.
		/// This cloud script allows to open purchase url with one item.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/server-side-cloud-script/executecloudscript?view=playfab-rest"/>
		/// <param name="itemId">Unique identifier of the item to purchase.</param>
		/// <param name="orderId">Purchase order identifier.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		private void ExecuteCloudScript(string itemId, string orderId,
			[NotNull] Action<CloudScriptResultEntity> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_CLOUD_SCRIPT);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(url, new CloudScriptRequestEntity
			{
				FunctionName = CLOUD_SCRIPT_DEMO_METHOD,
				FunctionParameter = new CloudScriptRequestEntity.CloudScriptArgs
				{
					sku = itemId,
					amount = ITEMS_QUANTITY_FOR_CLOUD_SCRIPT,
					orderId = orderId
				}
			}, headers, (CloudScriptResponseEntity response) => onSuccess?.Invoke(response.data), onError);
		}

		/// <summary>
		/// Buys a single item with virtual currency.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/player-item-management/purchaseitem?view=playfab-rest"/>
		/// <param name="itemId">Unique identifier of the item to purchase.</param>
		/// <param name="currency">Virtual currency to use to purchase the item.</param>
		/// <param name="price">Price the client expects to pay for the item.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="ItemPurchase"/>
		/// <seealso cref="CheckOrderStatus"/>
		public void ItemPurchaseForVirtualCurrency(string itemId, string currency, uint price,
			[NotNull] Action onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_BUY_ITEM_FOR_VC);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(url, new PurchaseForVcRequestEntity
			{
				ItemId = itemId,
				VirtualCurrency = currency,
				Price = price
			}, headers, onSuccess, onError);
		}

		/// <summary>
		/// Retrieves a purchase along with its current PlayFab status.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/player-item-management/getpurchase?view=playfab-rest"/>
		/// <param name="orderId">Purchase order identifier.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		/// <seealso cref="ItemPurchase"/>
		/// <seealso cref="ItemPurchaseForVirtualCurrency"/>
		public void CheckOrderStatus(string orderId, [NotNull] Action<PurchaseStatusEntity> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_GET_PURCHASE_STATUS);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(url, new PurchaseStatusRequestEntity {OrderId = orderId}, headers,
				(PurchaseStatusResponseEntity response) => onSuccess?.Invoke(response.data), onError);
		}
	}
}