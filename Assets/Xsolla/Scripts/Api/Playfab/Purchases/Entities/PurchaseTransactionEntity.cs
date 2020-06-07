using System;

namespace Playfab.Purchases
{
	[Serializable]
	public class PurchaseTransactionEntity
	{
		public string OrderId;
		public string ProviderToken;
	}
}