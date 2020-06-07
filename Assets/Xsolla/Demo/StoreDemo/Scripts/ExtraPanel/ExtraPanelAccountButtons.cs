using UnityEngine;
using UnityEngine.SceneManagement;
using Xsolla.Core;

[AddComponentMenu("Scripts/Xsolla.Store/Extra/ExtraPanelAccountButtons")]
public class ExtraPanelAccountButtons : MonoBehaviour
{
	[SerializeField] private GameObject signOutButton;

	public void Init()
	{
		signOutButton.SetActive(true);
		var btnComponent = signOutButton.GetComponent<SimpleTextButton>();
		btnComponent.onClick = () => {
			AuthTokenHelper.DeleteToken(Constants.LAST_SUCCESS_AUTH_TOKEN);
			SceneManager.LoadScene("Login");
		};
	}
}
