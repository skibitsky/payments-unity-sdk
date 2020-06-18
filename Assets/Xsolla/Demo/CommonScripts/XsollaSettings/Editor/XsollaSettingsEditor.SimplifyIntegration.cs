using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private void SimplifyIntegrationSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Simplify integration settings", EditorStyles.boldLabel);
				XsollaSettings.SimplifyProjectId = (uint)EditorGUILayout.IntField(
					new GUIContent("Xsolla Publisher project id"),  (int)XsollaSettings.SimplifyProjectId);
			}

			EditorGUILayout.Space();
		}
	}
}

