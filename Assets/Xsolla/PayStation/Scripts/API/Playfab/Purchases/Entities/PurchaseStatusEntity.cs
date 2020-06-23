using System;

namespace Xsolla.PayStation.Api.Playfab.Purchases
{
	[Serializable]
	public class PurchaseStatusEntity
	{
		public string OrderId;
		public string TransactionStatus;
	}
}