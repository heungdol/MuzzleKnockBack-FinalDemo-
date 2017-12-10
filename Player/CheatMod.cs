using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMod : MonoBehaviour {

	public GameObject itemLifePrefab;
	public GameObject itemExpPrefab;
	public GameObject itemKeyPrefab;
	public GameObject itemRingPrefab;

	public bool onCheat = true;

	void Update () {
		if (!onCheat)
			return;

		if (Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				Instantiate (itemLifePrefab, transform.position + Vector3.up * 1, Quaternion.identity);
			}

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				Instantiate (itemExpPrefab, transform.position + Vector3.up * 1, Quaternion.identity);
			}

			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				Instantiate (itemKeyPrefab, transform.position + Vector3.up * 1, Quaternion.identity);
			}

			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				Instantiate (itemRingPrefab, transform.position + Vector3.up * 1, Quaternion.identity);
			}

			if (Input.GetKeyDown (KeyCode.R)) {
				GameObject boss = FindObjectOfType<FieldBossRoomDoorController> ().gameObject;
				transform.position = boss.transform.position - Vector3.up * 2.5f;
			}
		}
	}
}
