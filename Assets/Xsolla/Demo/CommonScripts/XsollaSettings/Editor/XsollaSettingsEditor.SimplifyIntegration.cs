using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private void SimplifiedIntegrationSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Simplified integration settings", EditorStyles.boldLabel);
				XsollaSettings.SimplifiedProjectId = (uint) EditorGUILayout.IntField(
					new GUIContent("Xsolla Publisher project id"), (int) XsollaSettings.SimplifiedProjectId);
			}

			EditorGUILayout.Space();
		}
	}
}