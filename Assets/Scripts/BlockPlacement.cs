using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
	enum Mode { None, Add, Remove }

	static Mode mode;
	static Block highlightedForRemoval;

	[SerializeField] Block blockPrefab;
	[SerializeField] Transform placementGuide;

	public readonly Dictionary<Voxel, Block> voxelBlockMap = new Dictionary<Voxel, Block>();

	Transform activePlacementGuide;
	Camera mainCamera;

	void Start()
	{
		mainCamera = Camera.main;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			mode = Mode.Add;
		}
		else if (Input.GetKeyDown(KeyCode.Backspace))
		{
			mode = Mode.Remove;
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			mode = Mode.None;
		}

		switch (mode)
		{
			case Mode.Add:
				AddMode();
				break;

			case Mode.Remove:
				RemoveMode();
				break;
		}
	}

	void AddMode()
	{
		// Ensure no block remains highlighted from Remove mode.
		Unhighlight();

		// Place a block on mouse click.
		if (Input.GetMouseButtonDown(0))
		{
			if (activePlacementGuide == null)
			{
				return;
			}

			var voxel = new Voxel(activePlacementGuide.position);
			AddVoxel(voxel);
			return;
		}

		// Draw a guide based on the player's mouse position.
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out var hit))
		{
			if (activePlacementGuide == null)
			{
				activePlacementGuide = Instantiate(placementGuide, hit.point, Quaternion.identity);
			}

			// Reposition guide.
			if (hit.collider.GetComponent<Block>() != null)
			{
				activePlacementGuide.position = hit.collider.transform.position + hit.normal;
			}
			else
			{
				// On ground.
				activePlacementGuide.position = Vector3Int.RoundToInt(hit.point);
			}
		}
		else
		{
			// Remove existing guide if mouse is no longer hovering over an object.
			if (activePlacementGuide != null)
			{
				Destroy(activePlacementGuide.gameObject);
			}
		}
	}

	public void AddVoxel(Voxel voxel)
	{
		var block = Instantiate(blockPrefab, voxel.Position, Quaternion.identity);
		voxelBlockMap.Add(voxel, block);
	}

	void RemoveMode()
	{
		if (activePlacementGuide != null)
		{
			Destroy(activePlacementGuide.gameObject);
		}

		// Draw a guide based on the player's mouse position.
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		var layerMask = 1 << blockPrefab.gameObject.layer;

		if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
		{
			Unhighlight();
			return;
		}

		var hovered = hit.collider.GetComponent<Block>();

		if (hovered != highlightedForRemoval)
		{
			Unhighlight();
			highlightedForRemoval = hovered;
			highlightedForRemoval.HighlightForRemoval();
		}

		// Remove block on mouse click
		if (Input.GetMouseButtonDown(0))
		{
			var voxel = voxelBlockMap.First(x => x.Value == highlightedForRemoval).Key;
			RemoveVoxel(voxel);
		}
	}

	void Unhighlight()
	{
		// Skip out early if no block is highlighted.
		if (highlightedForRemoval == null)
		{
			return;
		}

		highlightedForRemoval.ClearRemovalHighlight();
		highlightedForRemoval = null;
	}

	public void Clear()
	{
		Unhighlight();

		foreach (var voxel in voxelBlockMap.Keys)
		{
			RemoveVoxel(voxel);
		}
	}

	void RemoveVoxel(Voxel voxel)
	{
		var block = voxelBlockMap[voxel];
		voxelBlockMap.Remove(voxel);
		Destroy(block.gameObject);
	}
}
