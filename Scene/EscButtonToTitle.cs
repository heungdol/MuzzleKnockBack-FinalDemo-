using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscButtonToTitle : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Dont[] donts = FindObjectsOfType<Dont>();
			for (int i = 0; i < donts.Length; i++) {
				Destroy (donts [i].gameObject);
			}
			SceneManager.LoadScene (0);
		}
	}
}
