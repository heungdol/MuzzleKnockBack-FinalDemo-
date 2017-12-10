using UnityEngine;

public class MousePosition : MonoBehaviour {
	public Vector3 GetMousePosition () {
		Vector3 returnVector3 = Vector3.zero;

		Ray ray = Camera.main.GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		Plane plane = new Plane (Vector3.back, Vector3.zero);
		float rayLenght;

		if (plane.Raycast (ray, out rayLenght)) {
			returnVector3 = ray.GetPoint (rayLenght);
		}

		return returnVector3;
	}

	void Start () {
		Cursor.visible = false;
	}

	public void Update () {
		transform.position = GetMousePosition();
	}
}
