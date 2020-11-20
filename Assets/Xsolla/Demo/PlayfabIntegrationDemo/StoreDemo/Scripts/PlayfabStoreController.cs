using UnityEngine;
using UnityEngine.SceneManagement;
using Xsolla.Core.Popup;
using Xsolla.Payments.Api.Playfab;

namespace Xsolla.Demo.Store
{
	public class PlayfabStoreController : StoreController
	{
		protected override IStoreDemoImplementation GetImplementation()
		{
			return PlayfabDemoImplementation.Instance;
		}

		protected override void Awake()
		{
			base.Awake();
			CheckAuth();
		}

		private void CheckAuth()
		{
			var playfabToken = PlayfabApi.Instance.Token;
			if (playfabToken.IsNullOrEmpty())
			{
				Debug.Log("Store demo started without token. Login scene will be launched.");
				SceneManager.LoadScene("Login");
				return;
			}

			Debug.Log($"Store demo started with token {playfabToken}");
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (PlayfabDemoImplementation.IsExist)
				Destroy(PlayfabDemoImplementation.Instance.gameObject);
			if (PopupFactory.IsExist)
				Destroy(PopupFactory.Instance.gameObject);
		}

		protected override void InitStoreUi()
		{
			base.InitStoreUi();

			IExtraPanelController extraController = FindObjectOfType<ExtraController>();
			extraController.Initialize();
		}
	}
}