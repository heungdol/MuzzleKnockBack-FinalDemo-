using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class BossLevelManager : MonoBehaviour {

	public GameObject startLevelPrefab;
	public GameObject[] easyLevelPrefabs;
	public GameObject[] normalLevelPrefabs;
	public GameObject[] hardLevelPrefabs;
	public GameObject[] bonusLevelPrefabs;

	public GameObject previousLevel;
	public GameObject currentLevel;
	public GameObject nextLevel;

	public GameObject previousLevelPivot;
	public GameObject currentLevelPivot;
	public GameObject nextLevelPivot;

	public UnityEvent changeStartEvent;
	public UnityEvent changeEndEvent;

	private PlayerController playerController;
	private BossInfo bossInfo;

	private int numberOfPassedLevel;
	private int currentDifficult;

	private int currentLevelInt = 0;
	private int nextLevelInt = 0;

	void Awake () {
		playerController = FindObjectOfType<PlayerController> ();
		bossInfo = FindObjectOfType<BossInfo> ();

		numberOfPassedLevel = 0;
		currentDifficult = 1;
	}

	void Start () {
		currentLevelInt = -1;
		nextLevelInt = -1;

		currentLevel = Instantiate (startLevelPrefab, currentLevelPivot.transform.position, Quaternion.identity) as GameObject;
		currentLevel.GetComponent<BossLevelInfo> ().enabled = true;
		playerController.transform.position = currentLevel.GetComponent<BossLevelInfo> ().GetPlayerStartPosition ();
		SetNextLevel();
		nextLevel.transform.position = nextLevelPivot.transform.position;
		nextLevel.GetComponent<BossLevelInfo> ().enabled = false;
		numberOfPassedLevel++;
	}

	public void StartChangeLevel () {
		DestroyAllEnemyBullets ();

		switch (currentDifficult) {
		case 1:
			if (numberOfPassedLevel >= 6)
				currentDifficult++;

			if (bossInfo && bossInfo.GetCurrentLife() < bossInfo.GetMaxLife() * 5 / 6)
				currentDifficult++;
			break;
		case 2:
			if (numberOfPassedLevel >= 18)
				currentDifficult++;

			if (bossInfo && bossInfo.GetCurrentLife() < bossInfo.GetMaxLife() * 0.66f)
				currentDifficult++;
			break;
		}

		numberOfPassedLevel++;

		StartCoroutine (ChangeLevel ());
	}

	void DestroyAllEnemyBullets () {
		GameObject[] bullets = GameObject.FindGameObjectsWithTag ("EnemyBullet");

		for (int i = 0; i < bullets.Length; i++) {
			Destroy (bullets [i]);
		}
	}

	void SetNextLevel () {
		if (Random.Range (0, 20) == 0) {
			nextLevel = Instantiate(bonusLevelPrefabs [Random.Range (0, bonusLevelPrefabs.Length)]) as GameObject;
		} else {
			switch (currentDifficult) {
			case 1:
				do {
					nextLevelInt = Random.Range (0, easyLevelPrefabs.Length);
				} while (currentLevelInt == nextLevelInt);
				nextLevel = Instantiate (easyLevelPrefabs [nextLevelInt]) as GameObject;
				break;
			case 2:
				do {
					nextLevelInt = Random.Range (0, normalLevelPrefabs.Length);
				} while (currentLevelInt == nextLevelInt);
				nextLevel = Instantiate(normalLevelPrefabs [nextLevelInt]) as GameObject;
				break;
			case 3:
				do {
					nextLevelInt = Random.Range (0, hardLevelPrefabs.Length);
				} while (currentLevelInt == nextLevelInt);
				nextLevel = Instantiate(hardLevelPrefabs [nextLevelInt]) as GameObject;
				break;
			}

			currentLevelInt = nextLevelInt;
			nextLevelInt = -1;
		}
	}


	IEnumerator ChangeLevel () {
		changeStartEvent.Invoke ();

		if (previousLevel != null)
			Destroy (previousLevel.gameObject);
		
		currentLevel.transform.position = currentLevelPivot.transform.position;
		currentLevel.GetComponent<BossLevelInfo> ().enabled = false;
		nextLevel.transform.position = nextLevelPivot.transform.position;

		Vector3 currentLevelVelocity = Vector3.zero;
		Vector3 nextLevelVelocity = Vector3.zero;
		Vector3 playerVelocity = Vector3.zero;
		float changingDelay = 0.25f;

		Vector3 playerTargetPosition = currentLevel.transform.position + (nextLevel.GetComponent<BossLevelInfo> ().GetPlayerStartPosition () - nextLevelPivot.transform.position);

		while ((nextLevel.transform.position.x - 0.005f > currentLevelPivot.transform.position.x) && (Vector3.Distance(playerController.transform.position, playerTargetPosition) > 0.005f)) {
			currentLevel.transform.position = Vector3.SmoothDamp(currentLevel.transform.position, previousLevelPivot.transform.position, ref currentLevelVelocity, changingDelay);
			nextLevel.transform.position = Vector3.SmoothDamp(nextLevel.transform.position, currentLevelPivot.transform.position, ref currentLevelVelocity, changingDelay);
			playerController.transform.position = Vector3.SmoothDamp (playerController.transform.position, playerTargetPosition, ref playerVelocity, changingDelay);

			yield return null;
		}

		previousLevel = currentLevel;
		currentLevel = nextLevel;
		currentLevel.transform.position = currentLevelPivot.transform.position;
		SetNextLevel ();
		nextLevel.transform.position = nextLevelPivot.transform.position;
		nextLevel.GetComponent<BossLevelInfo> ().enabled = false;

		yield return new WaitForSeconds (0.1f);

		changeEndEvent.Invoke ();

		currentLevel.GetComponent<BossLevelInfo> ().enabled = true;
	}
}
