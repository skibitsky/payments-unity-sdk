using System;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.Demo;

public class LoginPage : Page, ILogin
{
    [SerializeField] private InputField loginInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Toggle rememberMeChkBox;
    [SerializeField] private Toggle showPasswordToggle;

    private BasicAuth _basicAuth;
    
    public Action<AuthToken> OnSuccessfulLogin { get; set; }
    public Action<Error> OnUnsuccessfulLogin { get; set; }

    private void Start()
    {
        showPasswordToggle.onValueChanged.AddListener((isPasswordHidden) => {
            passwordInputField.contentType = isPasswordHidden ? InputField.ContentType.Password : InputField.ContentType.Standard;
            passwordInputField.ForceLabelUpdate();
        });

        TryAuthBy<SavedTokenAuth>(SavedTokenAuthFailed);
    }

    private void SavedTokenAuthFailed()
    {
        TryBasicAuth();
    }

    private void TryBasicAuth()
    {
        _basicAuth = TryAuthBy<BasicAuth>().SetLoginButton(loginButton);
        _basicAuth.UserAuthErrorEvent += error => OnUnsuccessfulLogin?.Invoke(error);

        ConfigBaseAuth();
    }

    private T TryAuthBy<T>(Action onFailed = null, Action<AuthToken> success = null) where T: MonoBehaviour, ILoginAuthorization
	{
        T auth = gameObject.AddComponent<T>();
        auth.OnSuccess = token => SuccessAuthorization(token, success);
        auth.OnFailed = onFailed;
        return auth;
    }

    private void SuccessAuthorization(AuthToken token, Action<AuthToken> success = null)
    {
        Debug.Log($"SUCCESS Token = {token}");
        if(rememberMeChkBox.isOn)
            AuthTokenHelper.SaveToken(Constants.LAST_SUCCESS_AUTH_TOKEN, token);
        else
            AuthTokenHelper.DeleteToken(Constants.LAST_SUCCESS_AUTH_TOKEN);
        
        success?.Invoke(token);
        OnSuccessfulLogin?.Invoke(token);
    }
    
    private void ConfigBaseAuth()
	{
        _basicAuth.SetUserName(loginInputField.text);
        _basicAuth.SetPassword(passwordInputField.text);

        loginInputField.onValueChanged.AddListener(_basicAuth.SetUserName);
        passwordInputField.onValueChanged.AddListener(_basicAuth.SetPassword);

        LogInHotkeys hotKeys = gameObject.GetComponent<LogInHotkeys>();
        hotKeys.EnterKeyPressedEvent += _basicAuth.SoftwareAuth;
        hotKeys.TabKeyPressedEvent += ChangeFocus;
    }

    private void ChangeFocus()
    {
        if (loginInputField.isFocused)
            passwordInputField.Select();
        else
            loginInputField.Select();
    }
}