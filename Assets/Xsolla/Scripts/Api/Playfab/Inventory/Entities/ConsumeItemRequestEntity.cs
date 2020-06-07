using System;

namespace Playfab.Inventory
{
	[Serializable]
	public class ConsumeItemRequestEntity
	{
		public string ItemInstanceId;
		public uint ConsumeCount;
	}
}
