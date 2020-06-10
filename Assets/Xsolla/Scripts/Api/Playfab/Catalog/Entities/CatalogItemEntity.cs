using System;
using System.Collections.Generic;
using System.Linq;

namespace Playfab.Catalog
{
	[Serializable]
	public class CatalogItemEntity : IItemEntity
	{
		public const string REAL_MONEY_CURRENCY = "RM";
		
		[Serializable]
		public class ConsumableOptions
		{
			public uint? UsageCount;
			public uint? UsagePeriod;
		}
		[Serializable]
		public class BundleSettings
		{
			public Dictionary<string, uint> BundledVirtualCurrencies;
		}
		[Flags]
		public enum ItemGroups
		{
			None = 0,
			All = 1,
			Premium = 2,
			PowerUps = 4,
			Currency = 8
		}
		
		public string ItemId;
		public string DisplayName;
		public string Description;
		public string ItemImageUrl;
		public bool IsStackable;
		
		public Dictionary<string, uint> VirtualCurrencyPrices;
		public ConsumableOptions Consumable;
		public BundleSettings Bundle;
		
		string IItemEntity.GetSku() => ItemId;
		string IItemEntity.GetName() => DisplayName;
		string IItemEntity.GetDescription() => Description;
		string IItemEntity.GetImageUrl() => ItemImageUrl;
		
		bool IItemEntity.IsVirtualCurrency() => Bundle?.BundledVirtualCurrencies != null;
		bool IItemEntity.IsConsumable() => Consumable?.UsageCount != null;
		
		KeyValuePair<string, uint>? IItemEntity.GetVirtualPrice()
		{
			var prices = VirtualCurrencyPrices.Where(pair => !pair.Key.Equals(REAL_MONEY_CURRENCY)).ToList();
			if (!prices.Any()) return null;
			return prices.Any() ? prices.First() : (KeyValuePair<string, uint>?) null;
		}

		KeyValuePair<string, float>? IItemEntity.GetRealPrice()
		{
			if (!VirtualCurrencyPrices.ContainsKey(REAL_MONEY_CURRENCY)) return null;
			float amount = VirtualCurrencyPrices[REAL_MONEY_CURRENCY] / 100.0F;
			return new KeyValuePair<string, float>("USD", amount);
		}

		public string GetVirtualCurrencySku() => (this as IItemEntity).IsVirtualCurrency()
			? Bundle.BundledVirtualCurrencies.First().Key
			: string.Empty;
	}
}
