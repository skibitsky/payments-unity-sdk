using System;

namespace Xsolla.Payments.Api.Playfab.Purchases
{
	[Serializable]
	public class PurchaseStatusEntity
	{
		public string OrderId;
		public string TransactionStatus;
	}
}