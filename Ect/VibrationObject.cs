using UnityEngine;

public class VibrationObject : MonoBehaviour {
	private Vector3 startLocalPosition;
	private Vector3 currentPosition;

	private float currentAngle = 0;

	void Start () {
		startLocalPosition = transform.localPosition;
	}

	void Update () {
		float deltaAngle = 180;
		float maxPositionY = 0.05f;

		currentAngle += Time.deltaTime * deltaAngle;

		if (currentAngle >= 360) {
			currentAngle -= 360;
		}

		if (currentAngle <= 0) {
			currentAngle += 360;
		}

		currentPosition = startLocalPosition + Vector3.up * Mathf.Sin (currentAngle * Mathf.Deg2Rad) * maxPositionY;
		transform.localPosition = currentPosition;
	}
}
