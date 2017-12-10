using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldLevelCavePixel : MonoBehaviour {

	public Sprite[] pixelSprite;
	public GameObject spriteGameObject;

	private int pixelMaxHp = 10;
	private int pixelCurrentHp;

	private SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = pixelSprite [0];

		pixelCurrentHp = pixelMaxHp;
	}

	public void Destroying (int damage) {
		pixelCurrentHp -= damage;

		if (pixelCurrentHp <= 0) {
			Destroy (gameObject);
		}
	}
}
