using UnityEngine;

public class ItemLife : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerInfo playerInfo = other.GetComponent<PlayerInfo> ();
			playerInfo.AddLife ();

			//UIInformManager uiInform = FindObjectOfType<UIInformManager> ();
			//uiInform.StartInforming ("+LIFE");

			Destroy (gameObject);
		}
	}

}
