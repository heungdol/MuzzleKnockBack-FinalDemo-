using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenText : MonoBehaviour {

	public GameObject UITopPanel;
	public GameObject UIBottomPanel;
	public GameObject UIGameOver;
	public GameObject UIYouWin;

	void Start () {
		UITopPanel.SetActive (true);
		UIBottomPanel.SetActive (false);
		UIGameOver.SetActive (false);
		UIYouWin.SetActive (false);
	}

	public void OnUIBottomPanel () {
		UIBottomPanel.SetActive (true);
	}

	public void GameOver () {
		UIGameOver.SetActive (true);
	}

	public void YouWin () {
		UIYouWin.SetActive (true);
	}
}
