using UnityEngine;

public class CameraDolly : MonoBehaviour {
	public GameObject trackingGameObject;
	public GameObject[] trackingAxis;	// 0: min, 1: max
	public GameObject movingGameObject;

	private Vector3 velocity;
	private float rangeX;

	void Start () {
		rangeX = trackingAxis[1].transform.position.x - trackingAxis[0].transform.position.x;
	}

	void FixedUpdate () {
		Vector3 currentPos = Vector3.zero;
		float x = trackingGameObject.transform.position.x - trackingAxis [0].transform.position.x;
		x = Mathf.Clamp (x, 0, rangeX);

		currentPos = trackingAxis [0].transform.position + Vector3.right * x + Vector3.forward * -2;
		movingGameObject.transform.position = Vector3.SmoothDamp(movingGameObject.transform.position, currentPos, ref velocity, 0.25f);
	}
}
