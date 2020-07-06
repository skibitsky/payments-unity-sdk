using System;
using System.Collections.Generic;
using Xsolla.Payments.Api.Playfab.Catalog;

namespace Xsolla.Payments.Api.Playfab.Purchases
{
	[Serializable]
	public class StartPurchaseResultEntity
	{
		public string OrderId;
		public List<CatalogItemEntity> Contents;
	}
}