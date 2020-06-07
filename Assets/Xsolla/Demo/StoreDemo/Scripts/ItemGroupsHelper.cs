using System;
using System.Collections.Generic;
using System.Linq;
using Playfab.Catalog;
using Xsolla.Core;
using static Playfab.Catalog.CatalogItemEntity;

namespace Xsolla.Store
{
    public static class ItemGroupsHelper
    {
        public static List<ItemGroups> GetAllAsList()
        {
            return Enum.
                GetNames(typeof(ItemGroups)).
                Select(name => (ItemGroups)Enum.Parse(typeof(ItemGroups), name)).
                ToList();
        }

        public static List<string> GetAllAsNames()
        {
            return GetNamesBy(GetAllAsFlags());
        }

        public static ItemGroups GetAllAsFlags()
        {
            List<ItemGroups> groups = GetAllAsList();
            ItemGroups result = ItemGroups.All;
            groups.ForEach(g => result |= g);
            return result;
        }

        public static ItemGroups GetBy(CatalogItemEntity item)
        {
            if (item.IsVirtualCurrency()) return ItemGroups.Currency;
            var result = ItemGroups.All;
            if (item.IsConsumable())
                result |= ItemGroups.PowerUps;
            if (item.IsDurable())
                result |= ItemGroups.Premium;
            return result;
        }

        public static List<string> GetNamesBy(ItemGroups groups)
        {
            var result = new List<string>();
            if((groups & ItemGroups.All) > 0)
                result.Add(Constants.AllGroupName);
            if((groups & ItemGroups.Currency) > 0)
                result.Add(Constants.CurrencyGroupName);
            if((groups & ItemGroups.PowerUps) > 0)
                result.Add(Constants.PowerUpsGroupName);
            if((groups & ItemGroups.Premium) > 0)
                result.Add(Constants.PremiumGroupName);
            return result;
        }
        
        public static List<string> GetNamesBy(CatalogItemEntity item)
        {
            var groups = GetBy(item);
            return GetNamesBy(groups);
        }
    }   
}
    