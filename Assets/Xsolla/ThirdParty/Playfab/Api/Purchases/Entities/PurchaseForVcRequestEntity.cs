using System;

namespace Xsolla.ThirdParty.Playfab.Api.Purchases
{
	[Serializable]
	public class PurchaseForVcRequestEntity
	{
		public string ItemId;
		public string VirtualCurrency;
		public uint Price;
	}
}