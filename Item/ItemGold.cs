using UnityEngine;

public class ItemGold : MonoBehaviour {
	private int goldAmount = 500;

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerInfo playerInfo = other.GetComponent<PlayerInfo> ();
			playerInfo.AddGold (goldAmount);

			//UIInformManager uiInform = FindObjectOfType<UIInformManager> ();
			//uiInform.StartInforming ("GOLD +500");

			Destroy (gameObject);
		}
	}
}
