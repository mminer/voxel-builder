using UnityEngine;

public class Block : MonoBehaviour
{
	[SerializeField] Material removalHighlight;

	Material originalMaterial;
	new Renderer renderer;

	public Voxel Voxel { get; private set; }

	public static Block InstantiateFromVoxel(Voxel voxel)
	{
		var block = Instantiate(Constants.block, voxel.Position, Quaternion.identity);
		block.Voxel = voxel;
		return block;
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
}
