using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;
using Xsolla.PayStation.Api.Playfab;
using Xsolla.PayStation.Api.Playfab.Inventory;

namespace Xsolla.Demo.Store
{
	public partial class PlayfabDemoImplementation : MonoSingleton<PlayfabDemoImplementation>, IStoreDemoImplementation
	{
		private UserInventoryEntity _inventory;
		private bool _isInProgress;
		
		public void GetInventoryItems([NotNull] Action<List<InventoryItemModel>> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(InventoryRequestCoroutine(inventory =>
			{
				onSuccess?.Invoke(inventory.Inventory.Select(i =>
				{
					CatalogItemModel catalogItem = UserCatalog.Instance.VirtualItems.First(c => c.Sku.Equals(i.ItemId));
					if (catalogItem == null)
					{
						Debug.LogWarning($"Can not create inventory item `{i.ItemId}`, because it not found in catalog");
						return null;
					}
					return new InventoryItemModel
					{
						Sku = i.ItemId,
						Name = i.DisplayName,
						RemainingUses = i.RemainingUses,
						InstanceId = i.ItemInstanceId,
						Description = catalogItem.Description,
						ImageUrl = catalogItem.ImageUrl,
						IsConsumable = catalogItem.IsConsumable
					};
				}).ToList());
			}, onError));
		}
		
		public void GetVirtualCurrencyBalance([NotNull] Action<List<VirtualCurrencyBalanceModel>> onSuccess, [CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(InventoryRequestCoroutine(inventory =>
			{
				onSuccess?.Invoke(inventory.VirtualCurrency.Select(pair =>
				{
					string vcSku = pair.Key;
					uint amount = pair.Value;
					CatalogItemModel catalogItem = UserCatalog.Instance.Currencies.First(c => c.CurrencySku.Equals(vcSku));
					if (catalogItem == null)
					{
						Debug.LogWarning($"Can not create virtual currency balance `{vcSku}`, because this  currency not found in catalog");
						return null;
					}
					return new VirtualCurrencyBalanceModel
					{
						Sku = vcSku,
						Name = catalogItem.Name,
						Description = catalogItem.Description,
						ImageUrl = catalogItem.ImageUrl,
						IsConsumable = catalogItem.IsConsumable,
						Amount = amount
					};
				}).ToList());
			}, onError));
		}

		public void ConsumeInventoryItem(InventoryItemModel item, uint count, 
			[CanBeNull] Action<InventoryItemModel> onSuccess = null,
			[CanBeNull] Action<InventoryItemModel> onFailed = null)
		{
			StoreDemoPopup.ConsumeConfirmation(item.Name, count, () => {
					PlayfabApi.Inventory.ConsumeItem(item.InstanceId, count, () =>
					{
						StoreDemoPopup.ShowSuccess();
						onSuccess?.Invoke(item);
					}, GetErrorCallback(_ => onFailed?.Invoke(item)));
				},
				() => onFailed?.Invoke(item)
			);
		}

		private IEnumerator InventoryRequestCoroutine([NotNull] Action<UserInventoryEntity> callback, [CanBeNull] Action<Error> onError = null)
		{
			if (!_isInProgress) {
				_isInProgress = true;
				yield return StartCoroutine(WaitUserCatalogCoroutine());
				PlayfabApi.Inventory.GetUserInventory(i =>
				{
					_inventory = i;
					_isInProgress = false;
				}, GetErrorCallback(onError));
			}
			yield return StartCoroutine(WaitInventoryCoroutine());
			callback?.Invoke(_inventory);
		}
		
		private IEnumerator WaitUserCatalogCoroutine()
		{
			yield return new WaitUntil(() => UserCatalog.Instance.IsUpdated);
		}

		private IEnumerator WaitInventoryCoroutine()
		{
			yield return new WaitWhile(() => _isInProgress);
		}
	}
}