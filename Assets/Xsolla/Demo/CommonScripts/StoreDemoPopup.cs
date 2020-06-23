using System;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Core.Popup;

namespace Xsolla.Demo
{
	public static class StoreDemoPopup
	{
		public static void ShowSuccess(string message = "") =>
			PopupFactory.Instance.CreateSuccess().SetMessage(message);

		public static IErrorPopup ShowError(Error error)
		{
			Debug.LogError(error);
			return PopupFactory.Instance.CreateError().SetMessage(error.ToString());
		}

		public static void ConsumeConfirmation(
			string itemName,
			uint count,
			Action confirmCase,
			Action cancelCase = null,
			string message = "")
		{
			if (string.IsNullOrEmpty(message))
				message = $"Item '{itemName}' x {count} will be consumed. Are you sure?";
			PopupFactory.Instance.CreateConfirmation().SetMessage(message).SetConfirmCallback(confirmCase)
				.SetCancelCallback(cancelCase);
		}


		public static void ShowConfirm(
			Action confirmCase,
			Action cancelCase = null,
			string message = "Are you sure you want to buy this item?"
		) =>
			PopupFactory.Instance.CreateConfirmation().SetMessage(message).SetConfirmCallback(confirmCase)
				.SetCancelCallback(cancelCase);
	}
}