using System.Linq;
using UnityEngine;

public class Tester : MonoBehaviour
{
	BlockPlacement blockPlacement;
	string path = "/Users/matthewminer/Desktop/model.dat";

	void Awake()
	{
		blockPlacement = GetComponent<BlockPlacement>();
	}

	void OnGUI()
	{
		path = GUILayout.TextArea(path);

		if (GUILayout.Button("Save"))
		{
			var model = blockPlacement.voxelBlockMap.Keys.ToArray();
			ModelIO.WriteModel(path, model);
		}

		if (GUILayout.Button("Load"))
		{
			blockPlacement.Clear();
			var model = ModelIO.ReadModel(path);

			foreach (var voxel in model)
			{
				blockPlacement.AddVoxel(voxel);
			}
		}
		
		if (GUILayout.Button("Clear"))
		{
			blockPlacement.Clear();
		}
	}
}
