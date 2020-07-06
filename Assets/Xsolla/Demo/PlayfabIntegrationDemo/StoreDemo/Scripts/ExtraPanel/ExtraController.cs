using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.Store
{
	public class ExtraController : MonoBehaviour, IExtraPanelController
	{
		[SerializeField] private ExtraPanelAccountButtons accountButtons;
		[SerializeField] private ExtraPanelInfoButtons infoButtons;

		private void Start()
		{
			infoButtons.OpenUrlEvent += url => BrowserHelper.Instance.Open(url);
		}

		public void Initialize()
		{
			accountButtons.Init();
			infoButtons.Init();
		}
	}
}