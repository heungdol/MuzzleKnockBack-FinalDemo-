using UnityEngine;

public class RotateObject : MonoBehaviour {

	public float rotationAnglePerSec = 360;

	void Update () {
		transform.Rotate (Vector3.forward * rotationAnglePerSec * Time.deltaTime);
	}
}
