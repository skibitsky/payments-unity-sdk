using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private void XsollaPaystationSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Xsolla Settings", EditorStyles.boldLabel);
				XsollaSettings.IsSandbox = EditorGUILayout.Toggle("Enable sandbox?", XsollaSettings.IsSandbox);
			}

			EditorGUILayout.Space();
		}
	}
}

