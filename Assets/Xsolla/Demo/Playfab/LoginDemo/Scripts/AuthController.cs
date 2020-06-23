using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.ThirdParty.Playfab.Api;

public class AuthController : MonoBehaviour
{
	[SerializeField] private GameObject resetPasswordPanel;
	[SerializeField] private GameObject loginSignUpPanel;
	[SerializeField] private GameObject signUpPanel;
	[SerializeField] private GameObject loginPanel;
	[SerializeField] private GameObject popUpController;

	[SerializeField] private Button openChangePasswordButton;
	[SerializeField] private Button openSignUpBtn;
	[SerializeField] private Button openLoginBtn;
	[SerializeField] private Button closeChangePasswordButton;

	private List<GameObject> _openedPages = new List<GameObject>();

	private void Awake()
	{
		PagesController();
		PagesEvents();
	}

	private void PagesEvents()
	{
		signUpPanel.GetComponent<ISignUp>().OnSuccessfulSignUp = OnLogin;
		signUpPanel.GetComponent<ISignUp>().OnUnsuccessfulSignUp = OnError;
		resetPasswordPanel.GetComponent<IResetPassword>().OnSuccessfulResetPassword = () =>
		{
			OpenPopUp("Password successfully reset", "Please check your email and change the password");
		};
		resetPasswordPanel.GetComponent<IResetPassword>().OnUnsuccessfulResetPassword = OnError;
		loginPanel.GetComponent<ILogin>().OnSuccessfulLogin = OnLogin;
		loginPanel.GetComponent<ILogin>().OnUnsuccessfulLogin = OnError;
	}

	private void PagesController()
	{
		OpenPage(loginSignUpPanel);
		openLoginBtn.GetComponent<IPanelVisualElement>().Select();
		OpenPage(loginPanel);

		popUpController.GetComponent<IPopUpController>().OnReturnToLogin = ReturnToTheLogIn;
		closeChangePasswordButton.onClick.AddListener(ReturnToTheLogIn);
		openChangePasswordButton.onClick.AddListener(() =>
		{
			CloseAll();
			OpenPage(resetPasswordPanel);
		});
		openSignUpBtn.onClick.AddListener(() =>
		{
			CloseAll();
			OpenPage(signUpPanel);
			OpenPage(loginSignUpPanel);
			openSignUpBtn.GetComponent<IPanelVisualElement>().Select();
			openLoginBtn.GetComponent<IPanelVisualElement>().Deselect();
		});
		openLoginBtn.onClick.AddListener(() =>
		{
			CloseAll();
			OpenPage(loginPanel);
			OpenPage(loginSignUpPanel);
			openLoginBtn.GetComponent<IPanelVisualElement>().Select();
			openSignUpBtn.GetComponent<IPanelVisualElement>().Deselect();
		});
	}

	private void ReturnToTheLogIn()
	{
		CloseAll();
		OpenPage(loginSignUpPanel);
		OpenPage(loginPanel);
		openLoginBtn.GetComponent<IPanelVisualElement>().Select();
		openSignUpBtn.GetComponent<IPanelVisualElement>().Deselect();
	}

	private void OpenPage(GameObject page)
	{
		page.GetComponent<IPage>().Open();
		_openedPages.Add(page);
	}

	private void OpenSaved()
	{
		foreach (var page in _openedPages)
		{
			page.GetComponent<IPage>().Open();
		}
	}

	private void CloseAndSave()
	{
		foreach (var page in _openedPages)
		{
			page.GetComponent<IPage>().Close();
		}
	}

	private void CloseAll()
	{
		foreach (var page in _openedPages)
		{
			page.GetComponent<IPage>().Close();
		}

		_openedPages = new List<GameObject>();
	}

	private void OpenPopUp(string message, PopUpWindows popUp)
	{
		CloseAndSave();
		popUpController.GetComponent<IPopUpController>().OnClosePopUp = OpenSaved;
		popUpController.GetComponent<IPopUpController>().ShowPopUp(message, popUp);
		Debug.Log(message);
	}

	private void OpenPopUp(string header, string message)
	{
		CloseAndSave();
		popUpController.GetComponent<IPopUpController>().OnClosePopUp = ReturnToTheLogIn;
		popUpController.GetComponent<IPopUpController>().ShowPopUp(header, message);
		Debug.Log(message);
	}

	private void OnError(Error error)
	{
		var errorMessage = error.ToString();
		OpenPopUp(errorMessage, PopUpWindows.Error);
	}

	private void OnLogin(AuthToken token)
	{
		PlayfabApi.Instance.Token = token;
		SceneManager.LoadScene("Store");
	}
}