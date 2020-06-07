using System;
using Playfab;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;

public class BasicAuth : MonoBehaviour, ILoginAuthorization
{
    public Action<AuthToken> OnSuccess { get; set; }
	public Action OnFailed { get; set; }
	
    public event Action<Error> UserAuthErrorEvent;

    private BasicAuthButton _loginButton;
    private string _username = "";
    private string _password = "";

    private void OnDestroy()
	{
		if(_loginButton != null) {
            Destroy(_loginButton);
        }
	}

	private bool IsValidCredentials()
	{
        return
			!string.IsNullOrEmpty(_username) &&
			!string.IsNullOrEmpty(_password) &&
			(_password.Length > 5);
    }

	public void SetUserName(string username)
	{
        _username = username;
    }

    public void SetPassword(string password)
	{
        _password = password;
	}

    public BasicAuth SetLoginButton(Button button)
	{
		_loginButton = gameObject.AddComponent<BasicAuthButton>().
			SetButton(button).
			SetActiveCondition(IsValidCredentials).
			SetHandler(Login);
		return this;
	}

	public void SoftwareAuth()
	{
        _loginButton.SoftwareClick();
	}

    private void Login()
    {
	    PlayfabApi.Auth.SignIn(_username, _password, token => OnSuccess?.Invoke(token), BasicAuthFailed);
    }

	private void BasicAuthFailed(Error error)
	{
        Debug.LogWarning("Basic auth failed! " + error.errorMessage);
        UserAuthErrorEvent?.Invoke(error);
        OnFailed?.Invoke();
	}
}
