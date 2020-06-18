using System;

namespace Xsolla.ThirdParty.Playfab.Api.Inventory
{
	[Serializable]
	public class InventoryItem
	{
		public string ItemId;
		public string ItemInstanceId;
		public string DisplayName;
		public uint? RemainingUses;
	}
}

