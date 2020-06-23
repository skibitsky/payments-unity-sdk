using System;
using System.Collections.Generic;
using Xsolla.PayStation.Api.Playfab.Catalog;

namespace Xsolla.PayStation.Api.Playfab.Purchases
{
	[Serializable]
	public class StartPurchaseResultEntity
	{
		public string OrderId;
		public List<CatalogItemEntity> Contents;
	}
}