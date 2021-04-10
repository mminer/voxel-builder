using System;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
	enum Mode { None, Add, Remove }

	static Transform placementGuide;
	static Mode mode;
	static Block highlightedForRemoval;

	static bool isGuideVisible => placementGuide != null;

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

	static void AddMode()
	{
		// Ensure no block remains highlighted from Remove mode.
		Unhighlight();

		// Place a block on mouse click.
		if (Input.GetMouseButtonDown(0))
		{
			if (isGuideVisible)
			{
				var position = Vector3Int.RoundToInt(placementGuide.position);

				var voxel = new Voxel(
					Convert.ToSByte(position.x),
					Convert.ToSByte(position.y),
					Convert.ToSByte(position.z),
					0,
					0);

				Block.InstantiateFromVoxel(voxel);
			}
		}

		// Draw a guide based on the player's mouse position.
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out var hit))
		{
			// Create guide if one doesn't yet exist.
			if (!isGuideVisible)
			{
				placementGuide = Instantiate(Constants.placementGuide, hit.point, Quaternion.identity);
			}

			// Reposition guide.
			if (hit.collider.CompareTag("Block"))
			{
				placementGuide.position = hit.collider.transform.position + hit.normal;
			}
			else
			{
				// On ground.
				placementGuide.position = SnapToGrid(hit.point);
			}
		}
		else
		{
			// Remove existing guide if mouse is no longer hovering over an object.
			if (isGuideVisible)
			{
				Destroy(placementGuide.gameObject);
			}
		}
	}

	static void RemoveMode()
	{
		if (isGuideVisible)
		{
			Destroy(placementGuide.gameObject);
		}

		// Draw a guide based on the player's mouse position.
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Block hovered = null;

		if (Physics.Raycast(ray, out var hit))
		{
			if (hit.collider.CompareTag("Block"))
			{
				hovered = hit.collider.GetComponent<Block>();
			}
		}

		if (hovered == null)
		{
			Unhighlight();
			return;
		}

		if (hovered != highlightedForRemoval)
		{
			Unhighlight();
			highlightedForRemoval = hovered;
			highlightedForRemoval.HighlightForRemoval();
		}

		// Remove block on mouse click
		if (Input.GetMouseButtonDown(0))
		{
			Destroy(highlightedForRemoval.gameObject);
		}
	}

	static void Unhighlight()
	{
		// Skip out early if no block is highlighted.
		if (highlightedForRemoval == null)
		{
			return;
		}

		highlightedForRemoval.ClearRemovalHighlight();
		highlightedForRemoval = null;
	}

	static Vector3 SnapToGrid(Vector3 vector)
	{
		return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
	}

	public static void Clear()
	{
		Unhighlight();
		var blocks = GameObject.FindGameObjectsWithTag("Block");

		foreach (var block in blocks)
		{
			Destroy(block);
		}
	}
}
