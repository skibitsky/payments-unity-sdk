using System.Collections.Generic;
using Playfab.Catalog;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour, IContainer
{
	[SerializeField]
	GameObject itemPrefab;

	[SerializeField]
	Transform itemParent;

	[SerializeField]
	Text emptyMessageText;

	public List<ItemUI> Items { get; private set; }

	private void Awake()
	{
		Items = new List<ItemUI>();
		DisableEmptyContainerMessage();
	}

	public void AddItem(CatalogItemEntity itemInformation)
	{
		ItemUI item = Instantiate(itemPrefab, itemParent).GetComponent<ItemUI>();
		item.Initialize(itemInformation);
		Items.Add(item);
	}
	
	public void EnableEmptyContainerMessage()
	{
		emptyMessageText.gameObject.SetActive(true);
	}

	private void DisableEmptyContainerMessage()
	{
		emptyMessageText.gameObject.SetActive(false);
	}
	
	public void Refresh()
	{ }
}
