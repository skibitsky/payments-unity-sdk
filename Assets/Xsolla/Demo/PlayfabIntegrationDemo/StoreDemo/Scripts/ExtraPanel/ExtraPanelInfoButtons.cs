using System;
using UnityEngine;

namespace Xsolla.Demo.Store
{
	public class ExtraPanelInfoButtons : MonoBehaviour
	{
		private const string URL_REGISTRATION = "https://publisher.xsolla.com/";
		private const string URL_PLAYFAB_GAME_MANAGER = "https://developer.playfab.com/en-US/sign-up";
		private const string URL_DOCUMENTATION = "https://developers.xsolla.com/sdk/";
		private const string URL_DOCUMENTATION_PLAYFAB = "https://docs.microsoft.com/en-us/gaming/playfab/";
		private const string URL_TEST_CARDS = "https://developers.xsolla.com/api/v2/pay-station/#api_payment_ui_test_cards";
		private const string URL_REQUEST_INTEGRATION = "https://forms.gle/eYvFZaEaiSBmNsyT6";

		public event Action<string> OpenUrlEvent;

		[SerializeField] private GameObject registrationButton;
		[SerializeField] private GameObject gameManagerButton;
		[SerializeField] private GameObject paystationDocsButton;
		[SerializeField] private GameObject playfabDocsButton;
		[SerializeField] private GameObject testCardsButton;
		[SerializeField] private GameObject requestIntegrationButton;

		public void Init()
		{
			EnableInfoButton(registrationButton, URL_REGISTRATION);
			EnableInfoButton(gameManagerButton, URL_PLAYFAB_GAME_MANAGER);
			EnableInfoButton(paystationDocsButton, URL_DOCUMENTATION);
			EnableInfoButton(playfabDocsButton, URL_DOCUMENTATION_PLAYFAB);
			EnableInfoButton(testCardsButton, URL_TEST_CARDS);
			EnableInfoButton(requestIntegrationButton, URL_REQUEST_INTEGRATION);
		}

		private void EnableInfoButton(GameObject go, string url)
		{
			if (go == null) return;
			go.SetActive(true);
			var buttonComponent = go.GetComponent<SimpleTextButton>();
			if (buttonComponent)
				buttonComponent.onClick = () => OpenUrlEvent?.Invoke(url);
		}
	}
}