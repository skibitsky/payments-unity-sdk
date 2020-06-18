using System;
using System.Collections.Generic;
using Xsolla.ThirdParty.Playfab.Api.Catalog;

namespace Xsolla.ThirdParty.Playfab.Api.Purchases
{
	[Serializable]
	public class StartPurchaseResultEntity
	{
		public string OrderId;
		public List<CatalogItemEntity> Contents;
	}
}
