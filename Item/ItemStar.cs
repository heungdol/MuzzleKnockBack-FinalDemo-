using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemStar : MonoBehaviour {
	private bool getable = false;

	void Start () {
		getable = false;
		StartCoroutine (GetableOn ());
	}

	void OnTriggerEnter (Collider other) {
		if (!getable)
			return;
		
		if (other.gameObject.CompareTag("Player")) {
			RifleInfo rifleInfo = FindObjectOfType<RifleInfo>();
			rifleInfo.AddLvRandomly ();
			Destroy (gameObject);
		}
	}

	IEnumerator GetableOn () {
		float enableGetAfterTime = 1;
		yield return new WaitForSeconds (enableGetAfterTime);
		getable = true;
	}
}
