using System.Collections.Generic;
using Playfab.Inventory;
using UnityEngine;
using Xsolla.Store;

public class InventoryItemContainer : MonoBehaviour, IContainer
{
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private Transform itemParent;

	private List<GameObject> _items;

	private void Awake()
	{
		_items = new List<GameObject>();
		UserInventory.Instance.UpdateItemsEvent += RefreshInternal;
	}
	
	public void Refresh()
	{
		RefreshInternal(UserInventory.Instance.GetItems());
	}

	private void RefreshInternal(List<InventoryItem> items)
	{
		ClearItems();
		items.ForEach(AddItem);
	}
	
	private void ClearItems()
	{
		_items.ForEach(Destroy);
		_items.Clear();
	}
	
	private void AddItem(InventoryItem itemInformation)
	{
		var newItem = Instantiate(itemPrefab, itemParent);
		newItem.GetComponent<InventoryItemUI>().Initialize(itemInformation);
		_items.Add(newItem);
	}
}