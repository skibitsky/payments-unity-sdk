namespace Xsolla.Demo.SimplifyIntegration
{
	public class SimplifyStoreController : StoreController
	{
		protected override IStoreDemoImplementation GetImplementation()
		{
			return SimplifyDemoImplementation.Instance;
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
			if (SimplifyDemoImplementation.IsExist)
				Destroy(SimplifyDemoImplementation.Instance);
		}
	}
}