using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHole : MonoBehaviour {

	public GameObject holeSpriteGameObject;
	public GameObject keySpriteGameObject;

	void Start () {
		holeSpriteGameObject.SetActive (true);
		keySpriteGameObject.SetActive (false);
	}

	public void StartInsert () {
		StartCoroutine (Inserting ());
	}

	IEnumerator Inserting () {
		keySpriteGameObject.SetActive (true);

		yield return new WaitForSeconds (0.5f);

		float totalPercent = 0;
		float spinDuration = 0.25f;
		float spinAngle = 90;

		while (totalPercent < 1.5) {
			totalPercent += Time.deltaTime / spinDuration;
			keySpriteGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * spinAngle * totalPercent);
			yield return null;
		}

		while (totalPercent > 1) {
			totalPercent -= Time.deltaTime / spinDuration;
			keySpriteGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * spinAngle * totalPercent);
			yield return null;
		}

		keySpriteGameObject.transform.rotation = Quaternion.Euler (Vector3.forward * spinAngle * 1);
	}
}
