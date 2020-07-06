using UnityEngine;
using Xsolla.Core;

public static class AuthTokenHelper
{
	public static void DeleteToken(string key)
	{
		if (!string.IsNullOrEmpty(key) && PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.DeleteKey(key);
		}
	}

	public static void SaveToken(string key, AuthToken token)
	{
		if (token == null || token.IsNullOrEmpty()) return;
		PlayerPrefs.SetString(key, token.SerializeToJson(false));
	}

	public static bool LoadToken(string key, out AuthToken token)
	{
		var json = PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : string.Empty;
		if (string.IsNullOrEmpty(json))
		{
			token = null;
			return false;
		}

		token = json.DeserializeTo<AuthToken.Playfab>(false);
		if (token == null || token.IsExpired())
		{
			PlayerPrefs.DeleteKey(key);
			token = null;
			return false;
		}

		return true;
	}
}