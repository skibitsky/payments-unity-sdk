﻿using System;
using UnityEngine;
using Xsolla.Core;
using Xsolla.Demo;

/// <summary>
/// Class that try load saved token, which is saved after completion
/// last login.
/// </summary>
public class SavedTokenAuth : MonoBehaviour, ILoginAuthorization
{
	public Action<AuthToken> OnSuccess { get; set; }
	public Action OnFailed { get; set; }

	private void Start()
	{
		if (AuthTokenHelper.LoadToken(Constants.LAST_SUCCESS_AUTH_TOKEN, out var token))
			OnSuccess?.Invoke(token);
		else
			OnFailed?.Invoke();
		Destroy(this, 0.1F);
	}
}