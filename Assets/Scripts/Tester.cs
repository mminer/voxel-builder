using System.Linq;
using UnityEngine;

public class Tester : MonoBehaviour
{
	string path = "/Users/matthewminer/Desktop/model.dat";

	void OnGUI()
	{
		path = GUILayout.TextArea(path);

		if (GUILayout.Button("Save"))
		{
			var model = GameObject
				.FindGameObjectsWithTag("Block")
				.Select(go => go.GetComponent<Block>().Voxel)
				.ToList();

			ModelIO.WriteModel(path, model);
		}

		if (GUILayout.Button("Load"))
		{
			BlockPlacement.Clear();
			var blockPlacement = GetComponent<BlockPlacement>();
			var model = ModelIO.ReadModel(path);

			foreach (var voxel in model)
			{
				blockPlacement.InstantiateBlockFromVoxel(voxel);
			}
		}
		
		if (GUILayout.Button("Clear"))
		{
			BlockPlacement.Clear();
		}
	}
}
