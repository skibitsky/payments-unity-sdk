using System;
using System.Collections;
using System.Linq;
using Playfab;
using Playfab.Catalog;
using Playfab.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.Store;

public class InventoryItemUI : MonoBehaviour
{
	private const int PLAYFAB_API_CONSUME_ITEMS_LIMIT = 25;
	
	[SerializeField] private Image itemImage;
	[SerializeField] private GameObject loadingCircle;
	[SerializeField] private Text itemName;
	[SerializeField] private Text itemDescription;
	[SerializeField] private GameObject itemQuantityImage;
	[SerializeField] private Text itemQuantityText;
	[SerializeField] private ConsumeButton consumeButton;
	
	private InventoryItem _itemInformation;
	private CatalogItemEntity _catalogItem;

	private void Awake()
	{
		DisableConsumeButton();
	}

	public void Initialize(InventoryItem itemInformation)
	{
		_itemInformation = itemInformation;
		_catalogItem = UserCatalog.Instance.GetItems()
			.FirstOrDefault(c => c.ItemId.Equals(itemInformation.ItemId));

		itemName.text = _itemInformation.DisplayName;
		if (_itemInformation.RemainingUses == null)
		{
			if(itemQuantityImage != null)
				itemQuantityImage.SetActive(false);
			else
			{
				if(itemQuantityText != null)
					itemQuantityText.text = string.Empty;
			}
		}
		else
			itemQuantityText.text = _itemInformation.RemainingUses.Value.ToString();

		if (_catalogItem == null) return;
		itemDescription.text = _catalogItem.Description;
		if(!string.IsNullOrEmpty(_catalogItem.ItemImageUrl))
			ImageLoader.Instance.GetImageAsync(_catalogItem.ItemImageUrl, LoadImageCallback);
		else
		{
			loadingCircle.SetActive(false);
			itemImage.sprite = null;
		}
	}

	private void LoadImageCallback(string url, Sprite image)
	{
		loadingCircle.SetActive(false);
		itemImage.sprite = image;

		RefreshConsumeButton();
	}

	private void RefreshConsumeButton()
	{
		if (_catalogItem.IsConsumable()) {
			EnableConsumeButton();
		} else {
			DisableConsumeButton();
		}
	}

	private void EnableConsumeButton()
	{
		consumeButton.gameObject.SetActive(true);
		consumeButton.onClick = ConsumeHandler;
		if (consumeButton.counter < 1)
			consumeButton.counter.IncreaseValue(1 - consumeButton.counter.GetValue());
		consumeButton.counter.ValueChanged += Counter_ValueChanged;
	}

	private void DisableConsumeButton()
	{
		consumeButton.counter.ValueChanged -= Counter_ValueChanged;
		consumeButton.gameObject.SetActive(false);
	}

	private void Counter_ValueChanged(int newValue)
	{
		if(newValue > _itemInformation.RemainingUses || newValue > PLAYFAB_API_CONSUME_ITEMS_LIMIT) {
			StartCoroutine(DecreaseConsumeQuantityCoroutine());
		}
	}

	private IEnumerator DecreaseConsumeQuantityCoroutine()
	{
		yield return new WaitForEndOfFrame();
		consumeButton.counter.DecreaseValue(1);
	}

	private void ConsumeHandler()
	{
		StoreDemoPopup.ShowConfirm(
			() => {
				loadingCircle.SetActive(true);
				DisableConsumeButton();
				SendConsumeRequest((uint)consumeButton.counter.GetValue(), ConsumeItemsSuccess);
			},
			null,
			$"Item '{_itemInformation.DisplayName}' x {consumeButton.counter} will be consumed. Are you sure?"
		);
	}
	
	private void SendConsumeRequest(uint count, Action callback)
	{
		PlayfabApi.Inventory.ConsumeItem(_itemInformation.ItemInstanceId, count, callback, ConsumeItemsFailed);
	}

	private void ConsumeItemsSuccess()
	{
		EnableConsumeButton();
		loadingCircle.SetActive(false);
		StoreDemoPopup.ShowSuccess();
		UserInventory.Instance.Refresh();
	}

	private void ConsumeItemsFailed(Error error)
	{
		EnableConsumeButton();
		loadingCircle.SetActive(false);
		StoreDemoPopup.ShowError(error);
	}
}