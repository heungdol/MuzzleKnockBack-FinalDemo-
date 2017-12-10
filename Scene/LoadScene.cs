using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public int loadLvInt;

	private bool isInputalbe;

	void OnEnable () {
		StartCoroutine (Inputable ());
	}

	void Update () {
		if (isInputalbe && Input.GetMouseButtonDown (0)) {
			Dont[] donts = FindObjectsOfType<Dont>();
			for (int i = 0; i < donts.Length; i++) {
				Destroy (donts [i].gameObject);
			}
			SceneManager.LoadScene (loadLvInt);
		}
	}

	IEnumerator Inputable () {
		isInputalbe = false;
		yield return new WaitForSeconds (1f);
		isInputalbe = true;
	}
}
