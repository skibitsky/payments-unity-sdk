using System;
using System.Collections.Generic;
using System.Linq;
using Playfab;
using Playfab.Catalog;
using Xsolla.Core;

namespace Xsolla.Store
{
    public class UserCatalog : MonoSingleton<UserCatalog>
    {
        public event Action<List<CatalogItemEntity>> UpdateItemsEvent;
        public event Action<List<VirtualCurrencyItem>> UpdateVirtualCurrenciesEvent;

        private List<CatalogItemEntity> _items = new List<CatalogItemEntity>();
        private List<VirtualCurrencyItem> _currencies = new List<VirtualCurrencyItem>();

        public List<CatalogItemEntity> GetItems() => _items;

        public void UpdateItems(Action<List<CatalogItemEntity>> onSuccess = null, Action<Error> onError = null)
        {
            PlayfabApi.Catalog.GetCatalog(items =>
            {
                _items = items.Catalog;
                CheckVirtualCurrencies(items);
                UpdateItemsEvent?.Invoke(_items);
                onSuccess?.Invoke(_items);
            }, onError);
        }

        private void CheckVirtualCurrencies(CatalogItemsEntity itemsEntity)
        {
            List<VirtualCurrencyItem> allCurrencies = itemsEntity.Catalog
                .Where(i => i.IsVirtualCurrency())
                .Select(c => new VirtualCurrencyItem 
                {
                    sku = c.Bundle.BundledVirtualCurrencies.First().Key,
                    imageUrl = c.ItemImageUrl
                }).ToList();
            _currencies = allCurrencies.GroupBy(c => c.sku).Select(g => g.FirstOrDefault()).ToList();
            UpdateVirtualCurrenciesEvent?.Invoke(_currencies);
        }
    }
}
    