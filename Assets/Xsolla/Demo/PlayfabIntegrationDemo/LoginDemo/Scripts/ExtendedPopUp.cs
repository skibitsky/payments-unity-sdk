using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExtendedPopUp : PopUp, IExtendedPopUp
{
	[SerializeField] private Text headerText;
	[SerializeField] private Button returnToLoginButton;

	public UnityAction OnReturnToLogin
	{
		set => returnToLoginButton.onClick.AddListener(value);
	}

	protected override void Awake()
	{
		base.Awake();
		returnToLoginButton.onClick.AddListener(Close);
	}

	public void ShowPopUp(string header, string message)
	{
		headerText.text = header;
		ShowPopUp(message);
	}
}