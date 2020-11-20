namespace Xsolla.Demo.ServerlessIntegration
{
	public class ServerlessStoreController : StoreController
	{
		protected override IStoreDemoImplementation GetImplementation()
		{
			return ServerlessDemoImplementation.Instance;
		}

		protected override void InitStoreUi()
		{
			base.InitStoreUi();

			IExtraPanelController extraController = FindObjectOfType<ExtraController>();
			extraController?.Initialize();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (ServerlessDemoImplementation.IsExist)
				Destroy(ServerlessDemoImplementation.Instance);
		}
	}
}