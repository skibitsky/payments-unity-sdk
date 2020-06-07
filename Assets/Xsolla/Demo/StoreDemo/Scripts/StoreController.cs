using System.Collections.Generic;
using Playfab;
using Playfab.Catalog;
using UnityEngine;
using UnityEngine.SceneManagement;
using Xsolla.Core;
using Xsolla.Store;

public class StoreController : MonoBehaviour
{	
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

	private void Awake()
	{
		CheckAuth();
	}

	private void Start()
	{
		_groupsController = FindObjectOfType<GroupsController>();
		_itemsController = FindObjectOfType<ItemsController>();
		_itemsTabControl = FindObjectOfType<ItemsTabControl>();
		_extraController = FindObjectOfType<ExtraController>();

		CatalogInit();
	}
	
	private void CheckAuth()
	{
		var playfabToken = PlayfabApi.Instance.Token;
		if (playfabToken.IsNullOrEmpty()) {
			Debug.Log("Store demo started without token. Login scene will be launched.");
			SceneManager.LoadScene("Login");
			return;
		}
		Debug.Log($"Store demo started with token {playfabToken}");
	}

	private void CatalogInit()
	{
		UserCatalog.Instance.UpdateItems(InitStoreUi, StoreDemoPopup.ShowError);
	}
	
	private void RefreshInventory()
	{
		UserInventory.Instance.Refresh(StoreDemoPopup.ShowError);
	}

	private void InitStoreUi(List<CatalogItemEntity> items)
	{	// This line for fastest image loading
		items.ForEach(i => ImageLoader.Instance.GetImageAsync(i.ItemImageUrl, null));
		
		_groupsController.CreateGroups();
		_itemsController.CreateItems(items);

		_itemsTabControl.Init();
		_extraController.Initialize();

		_groupsController.SelectDefault();
			
		RefreshInventory();
	}
}