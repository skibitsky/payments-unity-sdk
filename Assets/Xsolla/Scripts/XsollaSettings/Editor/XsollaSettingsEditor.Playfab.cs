using UnityEditor;
using UnityEngine;

namespace Xsolla.Core
{
	public partial class XsollaSettingsEditor : UnityEditor.Editor
	{
		private void PlayfabSettings()
		{
			using (new EditorGUILayout.VerticalScope("box"))
			{
				GUILayout.Label("Playfab Settings", EditorStyles.boldLabel);
				XsollaSettings.PlayfabTitleId = EditorGUILayout.TextField(new GUIContent("Playfab title id"),  XsollaSettings.PlayfabTitleId);
			}
			
			EditorGUILayout.Space();
		}
	}
}

