using System.Collections.Generic;
using System.Linq;
using Playfab.Catalog;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Store;

public class ItemsController : MonoBehaviour
{
	[SerializeField]
	GameObject itemsContainerPrefab;

	[SerializeField]
	GameObject inventoryContainerPrefab;

	[SerializeField]
	Transform content;

	private readonly Dictionary<string, GameObject> _containers = new Dictionary<string, GameObject>();
	private bool _isEmptyCatalog;
	
	public void CreateItems(List<CatalogItemEntity> items)
	{
		var defaultContainers = GetDefaultContainers();
		defaultContainers.ToList().ForEach(container => {
			AddContainer(container.Value, container.Key);
		});
		_isEmptyCatalog = !items.Any();
		if (_isEmptyCatalog) {
			ActivateContainer(Constants.EmptyContainerName);
		} else {
			items.ForEach(i => { ItemGroupsHelper.GetNamesBy(i).ForEach(group => AddItemToContainer(group, i)); });
		}
	}

	private void AddItemToContainer(string containerName, CatalogItemEntity item)
	{
		GameObject container = _containers.ContainsKey(containerName)
			? _containers[containerName]
			: AddContainer(itemsContainerPrefab, containerName);
		container.GetComponent<ItemContainer>().AddItem(item);
	}

	private Dictionary<string, GameObject> GetDefaultContainers()
	{
		Dictionary<string, GameObject> itemContainers = GetDefaultItemContainers();
		Dictionary<string, GameObject> otherContainers = new Dictionary<string, GameObject>() {
			{ Constants.InventoryContainerName, inventoryContainerPrefab }
		};
		itemContainers.ToList().ForEach((container) => { otherContainers.Add(container.Key, container.Value); });
		return otherContainers;
	}

	private Dictionary<string, GameObject> GetDefaultItemContainers()
	{
		return new Dictionary<string, GameObject>() {
			{ Constants.EmptyContainerName, itemsContainerPrefab }
		};
	}
	
	GameObject AddContainer(GameObject itemContainerPref, string containerName)
	{
		var newContainer = Instantiate(itemContainerPref, content);
		newContainer.name = containerName;
		newContainer.SetActive(false);
		_containers.Add(containerName, newContainer);
		return newContainer;
	}

	public void ActivateContainer(string groupId)
	{
		GameObject activeContainer = InternalActivateContainer(
			_containers.ContainsKey(groupId) ? groupId : Constants.EmptyContainerName
		);
		activeContainer.GetComponent<IContainer>().Refresh();
		CheckForEmptyCatalogMessage(activeContainer);
	}

	private GameObject InternalActivateContainer(string containerName)
	{
		_containers.Values.ToList().ForEach(o => o.SetActive(false));
		_containers[containerName].SetActive(true);
		return _containers[containerName];
	}

	private void CheckForEmptyCatalogMessage(GameObject activeContainer)
	{
		ItemContainer itemContainer = activeContainer.GetComponent<ItemContainer>();
		if (_isEmptyCatalog && (itemContainer != null) && (itemContainer.Items.Count == 0)) {
			itemContainer.EnableEmptyContainerMessage();
		}
	}
}