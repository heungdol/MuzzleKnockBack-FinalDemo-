using UnityEngine;

public class BossBullet : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("MovingPlatform")) {
			Destroy (this.gameObject);
		}

		if (other.gameObject.CompareTag("Bullet")) {
			Destroy (this.gameObject);
			Destroy (other.gameObject);
		}
	}
}
