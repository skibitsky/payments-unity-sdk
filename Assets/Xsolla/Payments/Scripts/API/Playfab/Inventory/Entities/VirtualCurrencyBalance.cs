using System;

namespace Xsolla.Payments.Api.Playfab.Inventory
{
	[Serializable]
	public struct VirtualCurrencyBalance
	{
		public string sku;
		public string type;
		public string name;
		public uint amount;
		public string description;
		public string image_url;
	}
}