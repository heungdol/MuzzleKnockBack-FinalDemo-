using UnityEngine;

public class CircleMovementObject : MonoBehaviour {

	public float deltaAngle = -180;
	public float currentAngle = 0;
	public float movementRadius = 2;

	public GameObject[] movementGameObjects;

	void Update () {
		currentAngle += Time.deltaTime * deltaAngle;

		if (currentAngle > 360) {
			currentAngle -= 360;
		}

		if (currentAngle < 0) {
			currentAngle += 360;
		}

		for (int i = 0; i < movementGameObjects.Length; i++) {
			float realCurrentAngle = currentAngle + (360 / movementGameObjects.Length) * i;
			movementGameObjects [i].transform.position = transform.position
			+ new Vector3 (Mathf.Cos (realCurrentAngle * Mathf.Deg2Rad) * movementRadius, Mathf.Sin (realCurrentAngle * Mathf.Deg2Rad) * movementRadius, 0);
		}
	}
}
