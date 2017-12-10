using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MONSTERGROUNDED
{
	BOTTOM
	, TOP
	, LEFT
	, RIGHT
}

public class Monster0Controller : MonoBehaviour {
	public GameObject spriteHolderGameObject;
	public GameObject spriteGameObject;

	public Sprite idleSprite;
	public Sprite leapSprite;

	//public UnityEvent leapEvent;
	public UnityEvent landEvent;

	private bool isActive = false;
	private bool isGrounded = false;

	private float laycastCurrent = 0;
	private float laycastDelay = 0.02f;

	public MONSTERGROUNDED monster0Grounded;

	private IEnumerator currentCoroutine;
	private PlayerController playerController;
	private Rigidbody monsterRigidbody;
	private SpriteRenderer spriteRenderer;

	private AudioSource audioSource;
	public AudioClip landClip;

	void Start () {
		isActive = false;
		playerController = FindObjectOfType<PlayerController> ();
		monsterRigidbody = GetComponent<Rigidbody> ();
		spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		float detectPlayerRadiusToAct = 10;
		float limitVelocity = 15f;

		if (Vector3.Distance (playerController.transform.position, transform.position) <= detectPlayerRadiusToAct && !isActive) {
			isActive = true;
			currentCoroutine = IsMoving ();
			StartCoroutine (currentCoroutine);
		}

		if (Vector3.Distance (playerController.transform.position, transform.position) > detectPlayerRadiusToAct && isActive) {
			isActive = false;
			if (currentCoroutine != null)
				StopCoroutine (currentCoroutine);
		}

		if (monsterRigidbody.velocity.magnitude > limitVelocity) {
			Vector3 rot = monsterRigidbody.velocity.normalized;
			monsterRigidbody.velocity = rot * limitVelocity;
		}
	}

	void FixedUpdate () {
		RaycastHit hit;

		if (isGrounded)
			return;

		if (laycastCurrent < laycastDelay)
			return;

		float raycastRange = 0.4f;
		float gapBetWall = 0.3f;

		if (Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange)
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			isGrounded = true;
			monster0Grounded = MONSTERGROUNDED.BOTTOM;

			spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 0);
			spriteRenderer.sprite = idleSprite;

			transform.position = hit.point + Vector3.up * gapBetWall;

			monsterRigidbody.useGravity = false;
			monsterRigidbody.velocity = Vector3.zero;
			monsterRigidbody.isKinematic = true;

			audioSource.clip = landClip;
			audioSource.Play ();

			landEvent.Invoke ();
		}

		if (Physics.Raycast (transform.position, Vector3.up, out hit, raycastRange)
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			isGrounded = true;
			monster0Grounded = MONSTERGROUNDED.TOP;

			spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 180);
			spriteRenderer.sprite = idleSprite;

			transform.position = hit.point + Vector3.down * gapBetWall;

			monsterRigidbody.useGravity = false;
			monsterRigidbody.velocity = Vector3.zero;
			monsterRigidbody.isKinematic = true;

			audioSource.clip = landClip;
			audioSource.Play ();

			landEvent.Invoke ();
		}

		if (Physics.Raycast (transform.position, Vector3.left, out hit, raycastRange)
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			isGrounded = true;
			monster0Grounded = MONSTERGROUNDED.LEFT;

			spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * -90);
			spriteRenderer.sprite = idleSprite;

			transform.position = hit.point + Vector3.right * gapBetWall;

			monsterRigidbody.useGravity = false;
			monsterRigidbody.velocity = Vector3.zero;
			monsterRigidbody.isKinematic = true;

			audioSource.clip = landClip;
			audioSource.Play ();

			landEvent.Invoke ();
		}

		if (Physics.Raycast (transform.position, Vector3.right, out hit, raycastRange)
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			isGrounded = true;
			monster0Grounded = MONSTERGROUNDED.RIGHT;

			spriteHolderGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * 90);
			spriteRenderer.sprite = idleSprite;

			transform.position = hit.point + Vector3.left * gapBetWall;

			monsterRigidbody.useGravity = false;
			monsterRigidbody.velocity = Vector3.zero;
			monsterRigidbody.isKinematic = true;

			audioSource.clip = landClip;
			audioSource.Play ();

			landEvent.Invoke ();
		}
	}

	IEnumerator IsMoving () {
		float delayFromLandToLeap = 1.5f;
		float leapMinVelocity = 10f;
		float leapMaxVelocity = 15f;
		float leapMaxAngle = 30f;

		while (true) {
			laycastCurrent += Time.deltaTime;

			while (isGrounded) {
				yield return new WaitForSeconds (delayFromLandToLeap);
				float angle = 0;
				float leapVelocity = Random.Range (leapMinVelocity, leapMaxVelocity);

				switch (monster0Grounded) {
				case MONSTERGROUNDED.BOTTOM:
					angle = Random.Range (-leapMaxAngle, leapMaxAngle);
					break;
				case MONSTERGROUNDED.TOP:
					angle = Random.Range (-leapMaxAngle, leapMaxAngle) + 180;
					break;
				case MONSTERGROUNDED.RIGHT:
					angle = Random.Range (-leapMaxAngle, leapMaxAngle) - 90;
					break;
				case MONSTERGROUNDED.LEFT:
					angle = Random.Range (-leapMaxAngle, leapMaxAngle) + 90;
					break;
				}

				isGrounded = false;

				monsterRigidbody.useGravity = true;
				monsterRigidbody.velocity = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * angle) * leapVelocity, Mathf.Cos (Mathf.Deg2Rad * angle) * leapVelocity, 0);
				monsterRigidbody.isKinematic = false;

				spriteRenderer.sprite = leapSprite;
				//leapEvent.Invoke ();

				laycastCurrent = 0;
			}
			yield return null;
		}
	}
}
