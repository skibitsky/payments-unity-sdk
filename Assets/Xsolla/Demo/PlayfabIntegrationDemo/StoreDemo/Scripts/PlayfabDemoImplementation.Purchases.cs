using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.Store
{
	public partial class PlayfabDemoImplementation : MonoSingleton<PlayfabDemoImplementation>, IStoreDemoImplementation
	{
		public void PurchaseForRealMoney(CatalogItemModel item, [CanBeNull] Action<CatalogItemModel> onSuccess = null,
			[CanBeNull] Action<Error> onError = null)
		{
			PurchaseHelper.Instance.PurchasePlayfabItemForRealMoney(item, _ =>
			{
				UserInventory.Instance.Refresh();
				StoreDemoPopup.ShowSuccess();
				Destroy(BrowserHelper.Instance);
				onSuccess?.Invoke(item);
			}, GetErrorCallback(onError));
		}

		public void PurchaseForVirtualCurrency(CatalogItemModel item,
			[CanBeNull] Action<CatalogItemModel> onSuccess = null, [CanBeNull] Action<Error> onError = null)
		{
			StoreDemoPopup.ShowConfirm(() =>
				PurchaseHelper.Instance.PurchasePlayfabItemForVirtualCurrency(item, purchasedItem =>
				{
					StartCoroutine(VirtualCurrencyPurchaseFinished(purchasedItem));
					StoreDemoPopup.ShowSuccess($"You have purchased `{purchasedItem.Name}`!");
					onSuccess?.Invoke(item);
				}, GetErrorCallback(onError)));
		}

		private IEnumerator VirtualCurrencyPurchaseFinished(CatalogItemModel virtualItem)
		{
			var refreshDelay = virtualItem.IsVirtualCurrency() ? PLAYFAB_VIRTUAL_CURRENCY_ACCRUAL_TIMEOUT : 0.0F;
			yield return new WaitForSeconds(refreshDelay);
			UserInventory.Instance.Refresh();
		}
	}
}