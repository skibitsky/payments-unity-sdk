using System;

namespace Xsolla.Demo.ServerlessIntegration
{
    [Serializable]
    public class ServerlessCatalogItem
    {
        [Serializable]
        public class Price
        {
            public string currency;
            public float amount;
        }

        [Serializable]
        public class PackageContent
        {
            public string currency;
            public float quantity;
        }
        public const string ITEM_TYPE_VIRTUAL_ITEM = "virtual_good";
        public const string ITEM_TYPE_CURRENCY = "virtual_currency_package";
        
        public string sku;
        public string type;
        public string display_name;
        public string description;
        public string image_url;
        public Price price;
        public PackageContent bundle_content;

        public bool IsCurrency()
        {
            return type.Equals(ITEM_TYPE_CURRENCY);
        }

        public bool IsVirtualItem()
        {
            return type.Equals(ITEM_TYPE_VIRTUAL_ITEM);
        }
    }
}
    