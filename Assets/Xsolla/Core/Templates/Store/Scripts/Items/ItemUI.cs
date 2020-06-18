using System;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;

public class ItemUI : MonoBehaviour
{
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

	private IStoreDemoImplementation _demoImplementation;
	
	public void Initialize(CatalogItemModel virtualItem, IStoreDemoImplementation demoImplementation)
	{
		_demoImplementation = demoImplementation;
		
		var text = "";
		var virtualPrice = virtualItem.VirtualPrice;
		if (virtualPrice != null)
		{
			var valuePair = virtualPrice.Value;
			text = FormatVirtualCurrencyBuyButtonText(valuePair.Key, valuePair.Value.ToString());
		} else
		{
			var realPrice = virtualItem.RealPrice;
			if (realPrice != null) {
				var valuePair = realPrice.Value;
				var currency = RegionalCurrency.GetCurrencySymbol(valuePair.Key);
				text = FormatBuyButtonText(currency, valuePair.Value.ToString("F2"));
			}
		}
		buyButton.Text = text;
		itemName.text = virtualItem.Name;
		itemDescription.text = virtualItem.Description;
		gameObject.name = "Item_" + virtualItem.Name.Replace(" ", "");
		ImageLoader.Instance.GetImageAsync(virtualItem.ImageUrl, LoadImageCallback);

		AttachBuyButtonHandler(virtualItem);
	}

	private void AttachBuyButtonHandler(CatalogItemModel virtualItem)
	{
		if (virtualItem.VirtualPrice != null) {
			buyButton.onClick = () => _demoImplementation.PurchaseForVirtualCurrency(virtualItem);
		}
		else {
			buyButton.onClick = () => _demoImplementation.PurchaseForRealMoney(virtualItem);
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
}
