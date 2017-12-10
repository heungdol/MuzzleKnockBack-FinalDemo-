using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldScaleFix : MonoBehaviour {

	private float currentCameraSize;
	private float standardCameraSize = 7;

	void Update () {
		currentCameraSize = Camera.main.orthographicSize;
		transform.localScale = Vector3.one * (currentCameraSize / standardCameraSize);
	}
}
