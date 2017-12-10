using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInformLevel : MonoBehaviour {
	public Text informingText;

	private Image[] images;

	private IEnumerator currentCoroutine;

	void Start () {
		images = GetComponentsInChildren<Image> ();
		SetAlpha (0f);
	}

	public void StartInforming (string t) {
		if (currentCoroutine != null)
			StopCoroutine (currentCoroutine);

		currentCoroutine = Informing (t);
		StartCoroutine (currentCoroutine);
	}

	void SetAlpha (float f) {
		Vector4 color = informingText.color;
		color.w = f;

		informingText.color = color;

		for (int i = 0; i < images.Length; i++) {
			color = images [i].color;
			color.w = f;

			images [i].color = color;
		}
	}

	IEnumerator Informing (string t) {
		informingText.text = t;

		float percent = 0;
		float percentSpeed = 5;

		while (percent < 1) {
			percent += Time.deltaTime * percentSpeed;
			SetAlpha (percent);
			yield return null;
		}
		SetAlpha (1f);

		yield return new WaitForSeconds (2f);

		while (percent > 0) {
			percent -= Time.deltaTime * percentSpeed;
			SetAlpha (percent);
			yield return null;
		}
		SetAlpha (0f);
	}
}
