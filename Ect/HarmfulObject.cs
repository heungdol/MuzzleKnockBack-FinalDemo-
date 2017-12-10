using UnityEngine;

public class HarmfulObject : MonoBehaviour {
	void OnTriggerStay (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.SendMessage("PlayerHurt", this.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
