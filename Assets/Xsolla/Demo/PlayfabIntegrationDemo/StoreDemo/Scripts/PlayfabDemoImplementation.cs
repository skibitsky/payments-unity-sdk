using System;
using Xsolla.Core;

namespace Xsolla.Demo.Store
{
	public partial class PlayfabDemoImplementation : MonoSingleton<PlayfabDemoImplementation>, IStoreDemoImplementation
	{
		private Action<Error> GetErrorCallback(Action<Error> onError)
		{
			return error =>
			{
				StoreDemoPopup.ShowError(error);
				onError?.Invoke(error);
			};
		}
	}
}