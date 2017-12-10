using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public GameObject playerSpriteGameObject;
	public GameObject rifleGameObject;
	public GameObject explosionParticelPrefab;

	public bool isInvincibility;

	private float invincibilityTime = 1;
	private float invincibilityGapTime = 0.05f;
	private float hurtVelocity = 5;

	private float limitGravity = 15;

	private Vector4 spriteStartColor;
	private Vector4 spriteCurrentColor;

	private RifleController rifleController;
	private RifleInfo rifleInfo;
	private Rigidbody playerRigidbody;
	private SpriteRenderer playerSpriteRenderer;

	private Vector3 startScale;
	private Vector3 currentScale;

	private GameObject currentMovingPlatform;
	private Vector3 movingPlatformOldPosition;
	private Vector3 movingPlatformNewPosition;
	private bool isMovingWithPlatform;

	public GRAVITY currentPlayerGravity;

	IEnumerator invincibilityCoroutine;

	void Start () {
		rifleController = rifleGameObject.GetComponent<RifleController> ();
		rifleInfo = rifleGameObject.GetComponent<RifleInfo> ();
		playerRigidbody = GetComponent<Rigidbody> ();

		startScale = playerSpriteGameObject.transform.localScale;

		isInvincibility = false;

		playerSpriteRenderer = playerSpriteGameObject.GetComponent<SpriteRenderer> ();
		spriteStartColor = playerSpriteRenderer.color;
		spriteCurrentColor = spriteStartColor;

		currentPlayerGravity = GRAVITY.AIR;
		SetPlayerGravity (currentPlayerGravity);
	}

	void Update () {
		if (rifleController.wherePlayerLookAt () == LOOKDIRECTION.RIGHT) {
			currentScale = startScale;
		} else {
			currentScale = new Vector3 (startScale.x * -1, startScale.y, startScale.z);
		}

		playerSpriteGameObject.transform.localScale = currentScale;
	}

	void FixedUpdate () {
		if (isMovingWithPlatform) {
			movingPlatformNewPosition = currentMovingPlatform.transform.position;
			Vector3 gapPosition = movingPlatformNewPosition - movingPlatformOldPosition;

			if (Mathf.Abs (transform.position.y - currentMovingPlatform.transform.position.y) < 0.2f) {
				if ((gapPosition.x > 0 && transform.position.x > currentMovingPlatform.transform.position.x)
					|| (gapPosition.x < 0 && transform.position.x < currentMovingPlatform.transform.position.x)) {
					playerRigidbody.MovePosition (transform.position + gapPosition);
				}

			} else if (transform.position.y > currentMovingPlatform.transform.position.y){
				playerRigidbody.MovePosition (transform.position + gapPosition);
			}

			movingPlatformOldPosition = movingPlatformNewPosition;
		}

		RaycastHit hit;

		if (Physics.Raycast (transform.position, Vector3.up, out hit, 0.3f) && playerRigidbody.velocity.y > 0
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			float buffer = 0.25f;
			playerRigidbody.velocity -= Vector3.up * playerRigidbody.velocity.y * (1 + buffer);
		}

		if (Physics.Raycast (transform.position, Vector3.left, out hit, 0.25f) && playerRigidbody.velocity.x < 0
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			playerRigidbody.velocity -= Vector3.right * playerRigidbody.velocity.x;
		}

		if (Physics.Raycast (transform.position, Vector3.right, out hit, 0.25f) && playerRigidbody.velocity.x > 0
			&& (hit.collider.gameObject.CompareTag("Wall") || hit.collider.gameObject.CompareTag("MovingPlatform"))) {
			playerRigidbody.velocity -= Vector3.right * playerRigidbody.velocity.x;
		}

		if (playerRigidbody.velocity.magnitude > limitGravity) {
			playerRigidbody.velocity = playerRigidbody.velocity.normalized * limitGravity;
		}
	}

	void OnCollisionEnter (Collision other) {
		if (other.gameObject.CompareTag("MovingPlatform")) {
			currentMovingPlatform = other.gameObject;
			movingPlatformOldPosition = currentMovingPlatform.transform.position;
			isMovingWithPlatform = true;
		}
	}

	void OnCollisionExit (Collision other) {
		if (other.gameObject.CompareTag("MovingPlatform")) {
			currentMovingPlatform = null;
			movingPlatformNewPosition = Vector3.zero;
			movingPlatformOldPosition = Vector3.zero;
			isMovingWithPlatform = false;
		}
	}

	public void SetPlayerVelocity (Vector3 velocity) {
		playerRigidbody.velocity = velocity;
	}

	public void SetPlayerVelocityFireForce () {
		Vector3 rotationVector = rifleGameObject.transform.forward * -1;
		Vector3 forceVector = rotationVector * rifleInfo.GetRifleProperties().rifleFixedKnockBackForce;
		playerRigidbody.velocity = forceVector;
	}

	public void SetPlayerVelocityZero () {
		playerRigidbody.velocity = Vector3.zero;
	}

	public bool IsInvincibility () {
		return isInvincibility;
	}

	public void StartHurt (GameObject harmfulGameObject) {
		Vector3 rotationVector = (transform.position - harmfulGameObject.transform.position).normalized;
		playerRigidbody.velocity = rotationVector * hurtVelocity;
	}

	public void StartInvincibility () {
		if (invincibilityCoroutine != null)
			StopCoroutine (invincibilityCoroutine);
		
		invincibilityCoroutine = Invincibility ();
		StartCoroutine (invincibilityCoroutine);
	}

	IEnumerator Invincibility () {
		isInvincibility = true;

		float invincibilityLeftTime = invincibilityTime % invincibilityGapTime * 2;

		for (int i = 0; i < (invincibilityTime / (invincibilityGapTime * 2)); i++) {
			spriteCurrentColor.w = 0;
			playerSpriteRenderer.color = spriteCurrentColor;
			yield return new WaitForSeconds(invincibilityGapTime);

			spriteCurrentColor.w = 255;
			playerSpriteRenderer.color = spriteCurrentColor;
			yield return new WaitForSeconds(invincibilityGapTime);
		}

		yield return new WaitForSeconds(invincibilityLeftTime);

		playerSpriteRenderer.color = spriteStartColor;

		isInvincibility = false;
	}

	public void SetIsInvicibilityTrue () {
		isInvincibility = true;
	}

	public void GameOver () {
		gameObject.SetActive (false);
	}

	public void InstantiateSmokePrefab () {
		GameObject particle = Instantiate (explosionParticelPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (particle, particle.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);
	}

	public void SetPlayerGravity (GRAVITY current) {
		switch (current) {
		case GRAVITY.AIR:
			Physics.gravity = Vector3.up * -15;
			break;
		case GRAVITY.WATER:
			Physics.gravity = Vector3.up * -5;
			break;
		case GRAVITY.SPACE:
			Physics.gravity = Vector3.zero;
			break;
		}
		currentPlayerGravity = current;
	}

	public GRAVITY GetPlayerGravity () {
		return currentPlayerGravity;
	}

	public void SetPlayerComponentsFalse () {
		rifleController.enabled = false;
		rifleInfo.enabled = false;
		playerRigidbody.useGravity = false;
		GetComponent<Collider> ().enabled = false;

		SetPlayerVelocityZero ();
		isMovingWithPlatform = false; 
	}

	public void SetPlayerComponentsTrue () {
		rifleController.enabled = true;
		rifleInfo.enabled = true;
		playerRigidbody.useGravity = true;
		GetComponent<Collider> ().enabled = true;
	}

	public void SetPlayerPosition (Vector3 pos) {
		transform.position = pos;
	}
}

public enum GRAVITY {
	WATER
	, AIR
	, SPACE
}
