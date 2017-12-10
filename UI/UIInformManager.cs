using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInformManager : MonoBehaviour {
	public GameObject informPrefab;

	private List<string> informList = new List<string> ();
	private float informDelay = 0.25f;
	private float currentTime = 0;
	private bool canInform = true;

	private AudioSource _audio;

	void Start () {
		_audio = GetComponent<AudioSource> ();

		currentTime = 0;
		canInform = true;
	}

	void Update () {
		if (!canInform) {
			currentTime += Time.deltaTime;
			if (currentTime > informDelay) {
				currentTime = 0;
				canInform = true;
			}
		} else {
			if (informList.Count != 0) {
				Inform (informList [0]);
				informList.RemoveAt (0);
				canInform = false;
			}
		}
	}

	public void AddInformList (string s) {
		informList.Add (s);
	}

	void Inform (string s) {
		_audio.Play ();

		GameObject i = Instantiate (informPrefab, transform.position, Quaternion.identity) as GameObject;
		i.transform.SetParent (gameObject.transform);
		i.GetComponent<RectTransform> ().transform.localPosition = Vector3.zero;
		i.GetComponent<UIInform> ().StartInforming (s);
	}
}
