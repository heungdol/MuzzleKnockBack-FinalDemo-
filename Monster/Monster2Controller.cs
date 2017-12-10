using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster2Controller : MonoBehaviour {
	public GameObject spriteHolderGameObject;
	public GameObject spriteGameObject;

	public GameObject monster2BulletPrefab;

	public UnityEvent fireEvent;

	public MONSTERGROUNDED monster2Grounded;

	private Rigidbody monsterRigidbody;
	private PlayerController playerController;
	private IEnumerator currentCoroutine;

	private bool isGrounded = false;
	private bool isActive = false;

	void Start () {
		monsterRigidbody = GetComponent<Rigidbody> ();
		monsterRigidbody.useGravity = true;
		monsterRigidbody.isKinematic = false;

		playerController = FindObjectOfType<PlayerController> ();

		isGrounded = false;
		isActive = false;

		FindSpawnPosition ();
	}

	void FindSpawnPosition () {
		bool find = false;
		int rot = UnityEngine.Random.Range (0, 4);

		float gapBetWall = 0.3f;

		Vector3 spawnPosition = Vector3.zero;
		RaycastHit hit;

		int j = 5;
		while (find == false && j > 0) {
			int i = 0;
			while (i < 4) {
				float raycastRange = 5;
				switch (rot) {
				case 0:	// top
					if (Physics.Raycast (transform.position, Vector3.up, out hit, raycastRange) && (hit.collider.gameObject.CompareTag("Wall"))) {
						spawnPosition = hit.point + Vector3.down * gapBetWall;
						monster2Grounded = MONSTERGROUNDED.TOP;
						spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 180);

						isGrounded = true;
						find = true;
						monsterRigidbody.useGravity = false;
						monsterRigidbody.isKinematic = true;
					}
					break;
				case 1:	// right
					if (Physics.Raycast (transform.position, Vector3.right, out hit, raycastRange) && (hit.collider.gameObject.CompareTag("Wall"))) {
						spawnPosition = hit.point + Vector3.left * gapBetWall;
						monster2Grounded = MONSTERGROUNDED.RIGHT;
						spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 90);

						isGrounded = true;
						find = true;
						monsterRigidbody.useGravity = false;
						monsterRigidbody.isKinematic = true;
					}
					break;
				case 2:	// bottom
					if (Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange) && (hit.collider.gameObject.CompareTag("Wall"))) {
						spawnPosition = hit.point + Vector3.up * gapBetWall;
						monster2Grounded = MONSTERGROUNDED.BOTTOM;
						spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 0);

						isGrounded = true;
						find = true;
						monsterRigidbody.useGravity = false;
						monsterRigidbody.isKinematic = true;
					}
					break;
				case 3:	// left
					if (Physics.Raycast (transform.position, Vector3.left, out hit, raycastRange) && (hit.collider.gameObject.CompareTag("Wall"))) {
						spawnPosition = hit.point + Vector3.right * gapBetWall;
						monster2Grounded = MONSTERGROUNDED.LEFT;
						spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * -90);

						isGrounded = true;
						find = true;
						monsterRigidbody.useGravity = false;
						monsterRigidbody.isKinematic = true;
					}
					break;
				}
				rot = (rot + 1) % 4;
				i++;
			}

			if (find == false) {
				float range = 2.5f;
				float rangeX = UnityEngine.Random.Range (-1, 1);
				float rangeY = UnityEngine.Random.Range (-1, 1);

				rangeX *= range;
				rangeY *= range;

				transform.position = transform.position + new Vector3 (rangeX, rangeY, 0);
			}
			j--;
		}

		if (j > 0) {
			transform.position = spawnPosition;
		}
	}

	void Update () {
		float detectPlayerRadiusToAct = 10;

		if (Vector3.Distance (playerController.transform.position, transform.position) <= detectPlayerRadiusToAct && !isActive) {
			isActive = true;
			currentCoroutine = Firing ();
			StartCoroutine (currentCoroutine);
		}

		if (Vector3.Distance (playerController.transform.position, transform.position) > detectPlayerRadiusToAct && isActive) {
			isActive = false;
			if (currentCoroutine != null)
				StopCoroutine (currentCoroutine);
		}
	}

	void FixedUpdate () {
		RaycastHit hit;

		float raycastRange = 0.5f;
		float gapBetWall = 0.3f;

		if (!isGrounded) {
			if (Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange)
				&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
				monster2Grounded = MONSTERGROUNDED.BOTTOM;

				spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 0);
				transform.position = hit.point + Vector3.up * gapBetWall;

				monsterRigidbody.useGravity = false;
				monsterRigidbody.velocity = Vector3.zero;
				monsterRigidbody.isKinematic = true;

				isGrounded = true;
			}
			if (Physics.Raycast (transform.position, Vector3.up, out hit, raycastRange)
				&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
				monster2Grounded = MONSTERGROUNDED.TOP;

				spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 180);
				transform.position = hit.point + Vector3.down * gapBetWall;

				monsterRigidbody.useGravity = false;
				monsterRigidbody.velocity = Vector3.zero;
				monsterRigidbody.isKinematic = true;

				isGrounded = true;
			}
			if (Physics.Raycast (transform.position, Vector3.left, out hit, raycastRange)
				&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
				monster2Grounded = MONSTERGROUNDED.LEFT;

				spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * -90);
				transform.position = hit.point + Vector3.right * gapBetWall;

				monsterRigidbody.useGravity = false;
				monsterRigidbody.velocity = Vector3.zero;
				monsterRigidbody.isKinematic = true;

				isGrounded = true;
			}
			if (Physics.Raycast (transform.position, Vector3.right, out hit, raycastRange)
				&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
				monster2Grounded = MONSTERGROUNDED.RIGHT;

				spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 90);
				transform.position = hit.point + Vector3.left * gapBetWall;

				monsterRigidbody.useGravity = false;
				monsterRigidbody.velocity = Vector3.zero;
				monsterRigidbody.isKinematic = true;

				isGrounded = true;
			}
		} else {
			if (!Physics.Raycast (transform.position, Vector3.up, out hit, raycastRange)
				&& !Physics.Raycast (transform.position, Vector3.right, out hit, raycastRange)
			    && !Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange)
				&& !Physics.Raycast (transform.position, Vector3.left, out hit, raycastRange)) {
				isGrounded = false;
				monsterRigidbody.useGravity = true;
				monsterRigidbody.isKinematic = false;
			}
		}
	}

	IEnumerator Firing () {
		float fireDelay = UnityEngine.Random.Range(1f, 3f);
		float fireDelay2 = 0.5f;

		while (true) {
			while (isGrounded) {
				yield return new WaitForSeconds (fireDelay);
				fireEvent.Invoke ();
				yield return new WaitForSeconds (fireDelay2);
				Fire ();
			}
			yield return null;
		}
	}

	void Fire () {
		float currentAngle = 0;
		switch (monster2Grounded) {
		case MONSTERGROUNDED.TOP:
			currentAngle = -90;
			break;
		case MONSTERGROUNDED.RIGHT:
			currentAngle = 180;
			break;
		case MONSTERGROUNDED.BOTTOM:
			currentAngle = 90;
			break;
		case MONSTERGROUNDED.LEFT:
			currentAngle = 0;
			break;
		}

		float maxFireAngle = 30;
		float maxFireForce = 10;
		float minFireForce = 5;

		float fireAngle = UnityEngine.Random.Range (-maxFireAngle, maxFireAngle);
		float fireForce = UnityEngine.Random.Range (minFireForce, maxFireForce);

		currentAngle += fireAngle;
		Vector3 fireVelocity = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * currentAngle), Mathf.Sin (Mathf.Deg2Rad * currentAngle), 0);
		fireVelocity *= fireForce;

		GameObject bullet = Instantiate (monster2BulletPrefab, transform.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<Rigidbody> ().velocity = fireVelocity;
	}
}