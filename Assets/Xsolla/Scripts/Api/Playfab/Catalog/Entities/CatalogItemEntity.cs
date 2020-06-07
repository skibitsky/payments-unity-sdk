using System;
using System.Collections.Generic;
using System.Linq;

namespace Playfab.Catalog
{
	[Serializable]
	public class CatalogItemEntity
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

		public bool IsConsumable()
		{
			return Consumable?.UsageCount != null;
		}

		public bool IsDurable()
		{
			return !IsConsumable();
		}
		
		public bool IsVirtualCurrency()
		{
			return Bundle?.BundledVirtualCurrencies != null;
		}

		public string GetVirtualCurrencySku() => IsVirtualCurrency()
			? Bundle.BundledVirtualCurrencies.First().Key
			: string.Empty;

		public uint GetVirtualCurrencyAmount(string currency = "")
		{
			return IsVirtualCurrency() ? 
				(string.IsNullOrEmpty(currency) 
					? Bundle.BundledVirtualCurrencies.First().Value 
					: Bundle.BundledVirtualCurrencies[currency]) 
				: 0;
		}

		public float? GetPriceForRealMoney()
		{
			return VirtualCurrencyPrices.ContainsKey(REAL_MONEY_CURRENCY) 
				? VirtualCurrencyPrices[REAL_MONEY_CURRENCY] / 100.0F
				: (float?) null;
		}

		public IEnumerable<KeyValuePair<string, uint>> GetPricesForVirtualCurrency()
		{
			return VirtualCurrencyPrices.Where(pair => !pair.Key.Equals(REAL_MONEY_CURRENCY));
		}
	}
}
