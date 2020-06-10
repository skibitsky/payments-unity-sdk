using System.Collections.Generic;
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

	private readonly List<ItemUI> _items = new List<ItemUI>();

	private void Awake()
	{
		DisableEmptyContainerMessage();
	}

	public bool IsEmpty()
	{
		return _items.Count == 0;
	}

	public void AddItem(IItemEntity itemInformation)
	{
		ItemUI item = Instantiate(itemPrefab, itemParent).GetComponent<ItemUI>();
		item.Initialize(itemInformation);
		_items.Add(item);
	}
	
	public void EnableEmptyContainerMessage(string text = null)
	{
		emptyMessageText.gameObject.SetActive(true);
		if (!string.IsNullOrEmpty(text))
			emptyMessageText.text = text;
	}

	private void DisableEmptyContainerMessage()
	{
		emptyMessageText.gameObject.SetActive(false);
	}
	
	public void Refresh()
	{ }
}
