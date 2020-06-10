using System;
using System.Collections.Generic;
using System.Linq;
using Playfab;
using Playfab.Catalog;
using Xsolla.Core;

namespace Xsolla.Store
{
    [Serializable]
    public class SimplifyCatalogItem
    {
        public const string ITEM_TYPE_VIRTUAL_ITEM = "item";
        public const string ITEM_TYPE_CURRENCY = "currency";
        
        public string DisplayName;
        public string Description;
        public string ItemImageUrl;
        public string Price;
        public string ItemType;

        public bool IsCurrency()
        {
            return ItemType.Equals(ITEM_TYPE_CURRENCY);
        }

        public bool IsVirtualItem()
        {
            return ItemType.Equals(ITEM_TYPE_VIRTUAL_ITEM);
        }

        public CatalogItemEntity ToCatalogItemEntity()
        {
            CatalogItemEntity result = new CatalogItemEntity
            {
                DisplayName = DisplayName,
                Description = Description,
                ItemImageUrl = ItemImageUrl
            };
            return result;
        }
    }
}
    