using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public GameObject targetGameObject;
	public bool bossBattle;

	private float fixedPositionZ = -1;
	private Vector3 fixedPositionVelocity;
	private float fixedPositionDelay = 0.25f;
//	private float finGapBetTarget = 0.1f;

	private float currentCameraSize = 6;
	private float fixedcurrentCameraSizeVelocity;
	private float fixedcurrentCameraSizeDelay = 0.25f;
//	private float finGapBetCurrnetSize = 0.1f;

	private float fixedCameraSize0 = 6;
	private float fixedCameraSize1 = 10;
	private float fixedCameraSize2 = 40;

	private IEnumerator currentCoroutine;

	void Start () {
		StartMove ();
	}

	public void SetCameraSize (int degree) {
		switch (degree) {
		case 0:
			currentCameraSize = fixedCameraSize0;
			break;
		case 1:
			currentCameraSize = fixedCameraSize1;
			break;
		case 2:
			currentCameraSize = fixedCameraSize2;
			break;
		}
		StartMove ();
	}

	public void SetInfoAndStartMove (/*float s,*/ GameObject t) {
		//this.currentCameraSize = s;
		this.targetGameObject = t;
		StartMove ();
	}

	public void SetInfoAndSetPos (/*float s,*/ Vector3 p) {
		//this.currentCameraSize = s;
		this.targetGameObject = null;

		transform.position = p;
		Camera.main.GetComponent<Camera> ().orthographicSize = currentCameraSize;
	}

	void StartMove () {
		if (currentCoroutine != null) 
			StopCoroutine (currentCoroutine);

		currentCoroutine = Moving ();
		StartCoroutine (currentCoroutine);
	}

	IEnumerator Moving () {
		while (/*targetGameObject != null && (Vector3.Distance(gameObject.transform.position, targetGameObject.transform.position) > finGapBetTarget 
			|| Mathf.Abs(currentCameraSize - Camera.main.GetComponent<Camera>().orthographicSize) > finGapBetCurrnetSize)*/true) {

			if (bossBattle && FindObjectOfType<PlayerController> ()) {
				GameObject player = FindObjectOfType<PlayerController> ().gameObject;
				GameObject boss = FindObjectOfType<BossInfo> ().gameObject;

				Vector3 pos = player.transform.position + boss.transform.position;
				pos /= 2;
				pos.z = fixedPositionZ;

				transform.position = Vector3.SmoothDamp (
					transform.position, pos, ref fixedPositionVelocity, fixedPositionDelay);

				float currentSize = Mathf.Abs (player.transform.position.y - boss.transform.position.y);
				currentSize = (currentSize > Mathf.Abs (player.transform.position.x - boss.transform.position.x) / Camera.main.GetComponent<Camera> ().aspect) 
					? currentSize : Mathf.Abs (player.transform.position.x - boss.transform.position.x) / Camera.main.GetComponent<Camera> ().aspect;
				currentSize = Mathf.Clamp (currentSize, 7, 14);

				Camera.main.GetComponent<Camera> ().orthographicSize = currentSize + 2.5f;
			} else {
				if (targetGameObject) {
					transform.position = Vector3.SmoothDamp (
						transform.position, new Vector3 (targetGameObject.transform.position.x, targetGameObject.transform.position.y, fixedPositionZ), ref fixedPositionVelocity, fixedPositionDelay);

					Camera.main.GetComponent<Camera> ().orthographicSize = 
					Mathf.SmoothDamp (Camera.main.GetComponent<Camera> ().orthographicSize, currentCameraSize, ref fixedcurrentCameraSizeVelocity, fixedcurrentCameraSizeDelay);
				}
			}
			
			yield return null;
		}

		/*if (targetGameObject != null)
			transform.position = targetGameObject.transform.position;
		else 
			transform.position = new Vector3 (0, 0, -2);
		
		Camera.main.GetComponent<Camera> ().orthographicSize = currentCameraSize;*/
	}
}
