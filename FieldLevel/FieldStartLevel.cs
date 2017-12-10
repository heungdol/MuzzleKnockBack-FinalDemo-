using UnityEngine;

//[RequireComponent(typeof(FieldLevelController))]
public class FieldStartLevel : MonoBehaviour {
	public GameObject playerStartPointGameObject;
	//public GameObject cameraStartPointGameObject;

	void Start () {
		if (playerStartPointGameObject == null)
			return;

		FindObjectOfType<PlayerController> ().gameObject.transform.position = playerStartPointGameObject.transform.position;
		FindObjectOfType<PlayerController> ().gameObject.transform.position -= Vector3.forward * FindObjectOfType<PlayerController> ().gameObject.transform.position.z;

		FindObjectOfType<CameraController> ().gameObject.transform.position = playerStartPointGameObject.transform.position + Vector3.forward * -2;
	}
}
