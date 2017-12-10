using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	public GameObject mouseMiddleGameObject;
	public GameObject mouseOutlineGameObject;

	public float currentMaxRange;
	private PlayerController playerController;

	public bool titleScreen;

	void Start () {
		Cursor.visible = false;
		playerController = FindObjectOfType<PlayerController> ();
	}

	public Vector3 GetCurrentMousePosition () {
		Vector3 returnVector3 = Vector3.zero;

		Ray ray = Camera.main.GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		Plane plane = new Plane (Vector3.back, Vector3.zero);
		float rayLenght;

		if (plane.Raycast (ray, out rayLenght)) {
			returnVector3 = ray.GetPoint (rayLenght);
		}

		return returnVector3;
	}

	public void SetCurrentRange (float range) {
		this.currentMaxRange = range;
	}

	void Update () {
		transform.position = GetCurrentMousePosition ();

		if (!titleScreen && playerController) {
			Vector3 currentMouseOutlinePosition = GetCurrentMousePosition ();
			Vector3 rotationVector = (currentMouseOutlinePosition - playerController.gameObject.transform.position).normalized;
			float currentRange = (currentMouseOutlinePosition - playerController.gameObject.transform.position).magnitude;
			currentRange = Mathf.Clamp (currentRange, 0, currentMaxRange);

			Vector3 newMouseOutlinePosition = playerController.gameObject.transform.position + rotationVector * currentRange;
			mouseOutlineGameObject.transform.position = newMouseOutlinePosition;
		}
	}
}
