using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VendingMachine : MonoBehaviour {
	public GameObject vendingOutlet;
	public GameObject vendingSprite;
	public GameObject[] vendingMachineSlotGameObjects;

	public AudioClip audioShaking;
	public AudioClip audioYes;

	private AudioSource audioSource;

	public enum VENDINGITEM
	{
		LIFE = 0, STAR = 1
	}
	public VENDINGITEM vendingItem;
	public GameObject[] itemPrefabs;	//0: life, 1: Star
	public Sprite[] vendingSprites;		//0: life, 1: Star

	public int needGoldAmount = 5000;

	private bool isIdle = true;
	private VendingMachineSlot[] vendingMachineSlots;

	void Start () {
		isIdle = true;

		audioSource = GetComponent<AudioSource> ();

		vendingSprite.GetComponent<SpriteRenderer> ().sprite = vendingSprites [(int)vendingItem];

		vendingMachineSlots = new VendingMachineSlot[vendingMachineSlotGameObjects.Length];
		for (int i = 0; i < vendingMachineSlots.Length; i++) {
			vendingMachineSlots [i] = vendingMachineSlotGameObjects [i].GetComponent<VendingMachineSlot> ();
		}
	}

	public void StartVending () {
		PlayerInfo playerInfo = FindObjectOfType<PlayerInfo> ();
		if (isIdle == true) {
			isIdle = false;
			if (playerInfo.GetGold () >= needGoldAmount) {
				playerInfo.SubGold (needGoldAmount);
				vendingSprite.GetComponent<VendingShake> ().Shake ();
				StartCoroutine (Vending ());
			} else {
				UIInformManager ui = FindObjectOfType<UIInformManager> ();
				ui.AddInformList ("NEED MORE GOLD");
				StartCoroutine (Vending2 ());
			}
		}
	}

	IEnumerator Vending () {
		float vendingDelay = 1f;
		float vendingBetDelay = 0.5f;

		audioSource.clip = audioShaking;
		audioSource.loop = true;
		audioSource.Play ();

		int[] slotInts = new int[vendingMachineSlots.Length];

		for (int i = 0; i < vendingMachineSlots.Length; i++) {
			vendingMachineSlots [i].StartShaking ();
			slotInts [i] = Random.Range (0, 2);
		}

		yield return new WaitForSeconds (vendingDelay);

		for (int i = 0; i < vendingMachineSlots.Length; i++) {
			yield return new WaitForSeconds (vendingBetDelay);
			vendingMachineSlots [i].EndShaking (slotInts [i]);
		}

		audioSource.Stop ();

		if (CheckTriple (slotInts)) {
			audioSource.clip = audioYes;
			audioSource.loop = false;
			audioSource.Play ();

			yield return new WaitForSeconds (vendingBetDelay);

			GameObject item = Instantiate (itemPrefabs [(int)vendingItem], vendingOutlet.transform.position, Quaternion.identity) as GameObject;
			item.transform.SetParent (gameObject.transform);
		}

		isIdle = true;
	}

	IEnumerator Vending2 () {
		float vendingDelay = 0.1f;

		isIdle = false;
		yield return new WaitForSeconds (vendingDelay);
		isIdle = true;
	}

	bool CheckTriple (int[] i) {
		for (int j = 0; j < i.Length; j++) {
			if (i [j] == 0)
				return false;
		}
		return true;
	}
}
