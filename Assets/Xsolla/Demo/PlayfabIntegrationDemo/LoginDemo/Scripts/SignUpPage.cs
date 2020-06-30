using System;
using UnityEngine;
using UnityEngine.UI;
using Xsolla.Core;
using Xsolla.Demo;
using Xsolla.Payments.Api.Playfab;
using Xsolla.Payments.Api.Playfab.Auth;

public class SignUpPage : Page, ISignUp
{
	[SerializeField] private InputField login_InputField;
	[SerializeField] private InputField password_InputField;
	[SerializeField] private InputField email_InputField;
	[SerializeField] private Toggle showPassword_Toggle;
	[SerializeField] private Button create_Btn;

	private DateTime _lastClick;
	private const float RATE_LIMIT_MS = Constants.LoginPageRateLimitMs;

	public string SignUpEmail => email_InputField.text;

	public Action<AuthToken> OnSuccessfulSignUp { get; set; }
	public Action<Error> OnUnsuccessfulSignUp { get; set; }

	void Awake()
	{
		login_InputField.onValueChanged.AddListener(delegate { UpdateButtonState(); });
		password_InputField.onValueChanged.AddListener(delegate { UpdateButtonState(); });
		email_InputField.onValueChanged.AddListener(delegate { UpdateButtonState(); });

		_lastClick = DateTime.MinValue;
		create_Btn.onClick.AddListener(SignUp);

		showPassword_Toggle.onValueChanged.AddListener((mood) =>
		{
			password_InputField.contentType = mood ? InputField.ContentType.Password : InputField.ContentType.Standard;
			password_InputField.ForceLabelUpdate();
		});

		create_Btn.onClick.AddListener(SignUp);
	}

	private void Start()
	{
		var hotKeys = gameObject.GetComponent<LogInHotkeys>();
		hotKeys.EnterKeyPressedEvent += SignUp;
		hotKeys.TabKeyPressedEvent += ChangeFocus;

		UpdateButtonState();
	}

	private void ChangeFocus()
	{
		if (login_InputField.isFocused)
		{
			email_InputField.Select();
		}
		else
		{
			if (email_InputField.isFocused)
			{
				password_InputField.Select();
			}
			else
			{
				login_InputField.Select();
			}
		}
	}

	void UpdateButtonState()
	{
		create_Btn.interactable = !string.IsNullOrEmpty(login_InputField.text) &&
		                          !string.IsNullOrEmpty(email_InputField.text) &&
		                          !string.IsNullOrEmpty(password_InputField.text) &&
		                          password_InputField.text.Length > 5;
	}

	public void SignUp()
	{
		TimeSpan ts = DateTime.Now - _lastClick;
		if (ts.TotalMilliseconds > RATE_LIMIT_MS)
		{
			_lastClick += ts;
			if (!string.IsNullOrEmpty(login_InputField.text) && !string.IsNullOrEmpty(email_InputField.text) &&
			    !string.IsNullOrEmpty(password_InputField.text) && password_InputField.text.Length > 5)
			{
				PlayfabApi.Auth.Registration(login_InputField.text, password_InputField.text, email_InputField.text,
					SuccessfulRegistration, OnUnsuccessfulSignUp);
			}
			else
				Debug.Log("Fill all fields");
		}
	}

	private void SuccessfulRegistration(PlayfabUserRegistrationResponseEntity response)
	{
		AuthToken token = new AuthToken.Playfab(response.SessionTicket, response.TokenExpiration);
		OnSuccessfulSignUp?.Invoke(token);
	}
}