using UnityEngine;

/// <summary>
/// Allows the camera to orbit and zoom.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
	[Header("Orbit")]
	[SerializeField] float horizontalOrbitSpeed = 100f;
	[SerializeField] float verticalOrbitSpeed = 100f;
	[SerializeField] float minVerticalOrbit = 10f;
	[SerializeField] float maxVerticalOrbit = 80f;

	[Space]

	[Header("Zoom")]
	[SerializeField] float zoomSpeed = 10f;

	void Update ()
	{
		Orbit();
		Zoom();
	}

	void Orbit()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.RotateAround(Vector3.zero, Vector3.up, -horizontalOrbitSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.RotateAround(Vector3.zero, Vector3.up, horizontalOrbitSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.DownArrow))
		{
			// Prevent camera from moving below bottom.
			if (transform.rotation.eulerAngles.x > minVerticalOrbit)
			{
				transform.RotateAround(Vector3.zero, transform.right, -verticalOrbitSpeed * Time.deltaTime);
			}
		}

		if (Input.GetKey(KeyCode.UpArrow))
		{
			// Prevent camera from moving beyond top.
			if (transform.rotation.eulerAngles.x < maxVerticalOrbit)
			{
				transform.RotateAround(Vector3.zero, transform.right, verticalOrbitSpeed * Time.deltaTime);
			}
		}
	}

	void Zoom ()
	{
		if (Input.GetKey(KeyCode.Equals))
		{
			transform.Translate(Vector3.forward * (zoomSpeed * Time.deltaTime));
		}

		if (Input.GetKey(KeyCode.Minus))
		{
			transform.Translate(Vector3.back * (zoomSpeed * Time.deltaTime));
		}
	}
}
