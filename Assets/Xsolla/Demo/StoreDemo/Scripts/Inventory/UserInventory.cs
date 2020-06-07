using System;
using System.Collections.Generic;
using System.Linq;
using Playfab;
using Playfab.Inventory;
using Xsolla.Core;

namespace Xsolla.Store
{
    public class UserInventory : MonoSingleton<UserInventory>
    {
        public event Action<List<InventoryItem>> UpdateItemsEvent;
        public event Action<List<VirtualCurrencyBalance>> UpdateVirtualCurrencyBalanceEvent;
        
        private List<InventoryItem> _items = new List<InventoryItem>();
        private List<VirtualCurrencyBalance> _balances = new List<VirtualCurrencyBalance>();
        
        public List<InventoryItem> GetItems() => _items;
        public List<VirtualCurrencyBalance> GetVirtualCurrencyBalance() => _balances;

        public void Refresh(Action<Error> onError = null)
        {
            PlayfabApi.Inventory.GetUserInventory(inventoryEntity =>
                {
                    _items = GetVirtualItemsFrom(inventoryEntity);
                    UpdateItemsEvent?.Invoke(GetItems());
                    
                    _balances = GetVirtualCurrenciesFrom(inventoryEntity);
                    UpdateVirtualCurrencyBalanceEvent?.Invoke(GetVirtualCurrencyBalance());
                },
                onError);
        }

        private List<InventoryItem> GetVirtualItemsFrom(UserInventoryEntity inventoryEntity)
        {
            return inventoryEntity.Inventory;
        }

        private List<VirtualCurrencyBalance> GetVirtualCurrenciesFrom(UserInventoryEntity inventoryEntity)
        {
            var currencies = inventoryEntity.VirtualCurrency.ToList();
            return currencies.Select(c => new VirtualCurrencyBalance
            {
                name = c.Key,
                sku = c.Key,
                amount = c.Value,
                image_url = UserCatalog.Instance.GetItems()
                    .FirstOrDefault(i => i.IsVirtualCurrency() && i.GetVirtualCurrencySku().Equals(c.Key))
                    ?.ItemImageUrl ?? string.Empty
            }).Distinct().ToList();
        }
    }    
}
