using System.IO;
using UnityEditor;
using UnityEngine;
using Xsolla.Demo;
using Xsolla.PayStation;

namespace Xsolla.Core
{
	public class XsollaSettings : ScriptableObject
	{
		private const string SettingsAssetName = "XsollaSettings";
		private const string SettingsAssetPath = "Resources/";
		private const string SettingsAssetExtension = ".asset";

		private static XsollaSettings _instance;

		[SerializeField] private string playfabTitleId = Constants.DEFAULT_PLAYFAB_TITLE_ID;
		[SerializeField] private uint simplifyProjectId = Constants.DEFAULT_SIMPLIFY_PROJECT_ID;
		[SerializeField] private bool isSandbox = true;
		[SerializeField] private bool inAppBrowserEnabled = true;
		[SerializeField] private PaystationTheme paystationTheme = PaystationTheme.Dark;

		public static string PlayfabTitleId
		{
			get => Instance.playfabTitleId;
			set
			{
				Instance.playfabTitleId = value;
				MarkAssetDirty();
			}
		}

		public static uint SimplifyProjectId
		{
			get => Instance.simplifyProjectId;
			set
			{
				Instance.simplifyProjectId = value;
				MarkAssetDirty();
			}
		}

		public static bool IsSandbox
		{
			get => Instance.isSandbox;
			set
			{
				Instance.isSandbox = value;
				MarkAssetDirty();
			}
		}

		public static bool InAppBrowserEnabled
		{
			get => Instance.inAppBrowserEnabled;
			set
			{
				Instance.inAppBrowserEnabled = value;
				MarkAssetDirty();
			}
		}

		public static PaystationTheme PaystationTheme
		{
			get => Instance.paystationTheme;
			set
			{
				Instance.paystationTheme = value;
				MarkAssetDirty();
			}
		}

		public static XsollaSettings Instance
		{
			get
			{
				_instance = _instance ? _instance : Resources.Load(SettingsAssetName) as XsollaSettings;
				if (_instance != null) return _instance;
				_instance = CreateInstance<XsollaSettings>();
				SaveAsset(Path.Combine(GetSdkPath(), SettingsAssetPath), SettingsAssetName);

				return _instance;
			}
		}

		private static string GetSdkPath()
		{
			return GetAbsoluteSdkPath().Replace("\\", "/").Replace(Application.dataPath, "Assets");
		}

		private static string GetAbsoluteSdkPath()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(FindEditor(Application.dataPath)));
		}

		private static string FindEditor(string path)
		{
			foreach (var d in Directory.GetDirectories(path))
			{
				foreach (var f in Directory.GetFiles(d))
				{
					if (f.Contains("XsollaSettingsEditor.cs"))
					{
						return f;
					}
				}

				var rec = FindEditor(d);
				if (rec != null)
				{
					return rec;
				}
			}

			return null;
		}

		private static void SaveAsset(string directory, string name)
		{
#if UNITY_EDITOR
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			AssetDatabase.CreateAsset(Instance, directory + name + SettingsAssetExtension);
			AssetDatabase.Refresh();
#endif
		}

		private static void MarkAssetDirty()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
#endif
		}
	}
}