using System;
using System.Collections.Generic;
using System.Linq;
using Xsolla.Core;

namespace Xsolla.Demo.ServerlessIntegration
{
	public partial class ServerlessDemoImplementation : 
		MonoSingleton<ServerlessDemoImplementation>,
		IStoreDemoImplementation
	{
		private const string ITEMS_GROUP = "ITEMS";
		private const string CURRENCY_GROUP = "CURRENCY";
		
		public void GetCatalogVirtualItems(Action<List<CatalogVirtualItemModel>> onSuccess, Action<Error> onError = null)
		{
			Action<List<ServerlessCatalogItem>> callback = items =>
			{
				onSuccess?.Invoke(items.Select(i => new CatalogVirtualItemModel
				{
					Sku = i.sku,
					Description = i.description,
					Name = i.display_name,
					ImageUrl = i.image_url,
					RealPrice = new KeyValuePair<string, float>(
						i.price.currency, i.price.amount),
					IsConsumable = true
				}).ToList());
			};
			ServerlessUserCatalog.Instance.UpdateItemsEvent += callback;
			ServerlessUserCatalog.Instance.UpdateCatalog();
			ServerlessUserCatalog.Instance.UpdateItemsEvent -= callback;
		}

		public void GetCatalogVirtualCurrencies(Action<List<CatalogVirtualCurrencyModel>> onSuccess, Action<Error> onError = null)
		{
			Action<List<ServerlessCatalogItem>> callback = items =>
			{
				onSuccess?.Invoke(items.Select(i => new CatalogVirtualCurrencyModel
				{
					Sku = i.sku,
					Description = i.description,
					Name = i.display_name,
					ImageUrl = i.image_url,
					RealPrice = new KeyValuePair<string, float>(
						i.price.currency, i.price.amount),
					CurrencySku = i.bundle_content.currency,
					Amount = (uint)(i.bundle_content.quantity),
					IsConsumable = true
				}).ToList());
			};
			ServerlessUserCatalog.Instance.UpdateVirtualCurrenciesEvent += callback;
			ServerlessUserCatalog.Instance.UpdateCatalog();
			ServerlessUserCatalog.Instance.UpdateVirtualCurrenciesEvent -= callback;
		}

		public List<string> GetCatalogGroupsByItem(CatalogItemModel item)
		{
			return new List<string>
			{
				item.IsVirtualCurrency() ? CURRENCY_GROUP : ITEMS_GROUP
			};
		}
	}
}