using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private void ServerlessIntegrationSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Serverless integration settings", EditorStyles.boldLabel);
				XsollaSettings.ServerlessProjectId = (uint) EditorGUILayout.IntField(
					new GUIContent("Xsolla Publisher project id"), (int) XsollaSettings.ServerlessProjectId);
			}

			EditorGUILayout.Space();
		}
	}
}