using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasuerBoxManager : MonoBehaviour {
	public GameObject treasureBoxPrefab;

	void Start () {
		StartCoroutine (Setting ());
	}

	void SetInfo () {
		List<GameObject> tbPointList = new List<GameObject> ();
		GameObject[] tbPointGameObjects = GameObject.FindGameObjectsWithTag ("TBPoint");

		for (int t = 0; t < tbPointGameObjects.Length; t++) {
			tbPointList.Add (tbPointGameObjects [t].gameObject);
			tbPointGameObjects [t].transform.SetParent (gameObject.transform);
		}

		int i = 10;	// tb number
		int k = 3;	// key number

		while (i > 0) {
			if (tbPointList.Count == 0)
				break;
			
			int j = Random.Range (0, tbPointList.Count);

			GameObject tb = Instantiate (treasureBoxPrefab, tbPointList [j].transform.position, Quaternion.identity) as GameObject;

			if (k > 0) {
				tb.GetComponent<TreasureBoxController> ().HasAKeyTrue ();
				tb.name = "TreasureBoxKey";
				k--;
			} else {
				tb.name = "TreasureBox";
			}

			tb.transform.SetParent (gameObject.transform);

			tbPointList.RemoveAt (j);
			i--;
		}

		for (int t = 0; t < tbPointGameObjects.Length; t++) {
			Destroy (tbPointGameObjects [t].gameObject);
		}
	}

	IEnumerator Setting () {
		yield return new WaitForSeconds (0.25f);
		SetInfo ();
	}
}
