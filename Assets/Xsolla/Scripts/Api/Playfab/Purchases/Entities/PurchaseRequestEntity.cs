using System;

namespace Playfab.Purchases
{
	[Serializable]
	public class PurchaseRequestEntity
	{
		public const string XSOLLA_PROVIDER = "xsolla";
			
		public string TokenProvider;

		public PurchaseRequestEntity(string provider = XSOLLA_PROVIDER)
		{
			TokenProvider = provider;
		}
	}
}