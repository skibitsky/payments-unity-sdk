using System;

namespace Xsolla.Payments.Api.Playfab.Inventory
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