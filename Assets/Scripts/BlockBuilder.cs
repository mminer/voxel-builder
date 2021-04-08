using UnityEngine;

public class BlockBuilder : MonoBehaviour
{
	static readonly GUILayoutOption materialThumbnailWidth = GUILayout.Width(32);
	static readonly GUILayoutOption materialThumbnailHeight = GUILayout.Height(32);

	[SerializeField] Material[] materials;

	Texture[] materialThumbnails;

	void Awake()
	{
		GenerateMaterialThumbnails();
	}

	void OnGUI()
	{
		foreach (var thumbnail in materialThumbnails)
		{
			//GUILayout.Label(null, GUILayout.Width(32), GUILayout.Height(32));
			//var rect = GUILayoutUtility.GetLastRect();
			var content = new GUIContent(thumbnail);
			var rect = GUILayoutUtility.GetRect(content, GUI.skin.label, materialThumbnailWidth, materialThumbnailHeight);
			GUI.DrawTexture(rect, thumbnail);
		}
	}

	void GenerateMaterialThumbnails()
	{
		materialThumbnails = new Texture[materials.Length];

		for (var i = 0; i < materials.Length; i++)
		{
			if (materials[i].mainTexture != null)
			{
				continue;
			}

			var texture = new Texture2D(1, 1);
			texture.SetPixels(new [] { materials[i].color });
			texture.Apply();
			materialThumbnails[i] = texture;
		}
	}
}
