using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Store;

public class GroupsController : MonoBehaviour
{
	[SerializeField]
	GameObject groupPrefab;

	[SerializeField]
	RectTransform scrollView;

	List<IGroup> _groups;
	
	ItemsController _itemsController;
	ItemsTabControl _itemsTabControl;
	
	void Awake()
	{
		_groups = new List<IGroup>();

		_itemsController = FindObjectOfType<ItemsController>();
		_itemsTabControl = FindObjectOfType<ItemsTabControl>();
	}

	private void Start()
	{
		GroupsHotKeys hotKeys = gameObject.GetComponent<GroupsHotKeys>();
		hotKeys.ArrowDownKeyPressedEvent += () => {
			IGroup group = GetSelectedGroup();
			int index = _groups.IndexOf(group) + 1;
			if (index >= _groups.Count)
				index = 0;
			group = _groups.ElementAt(index);
			group.Select();
			SelectGroup(group.Id);
		};
		hotKeys.ArrowUpKeyPressedEvent += () => {
			IGroup group = GetSelectedGroup();
			int index = _groups.IndexOf(group);
			if (index == 0) {
				index = _groups.Count - 1;
			} else {
				index--;
			}
			group = _groups.ElementAt(index);
			group.Select();
			SelectGroup(group.Id);
		};
	}

	public void CreateGroups()
	{
		ItemGroupsHelper.GetAllAsNames().ForEach(groupName => AddGroup(groupPrefab, groupName));
	}

	void AddGroup(GameObject groupPref, string groupName)
	{
		if (_groups.Exists(group => group.Name == groupName))
			return;
		var newGroupGameObject = Instantiate(groupPref, scrollView.transform);
		newGroupGameObject.name = 
			"Group_" +
			groupName.ToUpper().First() + 
			groupName.Substring(1).Replace(" ", "").ToLower(); 
			
		var newGroup = newGroupGameObject.GetComponent<IGroup>();
		newGroup.Id = groupName;
		newGroup.Name  = groupName;
		newGroup.OnGroupClick += SelectGroup;

		_groups.Add(newGroup);
	}
	
	private void SelectGroup(string id)
	{
		_itemsController.ActivateContainer(id);
		ChangeSelection(id);
		_itemsTabControl.ActivateStoreTab();
	}
	
	void ChangeSelection(string groupId)
	{
		_groups.Where(g => g.Id != groupId).ToList().ForEach(g => g.Deselect());
	}

	public void SelectDefault()
	{
		if (_groups.Any())
		{
			_groups.First().Select();
		}
	}

	public IGroup GetSelectedGroup()
	{
		return _groups.Find((group => group.IsSelected()));
	}
}