using System;

namespace Xsolla.Payments.Api.Playfab.Inventory
{
	[Serializable]
	public class ConsumeItemRequestEntity
	{
		public string ItemInstanceId;
		public uint ConsumeCount;
	}
}