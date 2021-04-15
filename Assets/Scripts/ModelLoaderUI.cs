using UnityEngine;

public class ModelLoaderUI : MonoBehaviour
{
	ModelBuilder modelBuilder;
	string path = "/Users/matthewminer/Desktop/model.dat";

	void Start()
	{
		modelBuilder = GetComponent<ModelBuilder>();
	}

	void OnGUI()
	{
		path = GUILayout.TextArea(path);

		if (GUILayout.Button("Save"))
		{
			var writer = new ModelWriter(path);
			writer.Write(modelBuilder.model);
		}

		if (GUILayout.Button("Load"))
		{
			var reader = new ModelReader(path);
			var model = reader.Read();

			foreach (var voxel in model)
			{
				modelBuilder.AddVoxel(voxel);
			}
		}
	}
}
