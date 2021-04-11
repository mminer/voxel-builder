using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField] Material removalHighlight;

	Material originalMaterial;
	new Renderer renderer;

	void Awake()
	{
		renderer = GetComponent<Renderer>();
		originalMaterial = renderer.material;
	}

	public void ClearRemovalHighlight()
	{
		renderer.material = originalMaterial;
	}

	public void HighlightForRemoval()
	{
		renderer.material = removalHighlight;
	}
}
