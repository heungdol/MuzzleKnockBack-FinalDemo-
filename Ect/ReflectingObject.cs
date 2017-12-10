using UnityEngine;

public class ReflectingObject : MonoBehaviour {

	private Rigidbody starRigidbody;

	private float maxStartAngle = 30;
	private float minStartVelocity = 7.5f;
	private float maxStartVelocity = 10f;

	void Start () {
		starRigidbody = GetComponent<Rigidbody> ();


		float startforce = Random.Range (minStartVelocity, maxStartVelocity);
		float startAngle = Random.Range (maxStartAngle * -1, maxStartAngle);
		Vector3 startVelocity = new Vector3 (Mathf.Sin (startAngle * Mathf.Deg2Rad) * startforce, Mathf.Cos (startAngle * Mathf.Deg2Rad) * startforce, 0);

		starRigidbody.velocity = startVelocity;
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("MovingPlatform") || other.gameObject.CompareTag("ThroughPlatform")) {
			float currentAngle = Vector3.Angle(starRigidbody.velocity.normalized, Vector3.right);
			float nextAngle = currentAngle + Random.Range (-45, 45);
			float nextForce = Random.Range (minStartVelocity, maxStartVelocity);
			starRigidbody.velocity = new Vector3 (Mathf.Cos (nextAngle * Mathf.Deg2Rad) * nextForce, Mathf.Sin (nextAngle * Mathf.Deg2Rad) * nextForce, 0);
		}
	}
}
