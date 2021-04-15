using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModelBuilder : MonoBehaviour
{
	enum Mode { None, Add, Remove }

	const string blockLayerName = "Block";

	[SerializeField] Material placementGuideMaterial;
	[SerializeField] Material removalHighlightMaterial;
	[SerializeField] Material voxelMaterial;

	public Voxel[] model => voxelGameObjectMap.Keys.ToArray();

	GameObject highlightedForRemoval;
	Camera mainCamera;
	Mode mode;
	GameObject placementGuide;
	readonly Dictionary<Voxel, GameObject> voxelGameObjectMap = new Dictionary<Voxel, GameObject>();

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
		else if (Input.GetKeyDown(KeyCode.C))
		{
			Clear();
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

	public void Load(Voxel[] model)
	{
		foreach (var voxel in model)
		{
			AddVoxel(voxel);
		}
	}

	void AddMode()
	{
		// Ensure no block remains highlighted from Remove mode.
		Unhighlight();

		// Place a block on mouse click.
		if (Input.GetMouseButtonDown(0))
		{
			if (placementGuide == null)
			{
				return;
			}

			var voxel = new Voxel(placementGuide.transform.position);
			AddVoxel(voxel);
			return;
		}

		// Draw a guide based on the player's mouse position.
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out var hit))
		{
			if (placementGuide == null)
			{
				placementGuide = GameObject.CreatePrimitive(PrimitiveType.Cube);
				placementGuide.GetComponent<Renderer>().material = placementGuideMaterial;
				placementGuide.layer = LayerMask.NameToLayer("Ignore Raycast");
				placementGuide.name = "PlacementGuide";
				placementGuide.transform.position = hit.point;
			}

			// Reposition guide.
			placementGuide.transform.position = hit.collider.gameObject.layer == LayerMask.NameToLayer(blockLayerName)
				? hit.collider.transform.position + hit.normal
				: Vector3Int.RoundToInt(hit.point); // On ground
		}
		else
		{
			// Remove existing guide if mouse is no longer hovering over an object.
			if (placementGuide != null)
			{
				Destroy(placementGuide);
			}
		}
	}

	void AddVoxel(Voxel voxel)
	{
		var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		go.GetComponent<Renderer>().material = voxelMaterial;
		go.layer = LayerMask.NameToLayer(blockLayerName);
		go.name = "Voxel";
		go.transform.position = voxel.Position;
		voxelGameObjectMap.Add(voxel, go);
	}

	void Clear()
	{
		Unhighlight();

		foreach (var go in voxelGameObjectMap.Values)
		{
			Destroy(go);
		}

		voxelGameObjectMap.Clear();
	}

	void RemoveMode()
	{
		if (placementGuide != null)
		{
			Destroy(placementGuide);
		}

		// Draw a guide based on the player's mouse position.
		var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		var layerMask = LayerMask.GetMask(blockLayerName);

		if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
		{
			Unhighlight();
			return;
		}

		if (hit.collider.gameObject.layer == LayerMask.NameToLayer(blockLayerName))
		{
			Unhighlight();
			highlightedForRemoval = hit.collider.gameObject;
			highlightedForRemoval.GetComponent<Renderer>().material = removalHighlightMaterial;
		}

		// Remove block on mouse click
		if (Input.GetMouseButtonDown(0))
		{
			var voxel = voxelGameObjectMap.First(x => x.Value == highlightedForRemoval).Key;
			RemoveVoxel(voxel);
		}
	}

	void RemoveVoxel(Voxel voxel)
	{
		var go = voxelGameObjectMap[voxel];
		voxelGameObjectMap.Remove(voxel);
		Destroy(go);
	}

	void Unhighlight()
	{
		// Skip out early if no block is highlighted.
		if (highlightedForRemoval == null)
		{
			return;
		}

		highlightedForRemoval.GetComponent<Renderer>().material = voxelMaterial;
		highlightedForRemoval = null;
	}
}
