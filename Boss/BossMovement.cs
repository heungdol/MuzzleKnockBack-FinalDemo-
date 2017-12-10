using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class BossMovement : MonoBehaviour {

	public GameObject bossSpriteGameObject;

	private bool isMoving = false;

	private float currentAngle;
	private float deltaAngle = 30;

	private float maxPositionX = 10f;
	private float minPositionX = 5;

	private float maxPositionY = 5f;
	private float minPositionY = 1f;

	private float currentPositionX;
	private float currentPositionY;

	private Vector3 startPosition;
	private Vector3 movePosition;
	private Vector3 currentPosition;

	private Vector3 positionVelocity;
	private float positionDelay = 0.5f;

	private Vector3 startScale;
	private Vector3 currentScale;


	void Start () {
		currentAngle = 0;
		startPosition = transform.position;
		startScale = bossSpriteGameObject.transform.localScale;
		ResetPositionXY ();
		StartCoroutine (BossAppear ());
	}

	IEnumerator BossAppear () {
		isMoving = false;
		transform.position = startPosition + Vector3.up * 5;

		while (transform.position.y - 0.05f > startPosition.y) {
			transform.position = Vector3.SmoothDamp (transform.position, startPosition, ref positionVelocity, positionDelay);

			yield return null;
		}

		isMoving = true;
	}

	void FixedUpdate () {
		if (!isMoving)
			return;
		
		currentAngle += Time.deltaTime * deltaAngle;

		if (currentAngle >= 360) {
			currentAngle -= 360;
			ResetPositionXY ();
		}

		if (currentAngle <= 0) {
			currentAngle += 360;
			ResetPositionXY ();
		}

		movePosition.x = Mathf.Sin (currentAngle * Mathf.Deg2Rad) * currentPositionX;
		movePosition.y = Mathf.Sin (currentAngle * 2 * Mathf.Deg2Rad) * currentPositionY;

		if (movePosition.x > 0) {
			currentScale = startScale;
			currentScale.x *= -1;
		} else {
			currentScale = startScale;
		}

		bossSpriteGameObject.transform.localScale = currentScale;

		currentPosition = startPosition + movePosition;
		transform.position = currentPosition;
	}

	void ResetPositionXY () {
		currentPositionX = Random.Range (minPositionX, maxPositionX);
		currentPositionY = Random.Range (minPositionY, maxPositionY);
	}

	public void SetDeltaAngle (float delta) {
		this.deltaAngle = delta;
	}

	public void SetMovementComponentOff () {
		GetComponent<BossMovement> ().enabled = false;
	}

}
