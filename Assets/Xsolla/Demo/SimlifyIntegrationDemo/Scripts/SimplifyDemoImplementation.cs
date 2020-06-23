using System;
using UnityEngine;
using Xsolla.Core;

namespace Xsolla.Demo.SimplifyIntegration
{
	public partial class SimplifyDemoImplementation :
		MonoSingleton<SimplifyDemoImplementation>,
		IStoreDemoImplementation
	{
		public override void Init()
		{
			base.Init();
			InitPurchases();
		}

		private Action<Error> GetErrorCallback(Action<Error> onError)
		{
			return error =>
			{
				StoreDemoPopup.ShowError(error);
				onError?.Invoke(error);
			};
		}

		private T LoadUserData<T>(string key)
		{
			T result = default;
			if (!PlayerPrefs.HasKey(key)) return result;
			var json = PlayerPrefs.GetString(key);
			try
			{
				result = json.DeserializeTo<T>();
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				PlayerPrefs.DeleteKey(key);
				result = default;
			}

			return result;
		}

		private void SaveUserData<T>(string key, T value)
		{
			string json = value.SerializeToJson();
			PlayerPrefs.SetString(key, json);
			Debug.Log($"Save to user data by key = `{key}` and value = `{json}`");
		}
	}
}