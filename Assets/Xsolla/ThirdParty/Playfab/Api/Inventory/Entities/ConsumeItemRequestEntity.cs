using System;

namespace Xsolla.ThirdParty.Playfab.Api.Inventory
{
	[Serializable]
	public class ConsumeItemRequestEntity
	{
		public string ItemInstanceId;
		public uint ConsumeCount;
	}
}
