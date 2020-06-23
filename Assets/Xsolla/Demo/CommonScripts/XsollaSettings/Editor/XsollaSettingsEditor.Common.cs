using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private static void CommonSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Common Settings", EditorStyles.boldLabel);
				XsollaSettings.IsSandbox = EditorGUILayout.Toggle("Enable sandbox?", XsollaSettings.IsSandbox);
				XsollaSettings.InAppBrowserEnabled =
					EditorGUILayout.Toggle("Enable in-app browser?", XsollaSettings.InAppBrowserEnabled);
				XsollaSettings.PaystationTheme =
					(PaystationTheme) EditorGUILayout.EnumPopup("Paystation theme", XsollaSettings.PaystationTheme);
			}

			EditorGUILayout.Space();
		}
	}
}