using System;
using UnityEngine;

namespace Xsolla.Demo.ServerlessIntegration
{
	public class ExtraPanelInfoButtons : MonoBehaviour
	{
		const string URL_REGISTRATION = "https://publisher.xsolla.com/";
		const string URL_DOCUMENTATION = "https://developers.xsolla.com/sdk/";
		const string URL_TEST_CARDS = "https://developers.xsolla.com/api/v2/pay-station/#api_payment_ui_test_cards";

		public event Action<string> OpenUrlEvent;

		[SerializeField] private GameObject registrationButton;
		[SerializeField] private GameObject paystationDocsButton;
		[SerializeField] private GameObject testCardsButton;

		public void Init()
		{
			EnableInfoButton(registrationButton, URL_REGISTRATION);
			EnableInfoButton(paystationDocsButton, URL_DOCUMENTATION);
			EnableInfoButton(testCardsButton, URL_TEST_CARDS);
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