using System;
using System.Collections;
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
	
	public void Initialize(IItemEntity item)
	{
		var text = "";

		var virtualPrice = item.GetVirtualPrice();
		if (virtualPrice != null)
		{
			var valuePair = virtualPrice.Value;
			text = FormatVirtualCurrencyBuyButtonText(valuePair.Key, valuePair.Value.ToString());
		} else
		{
			var realPrice = item.GetRealPrice();
			if (realPrice != null) {
				var valuePair = realPrice.Value;
				var currency = RegionalCurrency.GetCurrencySymbol(valuePair.Key);
				text = FormatBuyButtonText(currency, valuePair.Value.ToString("F2"));
			}
		}
		buyButton.Text = text;
		itemName.text = item.GetName();
		itemDescription.text = item.GetDescription();
		gameObject.name = "Item_" + item.GetName().Replace(" ", "");
		ImageLoader.Instance.GetImageAsync(item.GetImageUrl(), LoadImageCallback);

		AttachBuyButtonHandler(item);
	}

	private void AttachBuyButtonHandler(IItemEntity item)
	{
		if (item.GetVirtualPrice() != null) {
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
	
	private void PurchaseForVirtualCurrency(IItemEntity item)
	{
		PurchaseHelper.Instance.PurchasePlayfabItemForVirtualCurrency(item, purchasedItem =>
		{
			StartCoroutine(VirtualCurrencyPurchaseFinished(purchasedItem));
			StoreDemoPopup.ShowSuccess($"You have purchased `{purchasedItem}`!");
		}, StoreDemoPopup.ShowError);
	}

	static IEnumerator VirtualCurrencyPurchaseFinished(IItemEntity item)
	{
		var refreshDelay = item.IsVirtualCurrency() ? PLAYFAB_VIRTUAL_CURRENCY_ACCRUAL_TIMEOUT : 0.0F;
		yield return new WaitForSeconds(refreshDelay);
		UserInventory.Instance.Refresh();
	}

	private void PurchaseForRealMoney(IItemEntity item)
	{
		PurchaseHelper.Instance.PurchasePlayfabItemForRealMoney(item, _ =>
		{
			UserInventory.Instance.Refresh();
			StoreDemoPopup.ShowSuccess();
			Destroy(BrowserHelper.Instance);
		}, StoreDemoPopup.ShowError);
	}
}
