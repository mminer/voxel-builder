using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField] Material removalHighlight;

	Material originalMaterial;
	new Renderer renderer;

	public static Block InstantiateFromString(string representation)
	{
		var parts = representation.Split(',');
		float x, y, z;
		float.TryParse(parts[0], out x);
		float.TryParse(parts[1], out y);
		float.TryParse(parts[2], out z);
		var pos = new Vector3(x, y, z);
		return Instantiate(Constants.block, pos, Quaternion.identity);
	}

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

	public override string ToString()
	{
		var position = transform.position;
		return $"{(int)position.x},{(int)position.y},{(int)position.z}";
	}
}
