using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlink : MonoBehaviour {

	public float blinkTime = 0.5f;
	public Text blinkText;

	private Vector4 startColor;
	private Vector4 currentColor;

	void Start () {
		startColor = blinkText.color;
		currentColor = startColor;
		StartCoroutine (Blinking ());
	}

	IEnumerator Blinking () {
		while (true) {
			currentColor.w = 255;
			blinkText.color = currentColor;
			yield return new WaitForSeconds (blinkTime);

			currentColor.w = 0;
			blinkText.color = currentColor;
			yield return new WaitForSeconds (blinkTime);
		}
	}

}
