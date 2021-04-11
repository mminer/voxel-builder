using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField] Material removalHighlight;

	public Voxel Voxel { get; set; }

	Material originalMaterial;

	void Awake()
	{
		originalMaterial = GetComponent<Renderer>().material;
	}

	public void ClearRemovalHighlight()
	{
		GetComponent<Renderer>().material = originalMaterial;
	}

	public void HighlightForRemoval()
	{
		GetComponent<Renderer>().material = removalHighlight;
	}
}
