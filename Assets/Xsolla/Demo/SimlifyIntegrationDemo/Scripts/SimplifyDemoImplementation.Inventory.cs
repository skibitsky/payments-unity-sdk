using System;
using System.Collections.Generic;
using System.Linq;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public partial class SimplifyDemoImplementation :
		MonoSingleton<SimplifyDemoImplementation>,
		IStoreDemoImplementation
	{
		public void GetInventoryItems(Action<List<InventoryItemModel>> onSuccess, Action<Error> onError = null)
		{
			List<InventoryItemModel> inventoryItems =
				LoadUserData<List<InventoryItemModel>>(SimplifyDemoConstants.USER_INVENTORY_ITEMS) ??
				new List<InventoryItemModel>();
			onSuccess?.Invoke(inventoryItems);
		}

		public void GetVirtualCurrencyBalance(Action<List<VirtualCurrencyBalanceModel>> onSuccess,
			Action<Error> onError = null)
		{
			List<VirtualCurrencyBalanceModel> balanceModels =
				LoadUserData<List<VirtualCurrencyBalanceModel>>(SimplifyDemoConstants.USER_VIRTUAL_CURRENCY_BALANCE) ??
				new List<VirtualCurrencyBalanceModel>();
			UserCatalog.Instance.Currencies.ForEach(c =>
			{
				if (balanceModels.Exists(b => b.Sku.Equals(c.CurrencySku))) return;
				balanceModels.Add(GetVirtualBalanceBy(c));
				SaveUserData(SimplifyDemoConstants.USER_VIRTUAL_CURRENCY_BALANCE, UserInventory.Instance.Balance);
			});
			onSuccess?.Invoke(balanceModels);
		}

		public void ConsumeInventoryItem(InventoryItemModel item, uint count, Action<InventoryItemModel> onSuccess,
			Action<InventoryItemModel> onFailed = null)
		{
			StoreDemoPopup.ConsumeConfirmation(item.Name, count, () =>
			{
				StoreDemoPopup.ShowSuccess();
				RemoveItemFromInventory(item, count);
				onSuccess?.Invoke(item);
			}, () => onFailed?.Invoke(item));
		}

		private void PutItemToInventory(CatalogItemModel item)
		{
			if (item.IsVirtualCurrency())
			{
				var vcItem = UserCatalog.Instance.Currencies.First(i => i.Sku.Equals(item.Sku));
				IncreaseVirtualCurrencyBalance(vcItem.CurrencySku, vcItem.Amount);
			}
			else
			{
				var inventoryItem = GetInventoryItemBy(item);
				PutVirtualItemToInventory(inventoryItem);
			}
		}

		private void IncreaseVirtualCurrencyBalance(string sku, uint amount)
		{
			if (UserInventory.Instance.Balance.Count(b => b.Sku.Equals(sku)) <= 0) return;
			var balance = UserInventory.Instance.Balance.First(b => b.Sku.Equals(sku));
			balance.Amount += amount;
			SaveUserData(SimplifyDemoConstants.USER_VIRTUAL_CURRENCY_BALANCE, UserInventory.Instance.Balance);
		}

		private void PutVirtualItemToInventory(InventoryItemModel item)
		{
			if (UserInventory.Instance.Items.Any(i => i.Sku.Equals(item.Sku)))
			{
				InventoryItemModel inventoryItem = UserInventory.Instance.Items.First(i => i.Sku.Equals(item.Sku));
				if (inventoryItem.RemainingUses.HasValue)
					inventoryItem.RemainingUses++;
				else
					inventoryItem.RemainingUses = 1;
			}
			else
				UserInventory.Instance.Items.Add(item);

			SaveUserData(SimplifyDemoConstants.USER_INVENTORY_ITEMS, UserInventory.Instance.Items);
		}

		private void RemoveItemFromInventory(InventoryItemModel item, uint count)
		{
			if (item.RemainingUses.HasValue && item.RemainingUses.Value <= count ||
			    !string.IsNullOrEmpty(item.InstanceId))
				UserInventory.Instance.Items.Remove(item);
			else
			{
				if (item.RemainingUses.HasValue)
					item.RemainingUses -= count;
			}

			SaveUserData(SimplifyDemoConstants.USER_INVENTORY_ITEMS, UserInventory.Instance.Items);
		}

		private InventoryItemModel GetInventoryItemBy(CatalogItemModel item)
		{
			string json = item.SerializeToJson();
			InventoryItemModel inventoryItemModel = json.DeserializeTo<InventoryItemModel>();
			inventoryItemModel.RemainingUses = 1;
			inventoryItemModel.InstanceId = null;
			return inventoryItemModel;
		}

		private VirtualCurrencyBalanceModel GetVirtualBalanceBy(CatalogVirtualCurrencyModel item)
		{
			string json = item.SerializeToJson();
			VirtualCurrencyBalanceModel balanceModel = json.DeserializeTo<VirtualCurrencyBalanceModel>();
			balanceModel.Sku = item.CurrencySku;
			balanceModel.Amount = 0;
			return balanceModel;
		}
	}
}