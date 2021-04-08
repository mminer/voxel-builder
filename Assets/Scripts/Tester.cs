using UnityEngine;

public class Tester : MonoBehaviour
{
	string data = "";
	Vector3 scrollPos;
	
	void OnGUI()
	{
		if (GUILayout.Button("Save"))
		{
			data = BlockPlacement.Save();
			Debug.Log(data);
		}
		
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		data = GUILayout.TextArea(data);
		GUILayout.EndScrollView();
		
		if (GUILayout.Button("Generate From Saved"))
		{
			BlockPlacement.Load(data);
		}
		
		if (GUILayout.Button("Clear"))
		{
			BlockPlacement.Clear();
		}
	}
}
