using System.Collections.Generic;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Store;

public class SimplifyStoreController : MonoBehaviour
{
	private const string ITEMS_GROUP = "ITEMS";
	private const string CURRENCY_GROUP = "CURRENCY";
	
	GroupsController _groupsController;
	ItemsController _itemsController;
	IExtraPanelController _extraController;
	ItemsTabControl _itemsTabControl;

	private void OnDestroy()
	{
		StopAllCoroutines();
		if(UserCatalog.IsExist)
			Destroy(UserCatalog.Instance.gameObject);
		if(UserInventory.IsExist)
			Destroy(UserInventory.Instance.gameObject);
	}
	
	private void Start()
	{
		_groupsController = FindObjectOfType<GroupsController>();
		_itemsController = FindObjectOfType<ItemsController>();
		_itemsTabControl = FindObjectOfType<ItemsTabControl>();
		_extraController = FindObjectOfType<ExtraController>();

		CatalogInit();
	}

	private void CatalogInit()
	{
		_groupsController.AddGroup(ITEMS_GROUP);
		_groupsController.AddGroup(CURRENCY_GROUP);
		
		SimplifyUserCatalog.Instance.UpdateItemsEvent += list =>
		{
			list.ForEach(i => _itemsController.AddItemToContainer(ITEMS_GROUP, i.ToCatalogItemEntity()));
		};
		SimplifyUserCatalog.Instance.UpdateCatalog(InitStoreUi, StoreDemoPopup.ShowError);
	}
	
	private void InitStoreUi(List<SimplifyCatalogItem> items)
	{	// This line for fastest image loading
		items.ForEach(i => ImageLoader.Instance.GetImageAsync(i.ItemImageUrl, null));
		
		//_itemsController.CreateItems(items);
		_itemsTabControl.Init();
		_extraController.Initialize();

		_groupsController.SelectDefault();
			
		RefreshInventory();
	}
	
	private void RefreshInventory()
	{
		UserInventory.Instance.Refresh(StoreDemoPopup.ShowError);
	}
}