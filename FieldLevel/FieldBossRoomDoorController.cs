using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(FieldBossRoomDoorInfo))]
public class FieldBossRoomDoorController : MonoBehaviour {
	private FieldBossRoomDoorInfo fieldBossRoomDoorInfo;
	private bool doorIsOpened = false;
	private bool doorIsOpening = false;
	private float doorOpenDelay = 2.5f;

	private AudioSource audioSource;

	public Sprite doorOpenedSprite;
	public GameObject smokeEffectPrefab;

	void Awake () {
		audioSource = GetComponent<AudioSource> ();
		fieldBossRoomDoorInfo = GetComponent<FieldBossRoomDoorInfo> ();
		doorIsOpened = false;
	}

	void OnTriggerEnter (Collider other) {
		if (doorIsOpened) {
			DontDestroyOnLoad(FindObjectOfType<PlayerController> ().gameObject);
			DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UICanvas"));
			DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Mouse"));

			FindObjectOfType<PlayerController> ().gameObject.AddComponent<Dont> ();
			GameObject.FindGameObjectWithTag("UICanvas").gameObject.AddComponent<Dont> ();
			GameObject.FindGameObjectWithTag("Mouse").gameObject.AddComponent<Dont> ();

			FindObjectOfType<PlayerController> ().SetPlayerVelocityZero ();

			FindObjectOfType<UIScreenText> ().OnUIBottomPanel ();

			SceneManager.LoadScene(3);
		} else if (!doorIsOpening) {
			if (other.gameObject.CompareTag ("Player")) {
				PlayerInfo playerInfo = other.GetComponent<PlayerInfo> ();

				if (fieldBossRoomDoorInfo.GetKeyInfo () <= playerInfo.GetKey ()) {
					fieldBossRoomDoorInfo.InsertKeys ();
					playerInfo.UseAllKey ();
					doorIsOpening = true;
					StartCoroutine (OpenBossRoomDoor ());
				}
			}
		}
	}

	IEnumerator OpenBossRoomDoor () {
		yield return new WaitForSeconds (doorOpenDelay);
		audioSource.Play ();
		doorIsOpened = true;
		doorIsOpening = false;
		GetComponent<SpriteRenderer> ().sprite = doorOpenedSprite;
		Instantiate (smokeEffectPrefab, transform.position, Quaternion.identity);
		FindObjectOfType<CameraShake> ().LevelChangeShake ();
	}
}
