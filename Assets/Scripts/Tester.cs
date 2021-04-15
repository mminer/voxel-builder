using System.Linq;
using UnityEngine;

public class Tester : MonoBehaviour
{
	ModelBuilder m_ModelBuilder;
	string path = "/Users/matthewminer/Desktop/model.dat";

	void Awake()
	{
		m_ModelBuilder = GetComponent<ModelBuilder>();
	}

	void OnGUI()
	{
		path = GUILayout.TextArea(path);

		if (GUILayout.Button("Save"))
		{
			var model = m_ModelBuilder.voxelBlockMap.Keys.ToArray();
			var writer = new ModelWriter(path);
			writer.Write(model);
		}

		if (GUILayout.Button("Load"))
		{
			m_ModelBuilder.Clear();
			var reader = new ModelReader(path);
			var model = reader.Read();

			foreach (var voxel in model)
			{
				m_ModelBuilder.AddVoxel(voxel);
			}
		}
		
		if (GUILayout.Button("Clear"))
		{
			m_ModelBuilder.Clear();
		}
	}
}
