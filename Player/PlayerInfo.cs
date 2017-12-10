using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

	private int life;
	private int exp;
	private int key;
	private int gold;

	public UnityEvent hurtEvent;
	public UnityEvent gameOverEvent;

	public GameObject itemStarPrefab;

	public AudioClip hurtClip;
	//public AudioClip dieClip;

	private int maxLife = 6;
	private int startLife = 3;
	private int maxExp = 10;
	private int maxKey = 3;

	private PlayerController playerController;
//	private RifleInfo rifleInfo;

	private UIStateManager uiLife;
	private UIStateManager uiExp;
	private UIStateManager uiKey;

	private Text goldNumberText;

	private GameObject latestHarmfulGameObject;

	private AudioSource audioSource;

	void Start () {
		life = startLife;
		exp = 0;

		playerController = GetComponent<PlayerController> ();

		uiLife = GameObject.FindWithTag("UILife").GetComponent<UIStateManager>();
		uiLife.ChangeImgs (maxLife, life, 3);

		uiExp = GameObject.FindWithTag("UIExp").GetComponent<UIStateManager>();
		uiExp.ChangeImgs (maxExp, exp, 5);

		uiKey = GameObject.FindWithTag ("UIKey").GetComponent<UIStateManager> ();
		uiKey.ChangeImgs (maxKey, 0, 3);

		goldNumberText = GameObject.FindWithTag ("UIGold").GetComponent<Text>();

		audioSource = GetComponent<AudioSource> ();
	}

	public void AddLife () {
		life++;

		if (life >= maxLife) {
			life = maxLife;
		}

		uiLife.ChangeImgs (life);

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("+LIFE");
	}

	public void PlayerHurt (GameObject g) {
		SetHarmfulGameObject (g);
		SubLife ();
	}

	void SubLife () {
		if (playerController.IsInvincibility () == true)
			return;

		life--;

		playerController.StartHurt (latestHarmfulGameObject);
		hurtEvent.Invoke ();
		uiLife.ChangeImgs (life);

		if (life <= 0) {
			//Debug.Log ("GAME OVER");
			gameOverEvent.Invoke ();
			FindObjectOfType<UIScreenText> ().GameOver ();
			Destroy (gameObject);
			//audioSource.clip = dieClip;
		} else {
			audioSource.clip = hurtClip;
			audioSource.Play();

			FindObjectOfType<CameraShake> ().HurtShake ();
		}
	}

	void SetHarmfulGameObject (GameObject g) {
		latestHarmfulGameObject = g;
	}

	public void AddExp () {
		exp++;

		if (exp >= maxExp) {
			exp -= maxExp;
			//rifleInfo.AddLvRandomly ();
			Instantiate(itemStarPrefab, transform.position, Quaternion.identity);
		}

		uiExp.ChangeImgs (exp);

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("+EXP");
	}

	public void AddKey () {
		key++;
		uiKey.ChangeImgs (key);

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("+KEY");
	}

	public int GetKey () {
		return key;
	}

	public int GetNeedKey () {
		return maxKey;
	}

	public void UseAllKey () {
		key = 0;
		uiKey.ChangeImgs (key);

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("USE ALL KEYS");
	}

	public void AddGold (int n) {
		gold += n;
		goldNumberText.text = gold + "";

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("GOLD +" + n);
	}

	public void SubGold (int n) {
		gold -= n;
		goldNumberText.text = gold + "";

		UIInformManager ui = FindObjectOfType<UIInformManager> ();
		ui.AddInformList("GOLD -" + n);
	}

	public int GetGold () {
		return gold;
	}
}
