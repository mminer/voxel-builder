//
// Author: Matthew Miner
//         matthew@matthewminer.com
//         http://matthewminer.com
//
// Copyright (c) 2013
//

using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
	#region Public Inspector Variables

	public Material[] materials;

	#endregion

	Texture[] materialThumbnails;
	readonly GUILayoutOption materialThumbnailWidth = GUILayout.Width(32);
	readonly GUILayoutOption materialThumbnailHeight = GUILayout.Height(32);

	void Awake ()
	{
		GenerateMaterialThumbnails();
	}

	void OnGUI ()
	{
		foreach (var thumbnail in materialThumbnails) {
			//GUILayout.Label(null, GUILayout.Width(32), GUILayout.Height(32));
			//var rect = GUILayoutUtility.GetLastRect();
			var content = new GUIContent(thumbnail);
			var rect = GUILayoutUtility.GetRect(content, GUI.skin.label, materialThumbnailWidth, materialThumbnailHeight);
			GUI.DrawTexture(rect, thumbnail);
		}

	}

	void GenerateMaterialThumbnails ()
	{
		materialThumbnails = new Texture[materials.Length];

		for (int i = 0; i < materials.Length; i++) {
			if (materials[i].mainTexture == null) {
				var texture = new Texture2D(1, 1);
				texture.SetPixels(new Color[] { materials[i].color });
				texture.Apply();
				materialThumbnails[i] = texture;
			}
		}
	}
}
