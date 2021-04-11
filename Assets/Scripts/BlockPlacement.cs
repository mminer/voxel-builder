using System;
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

	bool isGuideVisible => activePlacementGuide != null;

	Transform activePlacementGuide;
	public readonly Dictionary<Voxel, Block> voxelBlockMap = new Dictionary<Voxel, Block>();

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			mode = Mode.Add;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			mode = Mode.Remove;
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
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
			if (isGuideVisible)
			{
				var position = Vector3Int.RoundToInt(activePlacementGuide.position);

				var voxel = new Voxel(
					Convert.ToSByte(position.x),
					Convert.ToSByte(position.y),
					Convert.ToSByte(position.z),
					0,
					0);

				AddVoxel(voxel);
			}
		}

		// Draw a guide based on the player's mouse position.
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out var hit))
		{
			// Create guide if one doesn't yet exist.
			if (!isGuideVisible)
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
			if (isGuideVisible)
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
		if (isGuideVisible)
		{
			Destroy(activePlacementGuide.gameObject);
		}

		// Draw a guide based on the player's mouse position.
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
