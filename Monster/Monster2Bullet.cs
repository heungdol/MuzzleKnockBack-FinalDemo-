using UnityEngine;

public class Monster2Bullet : MonoBehaviour {
	public GameObject wallHitEffectPrefab;

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag ("Wall")) {
			Instantiate (wallHitEffectPrefab, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
