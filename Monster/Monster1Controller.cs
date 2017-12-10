using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MONSTERDIRECTION {
	LEFT, RIGHT
}

public class Monster1Controller : MonoBehaviour {
	public MONSTERDIRECTION monster1Direction;
	public GameObject spriteHolderGameObject;

	private Rigidbody monster1Rigidbody;
	private Vector3 startScale;

	private float moveVelocity = 0.5f;
	private float currentVelocityX = 0;
	private bool isGrounded = false;
	private bool isActive = false;

	void Start () {
		monster1Rigidbody = GetComponent<Rigidbody> ();
		startScale = spriteHolderGameObject.transform.localScale;	//left

		isGrounded = false;
		monster1Rigidbody.useGravity = true;

		int d = UnityEngine.Random.Range (0, 2);
		switch (d) {
		case 0:
			monster1Direction = MONSTERDIRECTION.LEFT;
			break;
		case 1:
			monster1Direction = MONSTERDIRECTION.RIGHT;
			break;
		}

		switch (monster1Direction) { 
		case MONSTERDIRECTION.LEFT:
			spriteHolderGameObject.transform.localScale = new Vector3 (startScale.x, startScale.y, startScale.z);
			currentVelocityX = -moveVelocity;
			break;
		case MONSTERDIRECTION.RIGHT:
			spriteHolderGameObject.transform.localScale = new Vector3 (-startScale.x, startScale.y, startScale.z);
			currentVelocityX = moveVelocity;
			break;
		}
	}

	void Update () {
		SetUseGravity ();

		if (isActive) {
			if (!isGrounded) {
				monster1Rigidbody.velocity = new Vector3 (currentVelocityX, monster1Rigidbody.velocity.y, 0);
			} else {
				monster1Rigidbody.velocity = new Vector3 (currentVelocityX, 0, 0);
			}

			switch (monster1Direction) {
			case MONSTERDIRECTION.LEFT:
				if (DetectLeftWall ()) {
					monster1Direction = MONSTERDIRECTION.RIGHT;
					spriteHolderGameObject.transform.localScale = new Vector3 (-startScale.x, startScale.y, startScale.z);
					currentVelocityX = moveVelocity;
				}

				if (isGrounded && DetectLeftBottomWall () == false) {
					monster1Direction = MONSTERDIRECTION.RIGHT;
					spriteHolderGameObject.transform.localScale = new Vector3 (-startScale.x, startScale.y, startScale.z);
					currentVelocityX = moveVelocity;
				}
				break;
			case MONSTERDIRECTION.RIGHT:
				if (DetectRightWall ()) {
					monster1Direction = MONSTERDIRECTION.LEFT;
					spriteHolderGameObject.transform.localScale = new Vector3 (startScale.x, startScale.y, startScale.z);
					currentVelocityX = -moveVelocity;
				}

				if (isGrounded && DetectRightBottomWall () == false) {
					monster1Direction = MONSTERDIRECTION.LEFT;
					spriteHolderGameObject.transform.localScale = new Vector3 (startScale.x, startScale.y, startScale.z);
					currentVelocityX = -moveVelocity;
				}
				break;
			}

			if ((DetectLeftWall () || !DetectLeftBottomWall ()) && (DetectRightWall () || !DetectRightBottomWall ())) {
				monster1Rigidbody.velocity = Vector3.zero;
				isActive = false;
			}
		} else {
			if (!isGrounded) {
				monster1Rigidbody.velocity = new Vector3 (0, monster1Rigidbody.velocity.y, 0);
			} else {
				monster1Rigidbody.velocity = new Vector3 (0, 0, 0);
			}

			if (!((DetectLeftWall () || !DetectLeftBottomWall ()) && (DetectRightWall () || !DetectRightBottomWall ()))) {
				isActive = true;
			}
		}
	}

	bool DetectLeftWall () {
		bool ret = false;
		float raycastRange = 0.5f;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.left, out hit, raycastRange)
		    && (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
			ret = true;
		}

		return ret;
	}

	bool DetectLeftBottomWall () {
		bool ret = false;
		float gap = 0.3f;

		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.left * gap, Vector3.down, out hit, 0.35f)
			&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
			ret = true;
		}

		return ret;
	}

	bool DetectRightWall () {
		bool ret = false;
		float raycastRange = 0.5f;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, Vector3.right, out hit, raycastRange) 
			&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
			ret = true;
		}

		return ret;
	}

	bool DetectRightBottomWall () {
		bool ret = false;
		float gap = 0.3f;

		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.right * gap, Vector3.down, out hit, 0.35f)
			&& (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform"))) {
			ret = true;
		}

		return ret;
	}

	void SetUseGravity () {
		RaycastHit hit;

		float raycastRange = 0.35f;
		float gap = 0.3f;

		if (!isGrounded && Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange) && hit.transform.CompareTag ("Wall")) {
			isGrounded = true;
			monster1Rigidbody.useGravity = false;
			transform.position = hit.point + Vector3.up * gap;
		} else if (isGrounded && !Physics.Raycast (transform.position, Vector3.down, out hit, raycastRange)) {
			isGrounded = false;
			monster1Rigidbody.useGravity = true;
		}
	}
}