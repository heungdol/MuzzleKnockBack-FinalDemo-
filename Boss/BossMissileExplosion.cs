using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissileExplosion : MonoBehaviour {

	void Start () {
		Destroy (gameObject, 0.5f);
	}

	void OnTriggerEnter (Collider other) {
		if (other.GetComponent<FieldLevelCavePixel> ())
			Destroy (other.gameObject);

		if (other.GetComponent<PlayerInfo> ())
			other.GetComponent<PlayerInfo> ().PlayerHurt (gameObject);
	}
}
