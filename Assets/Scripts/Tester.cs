using UnityEngine;

public class Tester : MonoBehaviour
{
	ModelBuilder modelBuilder;
	string path = "/Users/matthewminer/Desktop/model.dat";

	void Awake()
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
			modelBuilder.Clear();
			var reader = new ModelReader(path);
			var model = reader.Read();

			foreach (var voxel in model)
			{
				modelBuilder.AddVoxel(voxel);
			}
		}
		
		if (GUILayout.Button("Clear"))
		{
			modelBuilder.Clear();
		}
	}
}
