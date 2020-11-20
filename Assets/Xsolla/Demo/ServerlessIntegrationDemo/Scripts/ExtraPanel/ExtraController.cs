using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.ServerlessIntegration
{
	public class ExtraController : MonoBehaviour, IExtraPanelController
	{
		[SerializeField] private ExtraPanelInfoButtons infoButtons;

		private void Start()
		{
			infoButtons.OpenUrlEvent += url => BrowserHelper.Instance.Open(url);
		}

		public void Initialize()
		{
			infoButtons.Init();
		}
	}
}