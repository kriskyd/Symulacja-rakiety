using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public GameObject camera;

	float zoomSpeed = 1000f;
	float yaw = 0f, pitch = 0f;

	void Start ()
	{

	}

	void Update ()
	{
		zoomSpeed = 1000f;
		if (Input.GetKey (KeyCode.LeftControl))
			zoomSpeed = 10000f;
		else if (Input.GetKey (KeyCode.LeftShift))
			zoomSpeed = 100f;

		float zoom = Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;
		Vector3 direction =  (camera.transform.position - transform.position).normalized;
		if ((camera.transform.position - zoom * direction).x > 25f)
			camera.transform.Translate (-zoom * direction , Space.World);

		if (Input.GetMouseButton (1))
		{

			yaw += Input.GetAxis ("Mouse X") * 10f;
			pitch += Input.GetAxis ("Mouse Y") * 10f;

			transform.eulerAngles = new Vector3 (0, yaw, -pitch);
		}
		if (Input.GetMouseButton (0))
		{
			transform.position -= Vector3.up * Input.GetAxis ("Mouse Y") * 10f;
		}
		//camera.transform.LookAt (transform);
	}
}
