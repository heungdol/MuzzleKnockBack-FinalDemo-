using UnityEngine;

public class ItemKey : MonoBehaviour {
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerInfo playerInfo = other.GetComponent<PlayerInfo> ();
			playerInfo.AddKey ();

			//UIInformManager uiInform = FindObjectOfType<UIInformManager> ();
			//uiInform.StartInforming ("+KEY");

			Destroy (gameObject);
		}
	}
}
