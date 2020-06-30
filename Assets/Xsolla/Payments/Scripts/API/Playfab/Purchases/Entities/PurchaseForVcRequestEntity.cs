using System;

namespace Xsolla.Payments.Api.Playfab.Purchases
{
	[Serializable]
	public class PurchaseForVcRequestEntity
	{
		public string ItemId;
		public string VirtualCurrency;
		public uint Price;
	}
}