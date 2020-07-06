using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUp : Page, IPopUp
{
	[SerializeField] private Text messageText;
	[SerializeField] protected Button closeButton;

	public UnityAction OnClose
	{
		set => closeButton.onClick.AddListener(value);
	}

	protected virtual void Awake()
	{
		closeButton.onClick.AddListener(Close);
	}

	public void ShowPopUp(string message)
	{
		messageText.text = message;
		Open();
	}
}