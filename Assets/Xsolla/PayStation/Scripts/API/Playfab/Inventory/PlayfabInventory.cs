using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Xsolla.Core;

namespace Xsolla.PayStation.Api.Playfab.Inventory
{
	public class PlayfabInventory
	{
		private const string URL_INVENTORY_GET_ITEMS = "https://{0}.playfabapi.com/Client/GetUserInventory";
		private const string URL_INVENTORY_ITEM_CONSUME = "https://{0}.playfabapi.com/Client/ConsumeItem";

		/// <summary>
		/// Retrieves the user's current inventory of virtual goods.
		/// </summary>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/player-item-management/getuserinventory?view=playfab-rest"/>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void GetUserInventory([NotNull] Action<UserInventoryEntity> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_INVENTORY_GET_ITEMS);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(
				url, headers, (InventoryResponseEntity response) => onSuccess?.Invoke(response.data), onError);
		}

		/// <summary>
		/// Consume uses of a consumable item.
		/// </summary>
		/// <remarks>When all uses are consumed, it will be removed from the player's inventory.</remarks>
		/// <see cref="https://docs.microsoft.com/ru-ru/rest/api/playfab/client/player-item-management/consumeitem?view=playfab-rest"/>
		/// <param name="itemInstanceId">Unique instance identifier of the item to be consumed.</param>
		/// <param name="count">Number of uses to consume from the item.</param>
		/// <param name="onSuccess">Success operation callback.</param>
		/// <param name="onError">Failed operation callback.</param>
		public void ConsumeItem(string itemInstanceId, uint count, [NotNull] Action onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			var url = PlayfabApi.GetFormattedUrl(URL_INVENTORY_ITEM_CONSUME);
			var headers = new List<WebRequestHeader> {PlayfabApi.Instance.GetAuthHeader()};
			WebRequestHelper.Instance.PostRequest(url, new ConsumeItemRequestEntity
			{
				ItemInstanceId = itemInstanceId,
				ConsumeCount = count
			}, headers, onSuccess, onError);
		}
	}
}