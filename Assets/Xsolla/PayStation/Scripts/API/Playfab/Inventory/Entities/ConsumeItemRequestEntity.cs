using System;

namespace Xsolla.PayStation.Api.Playfab.Inventory
{
	[Serializable]
	public class ConsumeItemRequestEntity
	{
		public string ItemInstanceId;
		public uint ConsumeCount;
	}
}