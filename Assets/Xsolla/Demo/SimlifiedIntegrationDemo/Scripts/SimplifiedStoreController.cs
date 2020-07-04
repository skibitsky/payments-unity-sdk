namespace Xsolla.Demo.SimplifiedIntegration
{
	public class SimplifiedStoreController : StoreController
	{
		protected override IStoreDemoImplementation GetImplementation()
		{
			return SimplifiedDemoImplementation.Instance;
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
			if (SimplifiedDemoImplementation.IsExist)
				Destroy(SimplifiedDemoImplementation.Instance);
		}
	}
}