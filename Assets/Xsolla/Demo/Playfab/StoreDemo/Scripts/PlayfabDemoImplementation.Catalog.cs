using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;
using Xsolla.ThirdParty.Playfab.Api;
using Xsolla.ThirdParty.Playfab.Api.Catalog;

namespace Xsolla.Demo.Store
{
	public partial class PlayfabDemoImplementation : MonoSingleton<PlayfabDemoImplementation>, IStoreDemoImplementation
	{
		private const float PLAYFAB_VIRTUAL_CURRENCY_ACCRUAL_TIMEOUT = 3.0F;

		private List<CatalogItemEntity> _catalog;
		private Coroutine _catalogCoroutine;

		public void GetCatalogVirtualItems([NotNull] Action<List<CatalogVirtualItemModel>> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(CatalogRequestCoroutine(catalog =>
			{
				onSuccess?.Invoke(catalog.Where(c => !c.IsVirtualCurrency()).Select(c => new CatalogVirtualItemModel
				{
					Sku = c.ItemId,
					Name = c.DisplayName,
					Description = c.Description,
					ImageUrl = c.ItemImageUrl,
					IsConsumable = c.IsConsumable(),
					RealPrice = c.GetRealPrice(),
					VirtualPrice = c.GetVirtualPrice()
				}).ToList());
			}, onError));
		}

		public void GetCatalogVirtualCurrencies([NotNull] Action<List<CatalogVirtualCurrencyModel>> onSuccess,
			[CanBeNull] Action<Error> onError = null)
		{
			StartCoroutine(CatalogRequestCoroutine(catalog =>
			{
				onSuccess?.Invoke(catalog.Where(c => c.IsVirtualCurrency()).Select(c => new CatalogVirtualCurrencyModel
				{
					Sku = c.ItemId,
					Name = c.DisplayName,
					Description = c.Description,
					ImageUrl = c.ItemImageUrl,
					RealPrice = c.GetRealPrice(),
					IsConsumable = c.IsConsumable(),
					VirtualPrice = c.GetVirtualPrice(),
					CurrencySku = c.GetVirtualCurrencySku(),
					Amount = c.GetVirtualCurrencyAmount()
				}).ToList());
			}, onError));
		}

		private IEnumerator CatalogRequestCoroutine(Action<List<CatalogItemEntity>> callback,
			[CanBeNull] Action<Error> onError = null)
		{
			if (_catalog == null)
			{
				if (_catalogCoroutine == null)
				{
					PlayfabApi.Catalog.GetCatalog(c => _catalog = c.Catalog, GetErrorCallback(onError));
				}

				_catalogCoroutine = StartCoroutine(WaitCatalogCoroutine());
				yield return _catalogCoroutine;
			}

			callback?.Invoke(_catalog);
		}

		private IEnumerator WaitCatalogCoroutine()
		{
			yield return new WaitWhile(() => _catalog == null);
		}

		public List<string> GetCatalogGroupsByItem(CatalogItemModel item)
		{
			return ItemGroupsHelper.GetNamesBy(item);
		}
	}
}