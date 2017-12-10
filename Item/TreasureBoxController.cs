using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBoxController : MonoBehaviour {
	public Sprite[] treasureBoxSprites;	// 0: close, 1: opened
	public GameObject spriteGameObject;
	public GameObject dropItemsPrefab;
	public GameObject ItemKeyPreafab;
	public ShakeObject shakeObject;
	public ShakeProperties shakeProperties;

	public bool hasAKey;

	private bool isGrounded;
	private bool isDestroyable;
	private bool isOpened;

	private SpriteRenderer spriteRenderer;	
	private Rigidbody boxRigidbody;

	void Start () {
		spriteRenderer = spriteGameObject.transform.GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = treasureBoxSprites [0];

		//shakeObject = GetComponent<ShakeObject> ();
		boxRigidbody = GetComponent<Rigidbody> ();
		boxRigidbody.useGravity = false;
		boxRigidbody.isKinematic = true;

		isGrounded = true;
		isDestroyable = true;
		isOpened = false;

		StartCoroutine (IsDestroyableFalse ());
	}

	void FixedUpdate () {
		CheckIsGrounded ();
	}

	void OnTriggerEnter (Collider other) {
		if (isDestroyable && (other.GetComponent<FieldLevelCavePixel> () || other.GetComponent<FieldLevelGoldPiece> ())) {
			Destroy (other.gameObject);
		}
	}

	void CheckIsGrounded () {
		float gapFromCenter = 0.25f;
		float gapBetWall = 0.45f;
		float raycastRange = 0.25f;

		RaycastHit hitLeft;
		RaycastHit hitRight;

		if (isGrounded) {
			bool left = true;
			bool right = true;
			bool total = true;

			if (Physics.Raycast (transform.position + Vector3.left * gapFromCenter + Vector3.down * 0.3f, Vector3.down, out hitLeft, raycastRange)) {
				if (!(hitLeft.transform.CompareTag ("Wall") || hitLeft.transform.CompareTag ("MovingPlatform"))) {
					left = false;
				}
			} else {
				left = false;
			}

			if (Physics.Raycast (transform.position + Vector3.right * gapFromCenter + Vector3.down * 0.3f, Vector3.down, out hitRight, raycastRange)) {
				if (!(hitRight.transform.CompareTag ("Wall") || hitRight.transform.CompareTag ("MovingPlatform"))) {
					right = false;
				}
			} else {
				right = false;
			}

			total = left || right;

			if (!total) {
				isGrounded = false;
				boxRigidbody.isKinematic = false;
				boxRigidbody.useGravity = true;
			}
		} else {
			if (Physics.Raycast (transform.position + Vector3.left * gapFromCenter + Vector3.down * 0.3f, Vector3.down, out hitLeft, raycastRange)
				&& (hitLeft.transform.CompareTag("Wall") || hitLeft.transform.CompareTag("MovingPlatform"))) {
				isGrounded = true;
				boxRigidbody.useGravity = false;
				boxRigidbody.isKinematic = true;

				transform.position = hitLeft.point + Vector3.right * gapFromCenter + Vector3.up * gapBetWall;
			} else if (Physics.Raycast (transform.position + Vector3.right * gapFromCenter + Vector3.down * 0.3f, Vector3.down, out hitRight, raycastRange)
				&& (hitRight.transform.CompareTag("Wall") || hitRight.transform.CompareTag("MovingPlatform"))) {
				isGrounded = true;
				boxRigidbody.useGravity = false;
				boxRigidbody.isKinematic = true;

				transform.position = hitRight.point + Vector3.left * gapFromCenter + Vector3.up * gapBetWall;
			}
		}
	}

	public void Open () {
		if (isOpened)
			return;

		isOpened = true;

		spriteRenderer.sprite = treasureBoxSprites [1];
		shakeObject.StartShake (shakeProperties);

		if (hasAKey) {
			GameObject key = Instantiate (ItemKeyPreafab, transform.position, Quaternion.identity) as GameObject;
			key.name = "Key";
			key.transform.SetParent (gameObject.transform.parent);
		}

		GameObject items = Instantiate (dropItemsPrefab, transform.position, Quaternion.identity) as GameObject;
		items.name = "Items";
		items.transform.SetParent (gameObject.transform.parent);
	}

	public void HasAKeyTrue () {
		hasAKey = true;
	}

	private IEnumerator IsDestroyableFalse () {
		yield return new WaitForSeconds (0.25f);
		isDestroyable = false;
	}
}