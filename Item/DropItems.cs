using UnityEngine;

public class DropItems : MonoBehaviour {
	public GameObject[] dropItemPrefabs;	// 0: exp, 1: gold, 2: life

	void Start () {
		DropItem ();

		int maxLeftTime = 30;
		Destroy (gameObject, maxLeftTime);
	}

	void DropItem () {
		int maxNumberOfItems = 5;
		int minNumberOfItems = 1;
		int numberOfItem = UnityEngine.Random.Range (minNumberOfItems, maxNumberOfItems);

		while (numberOfItem > 0) {
			GameObject item = Instantiate(dropItemPrefabs[WhatIndexIsNext()], transform.position, Quaternion.identity) as GameObject;
			item.transform.SetParent (gameObject.transform);

			numberOfItem--;
		}
	}

	int WhatIndexIsNext () {
		int ret = 0;

		int i = UnityEngine.Random.Range (0, 100);

		if (i == 0) {	// life
			ret = 2;
		} else if (1 <= i && i <= 20) {	// gold
			ret = 1;
		} else {	// exp
			ret = 0;
		}

		return ret;
	}
}
