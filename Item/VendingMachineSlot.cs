using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineSlot : MonoBehaviour {

	public GameObject spriteGameObject;

	public Sprite[] slotShakingSprites;
	public Sprite[] slotSprites;	// 0: false, 1: true;

	public int currentIndex = 0;

	private SpriteRenderer spriteRenderer;

	private bool isShaking;

	private float currentTime;
	private float slotChangeTime = 0.05f;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		isShaking = false;
	}

	public void StartShaking () {
		isShaking = true;

		spriteRenderer.sprite = slotShakingSprites [currentIndex];
	}

	void Update () {
		if (isShaking) {
			currentTime += Time.deltaTime;
			if (currentTime > slotChangeTime) {
				currentTime -= slotChangeTime;

				currentIndex++;
				currentIndex = currentIndex % slotShakingSprites.Length;

				spriteRenderer.sprite = slotShakingSprites [currentIndex];
			}
		}
	} 

	public void EndShaking (int i) {
		isShaking = false;

		if (i > 2)
			i = 1;

		spriteRenderer.sprite = slotSprites [i];
	}
}
