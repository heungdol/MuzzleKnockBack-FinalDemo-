using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldLevelShadow : MonoBehaviour {
	private int numberOfWidth;
	private int numberOfHeight;

	private float fixedDuration = 1;

	private float scaleRatio = 6.25f;

	public Sprite shadowSprite0;
	public Sprite shadowSprite1;
	public Sprite shadowSprite2;

	private List<SpriteRenderer> shadowRenderers = new List<SpriteRenderer>();
	private IEnumerator currentCoroutine;

	public void SetInfo (LEVELTYPE levelType, int h, int w) {
		switch (levelType) {
		case LEVELTYPE._11:
			this.numberOfHeight = h;
			this.numberOfWidth = w;
			break;
		case LEVELTYPE._12:
			this.numberOfHeight = h;
			this.numberOfWidth = w * 2;
			break;
		case LEVELTYPE._21:
			this.numberOfHeight = h * 2;
			this.numberOfWidth = w;
			break;
		case LEVELTYPE._22:
			this.numberOfHeight = h * 2;
			this.numberOfWidth = w * 2;
			break;
		}

		GenerateShadow ();
	}

	void GenerateShadow () {
		for (int y = 0; y < numberOfHeight+2; y++) {
			for (int x = 0; x < numberOfWidth+2; x++) {
				GameObject shadow = new GameObject ();
				shadow.name = "ShadowSprite";

				shadow.transform.localScale = Vector3.one * scaleRatio;
				shadow.transform.SetParent (gameObject.transform);
				shadow.transform.localPosition = 
					new Vector3 ((numberOfWidth+2) / -2f + 0.5f + x, (numberOfHeight+2) / -2f + 0.5f + y, 0);

				Sprite currentSprite = shadowSprite0;
				shadow.AddComponent<SpriteRenderer> ();

				if (y == 0) {
					if (x == 0) {	// B L
						currentSprite = shadowSprite2;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 180);
					} else if (x == numberOfWidth+2 - 1) {	// B R
						currentSprite = shadowSprite2;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * -90);
					} else {
						currentSprite = shadowSprite1;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 180);
					}

				} else if (y == numberOfHeight+2 - 1) {
					if (x == 0) {	// T L
						currentSprite = shadowSprite2;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 90);
					} else if (x == numberOfWidth+2 - 1) {	// T R
						currentSprite = shadowSprite2;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 0);
					} else {
						currentSprite = shadowSprite1;
						shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 0);
					}
				} else if (x == 0) {
					currentSprite = shadowSprite1;
					shadow.transform.rotation = Quaternion.Euler (Vector3.forward * 90);
				} else if (x == numberOfWidth+2 - 1) {
					currentSprite = shadowSprite1;
					shadow.transform.rotation = Quaternion.Euler (Vector3.forward * -90);
				} else {
					currentSprite = shadowSprite0;
				}

				shadow.GetComponent<SpriteRenderer> ().sprite = currentSprite;
				shadow.GetComponent<SpriteRenderer> ().sortingLayerName = "FrontObject";
				shadow.GetComponent<SpriteRenderer> ().sortingOrder = 5;

				shadowRenderers.Add (shadow.GetComponent<SpriteRenderer> ());
			}
		}
	}

	public void StartUnshadow () {
		if (!isActiveAndEnabled)
			return;
		
		if (currentCoroutine != null)
			StopCoroutine (currentCoroutine);
		
		currentCoroutine = Unshadowing ();
		StartCoroutine (currentCoroutine);
	}

	IEnumerator Unshadowing () {
		float currentPercent = 0;

		while (currentPercent < 1) {
			currentPercent += Time.deltaTime / fixedDuration;
			SetAlpha (1 * (1f - currentPercent));
			yield return null;
		}

		//SetAlpha (0);
		gameObject.SetActive(false);
	}

	void SetAlpha (float a) {	
		for (int i = 0; i < shadowRenderers.Count; i++) {
			Vector4 color = shadowRenderers [i].material.color;
			color.w = a;
			shadowRenderers [i].material.color = color;
		}
	}

	public void StartInshadow () {
		if (!isActiveAndEnabled)
			return;
		
		if (currentCoroutine != null)
			StopCoroutine (currentCoroutine);
		
		currentCoroutine = Inshadowing ();
		StartCoroutine (currentCoroutine);
	}

	IEnumerator Inshadowing () {
		SetAlpha (0);

		float currentPercent = 0;

		while (currentPercent < 1) {
			currentPercent += Time.deltaTime / fixedDuration;
			SetAlpha (1 * (currentPercent));
			yield return null;
		}

	}
	public float GetDuration () {
		return fixedDuration;
	}
}
