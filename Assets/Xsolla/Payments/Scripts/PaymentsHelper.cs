using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Payments
{
	public static class PaymentsHelper
	{
		private const string URL_PAYSTATION_UI =
			"https://secure.xsolla.com/paystation2/?access_token=";

		private const string URL_PAYSTATION_UI_IN_SANDBOX_MODE =
			"https://sandbox-secure.xsolla.com/paystation2/?access_token=";

		private const string URL_PAYSTATION_UI_BY_ACCESS_DATA =
			"https://secure.xsolla.com/paystation2/?access_data=";

		private const string URL_PAYSTATION_UI_IN_SANDBOX_MODE_BY_ACCESS_DATA =
			"https://sandbox-secure.xsolla.com/paystation2/?access_data=";

		/// <summary>
		/// Open Paystation in the browser with retrieved Paystation Token.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/doc/pay-station"/>
		/// <param name="token">Paystation token.</param>
		/// <param name="sandbox">Purchase mode.</param>
		/// <seealso cref="OpenPurchaseByAccessData"/>
		/// <seealso cref="BrowserHelper"/>
		public static void OpenPurchaseByAccessToken(string token, bool sandbox)
		{
			var url = sandbox ? URL_PAYSTATION_UI_IN_SANDBOX_MODE : URL_PAYSTATION_UI;
			BrowserHelper.Instance.OpenPurchase(url, token, sandbox, XsollaSettings.InAppBrowserEnabled);
		}

		/// <summary>
		/// Open Paystation in the browser with retrieved Paystation Token.
		/// </summary>
		/// <see cref="https://developers.xsolla.com/doc/pay-station"/>
		/// <param name="accessData">Paystation access data.</param>
		/// <param name="sandbox">Purchase mode.</param>
		/// <seealso cref="OpenPurchaseByAccessToken"/>
		/// <seealso cref="BrowserHelper"/>
		public static void OpenPurchaseByAccessData(string accessData, bool sandbox)
		{
			var url = sandbox
				? URL_PAYSTATION_UI_IN_SANDBOX_MODE_BY_ACCESS_DATA
				: URL_PAYSTATION_UI_BY_ACCESS_DATA;
			BrowserHelper.Instance.OpenPurchase(url, accessData, sandbox, XsollaSettings.InAppBrowserEnabled);
		}

		public static string GetAdditionalInformation(string integrationType)
		{
			return $"SDK-payments_ver-{Application.version.ToUpper()}_" +
			       $"integr-{integrationType}_" +
			       $"engine-unity_enginever_{Application.unityVersion.ToUpper()}";
		}
	}
}
