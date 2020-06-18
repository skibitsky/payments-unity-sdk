using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xsolla.Demo.Store
{
	public class ExtraPanelAccountButtons : MonoBehaviour
	{
		[SerializeField] private GameObject signOutButton;

		public void Init()
		{
			signOutButton.SetActive(true);
			var btnComponent = signOutButton.GetComponent<SimpleTextButton>();
			btnComponent.onClick = () =>
			{
				AuthTokenHelper.DeleteToken(Constants.LAST_SUCCESS_AUTH_TOKEN);
				SceneManager.LoadScene("Login");
			};
		}
	}
}