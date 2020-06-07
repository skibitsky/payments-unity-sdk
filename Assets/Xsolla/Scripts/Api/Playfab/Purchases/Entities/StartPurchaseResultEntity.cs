using System;
using System.Collections.Generic;
using Playfab.Catalog;

namespace Playfab.Purchases
{
	[Serializable]
	public class StartPurchaseResultEntity
	{
		public string OrderId;
		public List<CatalogItemEntity> Contents;
	}
}
