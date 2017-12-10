using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (VibrationObject))]
[RequireComponent (typeof (Rigidbody))]
public class SpitObject : MonoBehaviour {
	private VibrationObject vibrationObject;
	private Rigidbody rigidbodyObject;

	private bool isCollided = false;

	void Awake () {
		vibrationObject = GetComponent<VibrationObject> ();
		vibrationObject.enabled = false;

		rigidbodyObject = GetComponent<Rigidbody> ();
		rigidbodyObject.useGravity = true;

		float maxSpitAngle = 17.5f;
		float maxSpitForce = 7.5f;
		float minSpitForce = 5f;

		float spitRange = UnityEngine.Random.Range (-maxSpitAngle, maxSpitAngle);
		float spitForce = UnityEngine.Random.Range (minSpitForce, maxSpitForce);

		Vector3 spitVelocity = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * spitRange), Mathf.Cos (Mathf.Deg2Rad * spitRange), 0);
		spitVelocity *= spitForce;
		rigidbodyObject.velocity = spitVelocity;

		StartCoroutine (CollidedTrue ());
	}

	void Update () {
		RaycastHit hit;

		float raycastRange = 0.1f;
		float gapBetWall = 0.5f;

		if (Physics.Raycast(transform.position, Vector3.left, out hit, raycastRange)
			&& (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("MovingPlatform"))) {
			rigidbodyObject.velocity = new Vector3 (0, rigidbodyObject.velocity.y, 0);
			transform.position = hit.point + Vector3.right * gapBetWall;
		}

		if (Physics.Raycast(transform.position, Vector3.right, out hit, raycastRange)
			&& (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("MovingPlatform"))) {
			rigidbodyObject.velocity = new Vector3 (0, rigidbodyObject.velocity.y, 0);
			transform.position = hit.point + Vector3.left * gapBetWall;
		}

		if (Physics.Raycast(transform.position, Vector3.up, out hit, raycastRange)
			&& (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("MovingPlatform"))) {
			rigidbodyObject.velocity = new Vector3 (rigidbodyObject.velocity.x, 0, 0);
			transform.position = hit.point + Vector3.down * gapBetWall;
		}

		if (isCollided 
			&& Physics.Raycast(transform.position, Vector3.down, out hit, raycastRange * 3)
			&& (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("MovingPlatform"))) {
			rigidbodyObject.useGravity = false;
			rigidbodyObject.velocity = Vector3.zero;
			vibrationObject.enabled = true;
		}
	}

	IEnumerator CollidedTrue () {
		isCollided = false;
		yield return new WaitForSeconds (0.25f);
		isCollided = true;
	}

}
