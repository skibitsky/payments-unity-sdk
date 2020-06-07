using System;

namespace Playfab.Purchases
{
	[Serializable]
	public class PurchaseStatusEntity
	{
		public string OrderId;
		public string TransactionStatus;
	}
}