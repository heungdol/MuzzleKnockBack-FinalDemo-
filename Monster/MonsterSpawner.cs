using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	public GameObject[] monsterPrefabs;

	private bool alreayActive = false;
	private PlayerController playerController;

	void Start () {
		playerController = FindObjectOfType<PlayerController> ();
		alreayActive = false;
	}

	void Update () {
		if (alreayActive)
			return;

		float rangeX = 7.5f;
		float rangeY = 6;

		if (Mathf.Abs (playerController.transform.position.x - transform.position.x) < rangeX
		    && Mathf.Abs (playerController.transform.position.y - transform.position.y) < rangeY) {

			alreayActive = true;

			int i = UnityEngine.Random.Range (3, 6);
			float monsterRangeX = 2.5f;
			float monsterRangeY = 2f;

			while (i > 0) { 
				Vector3 spawnPosition = transform.position + new Vector3 (UnityEngine.Random.Range (-monsterRangeX, monsterRangeX), UnityEngine.Random.Range (-monsterRangeY, monsterRangeY), 0);
				GameObject monster = Instantiate (monsterPrefabs [UnityEngine.Random.Range (0, monsterPrefabs.Length)], spawnPosition, Quaternion.identity) as GameObject;
				monster.name = "Monster\t[" + i +"]";
				monster.transform.SetParent (gameObject.transform);
				i--;
			}
		}

	}
}
