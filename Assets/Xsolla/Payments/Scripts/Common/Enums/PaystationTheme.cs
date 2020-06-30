using System;

namespace Xsolla.Payments
{
	[Serializable]
	public enum PaystationTheme
	{
		Default,
		Dark,
		DefaultDark
	}

	public static class PaystationThemeHelper
	{
		private const string PAYSTATION_THEME_DARK = "dark";
		private const string PAYSTATION_THEME_DEFAULT = "default";
		private const string PAYSTATION_THEME_DEFAULT_DARK = "default_dark";

		public static string ConvertToSettings(PaystationTheme theme)
		{
			switch (theme)
			{
				case PaystationTheme.Dark: return PAYSTATION_THEME_DARK;
				case PaystationTheme.Default: return PAYSTATION_THEME_DEFAULT;
				case PaystationTheme.DefaultDark: return PAYSTATION_THEME_DEFAULT_DARK;
				default: return PAYSTATION_THEME_DEFAULT;
			}
		}
	}
}