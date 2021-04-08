//
// Author: Matthew Miner
//         matthew@matthewminer.com
//         http://matthewminer.com
//
// Copyright (c) 2013
//

using UnityEngine;

/// <summary>
/// Allows the camera to orbit and zoom.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
	#region Public Inspector Variables

	public float horizontalOrbitSpeed = 100f;
	public float verticalOrbitSpeed = 100f;
	public float zoomSpeed = 10f;
	public float minVerticalOrbit = 10f;
	public float maxVerticalOrbit = 80f;

	#endregion

	void Update ()
	{
		Orbit();
		Zoom();
	}

	/// <summary>
	/// Rotates the camera around a central axis.
	/// </summary>
	void Orbit ()
	{
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.RotateAround(Vector3.zero, Vector3.up, -horizontalOrbitSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.RotateAround(Vector3.zero, Vector3.up, horizontalOrbitSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			// Prevent camera from moving below bottom.
			if (transform.rotation.eulerAngles.x > minVerticalOrbit) {
				transform.RotateAround(Vector3.zero, transform.right, -verticalOrbitSpeed * Time.deltaTime);
			}
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			// Prevent camera from moving beyond top.
			if (transform.rotation.eulerAngles.x < maxVerticalOrbit) {
				transform.RotateAround(Vector3.zero, transform.right, verticalOrbitSpeed * Time.deltaTime);
			}
		}
	}

	/// <summary>
	/// Magnifies or zooms out from a point.
	/// </summary>
	void Zoom ()
	{
		if (Input.GetKey(KeyCode.Equals)) {
			transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.Minus)) {
			transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
		}
	}
}
