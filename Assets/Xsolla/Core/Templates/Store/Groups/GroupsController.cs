using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupsController : MonoBehaviour
{
	public event Action<string> GroupSelectedEvent;
	
	[SerializeField] private GameObject groupPrefab;
	[SerializeField] private RectTransform scrollView;

	private readonly List<IGroup> _groups = new List<IGroup>();

	private void Start()
	{
		var hotKeys = gameObject.AddComponent<GroupsHotKeys>();
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

	public void AddGroup(string groupName)
	{
		if (_groups.Exists(group => group.Name == groupName))
			return;
		var newGroupGameObject = Instantiate(groupPrefab, scrollView.transform);
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
	
	private void SelectGroup(string groupId)
	{
		_groups.Where(g => g.Id != groupId).ToList().ForEach(g => g.Deselect());
		GroupSelectedEvent?.Invoke(groupId);
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