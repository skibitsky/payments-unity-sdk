using System;

namespace Xsolla.ThirdParty.Playfab.Api.Purchases
{
	[Serializable]
	public class PurchaseStatusEntity
	{
		public string OrderId;
		public string TransactionStatus;
	}
}