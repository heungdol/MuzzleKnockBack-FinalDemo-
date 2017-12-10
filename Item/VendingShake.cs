using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingShake : MonoBehaviour {
	public ShakeProperties shakeProperties;
	private ShakeObject shakeObject;

	void Start () {
		shakeObject = GetComponent<ShakeObject> ();
	}

	public void Shake () {
		shakeObject.StartShake (shakeProperties);
	}
}
