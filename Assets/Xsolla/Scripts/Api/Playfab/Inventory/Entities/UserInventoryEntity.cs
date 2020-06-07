using System;
using System.Collections.Generic;

namespace Playfab.Inventory
{
	[Serializable]
	public class UserInventoryEntity
	{
		public List<InventoryItem> Inventory;
		public Dictionary<string, uint> VirtualCurrency;
	}
}
