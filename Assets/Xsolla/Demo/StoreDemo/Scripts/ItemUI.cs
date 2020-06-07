using System;
using System.Collections;
using System.Linq;
using Playfab.Catalog;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.Store;

public class ItemUI : MonoBehaviour
{
	private const float PLAYFAB_VIRTUAL_CURRENCY_ACCRUAL_TIMEOUT = 3.0F;
	
	[SerializeField]
	Image itemImage;
	[SerializeField]
	GameObject loadingCircle;
	[SerializeField]
	Text itemName;
	[SerializeField]
	Text itemDescription;
	[SerializeField]
	SimpleTextButton buyButton;
	
	public void Initialize(CatalogItemEntity itemInformation)
	{
		var text = "";

		if (itemInformation.GetPricesForVirtualCurrency().Any())
		{
			var virtualPrice = PurchaseHelper.Instance.GetVirtualPrice(itemInformation);
			text = FormatVirtualCurrencyBuyButtonText(virtualPrice.Key, virtualPrice.Value.ToString());
		} else {
			if (itemInformation.GetPriceForRealMoney() != null) {
				var price = itemInformation.GetPriceForRealMoney()?.ToString("F2");
				var currency = RegionalCurrency.GetCurrencySymbol("USD");
				text = FormatBuyButtonText(currency, price);
			}
		}
		buyButton.Text = text;
		itemName.text = itemInformation.DisplayName;
		itemDescription.text = itemInformation.Description;
		gameObject.name = "Item_" + itemInformation.DisplayName.Replace(" ", "");
		ImageLoader.Instance.GetImageAsync(itemInformation.ItemImageUrl, LoadImageCallback);

		AttachBuyButtonHandler(itemInformation);
	}

	private void AttachBuyButtonHandler(CatalogItemEntity item)
	{
		if (item.GetPricesForVirtualCurrency().Any()) {
			buyButton.onClick = () => StoreDemoPopup.ShowConfirm(() => PurchaseForVirtualCurrency(item));
		}
		else {
			buyButton.onClick = () => PurchaseForRealMoney(item);
		}
	}

	private void LoadImageCallback(string url, Sprite image)
	{
		loadingCircle.SetActive(false);
		itemImage.sprite = image;
	}
	
	private string FormatBuyButtonText(string currency, string price)
	{
		return $"BUY FOR {currency}{price}";
	}

	private string FormatVirtualCurrencyBuyButtonText(string currency, string price)
	{
		return "BUY FOR" + Environment.NewLine + $"{price} {currency}";
	}
	
	private void PurchaseForVirtualCurrency(CatalogItemEntity item)
	{
		PurchaseHelper.Instance.PurchasePlayfabItemForVirtualCurrency(item, purchasedItem =>
		{
			StartCoroutine(VirtualCurrencyPurchaseFinished(
				item.IsVirtualCurrency() ? PLAYFAB_VIRTUAL_CURRENCY_ACCRUAL_TIMEOUT : 0.0F
				));
			StoreDemoPopup.ShowSuccess($"You are purchased `{purchasedItem}`!");
		}, StoreDemoPopup.ShowError);
	}

	IEnumerator VirtualCurrencyPurchaseFinished(float refreshDelay)
	{
		yield return new WaitForSeconds(refreshDelay);
		UserInventory.Instance.Refresh();
	}

	private void PurchaseForRealMoney(CatalogItemEntity item)
	{
		PurchaseHelper.Instance.PurchasePlayfabItemForRealMoney(item, _ =>
		{
			UserInventory.Instance.Refresh();
			StoreDemoPopup.ShowSuccess();
			Destroy(BrowserHelper.Instance);
		}, StoreDemoPopup.ShowError);
	}
}
