using UnityEngine;

public class FieldLevelGoldPiece : MonoBehaviour {
	public int goldAmount = 1000;

	public void GetGold () {

		PlayerInfo playerInfo = FindObjectOfType<PlayerInfo> ();
		playerInfo.AddGold (goldAmount);

		//UIInformManager uiInform = FindObjectOfType<UIInformManager> ();
		//uiInform.AddInformList ("GOLD +" + goldAmount);

		Destroy (gameObject);
	}
}
