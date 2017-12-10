using UnityEngine;

public class PlatformMoving : MonoBehaviour {

	public float currentAngle;
	public float deltaAngle = 90;

	public GameObject startPivot;
	public GameObject endPivot;

	private Vector3 currentLocalPosition;

	void FixedUpdate () {
		currentAngle += Time.deltaTime * deltaAngle;

		if (currentAngle >= 360) {
			currentAngle -= 360;
		}

		if (currentAngle <= 0) {
			currentAngle += 360;
		}

		currentLocalPosition = startPivot.transform.localPosition - (startPivot.transform.localPosition - endPivot.transform.localPosition) * 0.5f
		+ (startPivot.transform.localPosition - endPivot.transform.localPosition) * 0.5f * Mathf.Sin (currentAngle * Mathf.Deg2Rad);

		transform.localPosition = currentLocalPosition;
	}
}
