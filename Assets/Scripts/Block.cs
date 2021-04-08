using UnityEngine;

public class Block : MonoBehaviour
{
	public Material removalHighlight;

	Material originalMaterial;
	Renderer renderer;

	public static Block InstantiateFromString (string representation)
	{
		var parts = representation.Split(',');
		float x, y, z;
		float.TryParse(parts[0], out x);
		float.TryParse(parts[1], out y);
		float.TryParse(parts[2], out z);
		var pos = new Vector3(x, y, z);
		var block = Instantiate(Constants.block, pos, Quaternion.identity) as Block;
		return block;
	}

	void Awake ()
	{
		renderer = GetComponent<Renderer>();
		originalMaterial = renderer.material;
	}

	public void HighlightForRemoval ()
	{
		renderer.material = removalHighlight;
	}

	public void ClearRemovalHighlight ()
	{
		renderer.material = originalMaterial;
	}

	/// <summary>
	/// Creates a short string representation of the block's attributes.
	/// Format: x,y,z
	/// </summary>
	/// <returns>A representation of the block's attributes.</returns>
	override public string ToString ()
	{
		var str = (int)transform.position.x + "," + (int)transform.position.y + "," + (int)transform.position.z;
		return str;
	}
}
