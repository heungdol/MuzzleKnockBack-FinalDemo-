using UnityEngine;

public class BulletShell : MonoBehaviour {

	private float rotationAngle;
	private float rigidbodyForce;
	private Rigidbody shellRigidbody;

	void Start () {
		shellRigidbody = GetComponent<Rigidbody> ();

		Vector3 rotationVector = new Vector3 (Random.Range (-1f, 1f), 1, 0);
		rotationVector = rotationVector.normalized;
		rigidbodyForce = Random.Range (5f, 7.5f);
		shellRigidbody.velocity = rotationVector * rigidbodyForce;

		rotationAngle = Random.Range (-180, 180);
	}

	void Update () {
		transform.Rotate (Time.deltaTime * rotationAngle * Vector3.forward);
	}
}
