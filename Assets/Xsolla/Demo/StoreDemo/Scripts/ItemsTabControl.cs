using UnityEngine;
using Xsolla.Core;
using Xsolla.Store;

public partial class ItemsTabControl : MonoBehaviour
{
	private const string STORE_BUTTON_TEXT = "Store";

	[SerializeField] private MenuButton storeButton;
	[SerializeField] private MenuButton inventoryButton;
	[SerializeField] private VirtualCurrencyContainer virtualCurrencyBalance;

	private ItemsController _itemsController;
	private GroupsController _groupsController;

	private void Awake()
	{
		UserCatalog.Instance.UpdateVirtualCurrenciesEvent += virtualCurrencyBalance.SetCurrencies;
		UserInventory.Instance.UpdateVirtualCurrencyBalanceEvent += virtualCurrencyBalance.SetCurrenciesBalance;
	}

	public void Init()
	{
		_groupsController = FindObjectOfType<GroupsController>();
		_itemsController = FindObjectOfType<ItemsController>();
		
		storeButton.gameObject.SetActive(true);
		inventoryButton.gameObject.SetActive(true);

		storeButton.Select(false);
		storeButton.onClick = _ => InternalActivateStoreTab();
		inventoryButton.onClick = _ => InternalActivateInventoryTab();

		InitHotKeys();
	}

	private void InternalActivateStoreTab()
	{
		inventoryButton.Deselect();

		var selectedGroup = _groupsController.GetSelectedGroup();
		if (selectedGroup != null) {
			_itemsController.ActivateContainer(selectedGroup.Id);
		}
	}

	private void InternalActivateInventoryTab()
	{
		storeButton.Deselect();
		_itemsController.ActivateContainer(Constants.InventoryContainerName);
	}

	public void ActivateStoreTab()
	{
		storeButton.Text = STORE_BUTTON_TEXT;
		storeButton.Select(false);
		inventoryButton.Deselect();
	}
}