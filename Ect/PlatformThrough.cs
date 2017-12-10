using UnityEngine;

public class PlatformThrough : MonoBehaviour {

	private PlayerController playerController;
	private Rigidbody playerRigidbody;
	private BoxCollider platformCollider;

	private bool playerIsStay = false;

	void Start () {
		playerController = FindObjectOfType<PlayerController> ();
		playerRigidbody = playerController.gameObject.GetComponent<Rigidbody> ();
		platformCollider = GetComponent<BoxCollider> ();

		playerIsStay = false;
	}

	void Update () {
		if (playerRigidbody.velocity.y > 0.1f) {
			platformCollider.isTrigger = true;
		} else {
			if (playerIsStay == false) {
				platformCollider.isTrigger = false;
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject == playerController.gameObject) {
			playerIsStay = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject == playerController.gameObject) {
			playerIsStay = false;
		}
	}
}
