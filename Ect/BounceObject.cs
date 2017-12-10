using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour {
	
	[System.Serializable]
	public class BounceProperties {
		public Vector3 localPosition;
		public Vector3 localScaleRatio;

		public float bounceDelay;
		public float bounceWaitSec;
	}

	public bool activeAtStart = false;
	public bool isLooping = false;
	public bool fixedPosition = false;

	public BounceProperties[] bounceProperties;

	private Vector3 localPositionVelocity;
	private Vector3 localScaleVelocity;

	private Vector3 startLocalPosition;

	private Vector3 startLocalScale;
	private Vector3 currentLocalScale;

	private IEnumerator currentCoroutine;

	void Start () {
		startLocalScale = transform.localScale;
		startLocalPosition = transform.localPosition;

		if (activeAtStart) {
			StartTransformBounce ();
		}
	}

	public void StartTransformBounce () {
		if (currentCoroutine != null)
			StopCoroutine (currentCoroutine);

		currentCoroutine = TransformBounce ();
		StartCoroutine (currentCoroutine);
	}

	IEnumerator TransformBounce () { 
		do {
			for (int i = 0; i < bounceProperties.Length; i++) {
				currentLocalScale = startLocalScale;
				currentLocalScale.x *= bounceProperties [i].localScaleRatio.x;
				currentLocalScale.y *= bounceProperties [i].localScaleRatio.y;
				currentLocalScale.z *= bounceProperties [i].localScaleRatio.z;

				float currentBounceWaitSec = 0;
				while (currentBounceWaitSec <= 1) {
					currentBounceWaitSec += Time.deltaTime / bounceProperties [i].bounceWaitSec;

					if (!fixedPosition) { 
					transform.localPosition = Vector3.SmoothDamp (transform.localPosition, bounceProperties [i].localPosition, 
						ref localPositionVelocity, bounceProperties [i].bounceDelay);
					} else {
						transform.localPosition = startLocalPosition;
					}

					transform.localScale = Vector3.SmoothDamp (transform.localScale, currentLocalScale, 
						ref localScaleVelocity, bounceProperties [i].bounceDelay);

					yield return null;
				}
			}
		} while (isLooping);
	}

}
