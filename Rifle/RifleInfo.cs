using UnityEngine;
using UnityEngine.UI;

public class RifleInfo : MonoBehaviour {

	[System.Serializable]
	public class RifleLevelProperties {
		public int rifleBulletRangeLv;
		public int rifleBulletDamageLv;
		public int rifleBundleBulletNumberLv;
		public int rifleMaxReloadBulletNumberLv;

		public RifleLevelProperties (int rifleBulletRangeLv, int rifleBulletDamageLv, int rifleBundleBulletNumberLv, int rifleMaxReloadBulletNumberLv)
		{
			this.rifleBulletRangeLv = rifleBulletRangeLv;
			this.rifleBulletDamageLv = rifleBulletDamageLv;
			this.rifleBundleBulletNumberLv = rifleBundleBulletNumberLv;
			this.rifleMaxReloadBulletNumberLv = rifleMaxReloadBulletNumberLv;
		}
	}

	public void AddLvRandomly () {
		//int i = Random.Range (0, 4);
		int i = Random.Range (0, 3);

		switch (i) {
		case 0:	// range
			AddRifleBulletRangeLv ();
			uiInformLvUp.AddInformList ("RANGE UP");
			break;
		case 1:	// damage
			AddRifleBulletDamageLv();
			uiInformLvUp.AddInformList ("DAMAGE UP");
			break;
		case 2:	// bundle
			AddRifleBundleBulletNumberLv();
			uiInformLvUp.AddInformList ("BUCKSHOT UP");
			break;
		/*case 3:	// max reload
			AddRifleReloadBulletNumberLv();
			uiInformLvUp.StartInforming ("MAGAZINE UP!");
			break;*/
		}
	}

	[System.Serializable]
	public class RifleProperties
	{
		public float rifleBulletRange;
		public int rifleBulletDamage;
		public int rifleBundleBulletNumber;
		public int rifleMaxReloadBulletNumber;
		public int rifleCurrentReloadBulletNumber;
		public float rifleFixedKnockBackForce;
		public float rifleFixedBulletTime;
		public float rifleFixedFireDelayTime;
		public float rifleFixedReloadTime;
		public float rifleFixedAngleBetBullets;

		public RifleProperties (float rifleBulletRange, int rifleBulletDamage, int rifleBundleBulletNumber, int rifleMaxReloadBulletNumber, int rifleCurrentReloadBulletNumber, float rifleFixedKnockBackForce, float rifleFixedBulletTime, float rifleFixedFireDelayTime, float rifleFixedReloadTime, float rifleFixedAngleBetBullets)
		{
			this.rifleBulletRange = rifleBulletRange;
			this.rifleBulletDamage = rifleBulletDamage;
			this.rifleBundleBulletNumber = rifleBundleBulletNumber;
			this.rifleMaxReloadBulletNumber = rifleMaxReloadBulletNumber;
			this.rifleCurrentReloadBulletNumber = rifleCurrentReloadBulletNumber;

			this.rifleFixedKnockBackForce = rifleFixedKnockBackForce;
			this.rifleFixedBulletTime = rifleFixedBulletTime;
			this.rifleFixedFireDelayTime = rifleFixedFireDelayTime;
			this.rifleFixedReloadTime = rifleFixedReloadTime;
			this.rifleFixedAngleBetBullets = rifleFixedAngleBetBullets;
		}
	}

	private RifleLevelProperties rifleLevelProperties;
	private RifleProperties rifleProperties;

	private MouseOutlineSpritesManager mouseOutlineSpriteManager;
	private MouseController mouseController;

	private UIStateManager uiAmmo;
	private UIInformManager uiInformLvUp;
	private Text lvText;

	private float currentFireTime;
	private float currentReloadTime;

	private AudioSource audioSource;

	public AudioClip[] audioClips;

	void Start () {
		rifleLevelProperties = new RifleLevelProperties (1, 1, 1, 1);
		rifleProperties = new RifleProperties (
			GetRifleBulletRange(rifleLevelProperties.rifleBulletRangeLv)
			, GetRifleBulletDamage(rifleLevelProperties.rifleBulletDamageLv)
			, GetRifleBundleBulletNumber (rifleLevelProperties.rifleBundleBulletNumberLv)
			, GetRifleReloadBulletNumber(rifleLevelProperties.rifleMaxReloadBulletNumberLv)
			, GetRifleReloadBulletNumber(rifleLevelProperties.rifleMaxReloadBulletNumberLv)

			, 10f, 0.5f, 0.2f, 0.35f, 5f);

		mouseOutlineSpriteManager = FindObjectOfType<MouseOutlineSpritesManager>();
		mouseOutlineSpriteManager.SetInfo (/*rifleProperties.rifleBulletRange,*/ rifleProperties.rifleBulletDamage, rifleProperties.rifleBundleBulletNumber/*, gameObject*/);
		mouseOutlineSpriteManager.ResetMouseSprites ();

		mouseController = GameObject.FindWithTag ("Mouse").GetComponent<MouseController> ();

		//uiAmmo = GameObject.FindWithTag("UIAmmo").GetComponent<UIStateManager>();
		//uiAmmo.ChangeImgs (rifleProperties.rifleMaxReloadBulletNumber, rifleProperties.rifleCurrentReloadBulletNumber, rifleProperties.rifleMaxReloadBulletNumber);

		uiInformLvUp = FindObjectOfType<UIInformManager> ();

		MouseInfoReset ();
		mouseController.SetCurrentRange (this.rifleProperties.rifleBulletRange);

		lvText = GameObject.FindWithTag ("UILv").GetComponent<Text>();

		audioSource = GetComponent<AudioSource> ();
	}

	void WriteNewLv () {
		string s0 = "- Range\tLV.";
		string s1 = "- Damage\tLV.";
		string s2 = "- Buck\t\tLV.";

		s0 += rifleLevelProperties.rifleBulletRangeLv;
		s1 += rifleLevelProperties.rifleBulletDamageLv;
		s2 += rifleLevelProperties.rifleBundleBulletNumberLv;

		string t = s0 + "\n" + s1 + "\n" + s2;
		lvText.text = t;
	}

	void MouseInfoReset () {
		mouseOutlineSpriteManager.FixInfo (/*rifleProperties.rifleBulletRange,*/ rifleProperties.rifleBulletDamage, rifleProperties.rifleBundleBulletNumber);
		mouseOutlineSpriteManager.ResetMouseSprites ();
	}

	public void AddRifleBulletRangeLv () {
		rifleLevelProperties.rifleBulletRangeLv++;
		rifleProperties.rifleBulletRange = GetRifleBulletRange (rifleLevelProperties.rifleBulletRangeLv);
		WriteNewLv ();
		mouseController.SetCurrentRange (this.rifleProperties.rifleBulletRange);
	}

	float GetRifleBulletRange (int lv) {
		float startRange = 5f;
		float perRange = 1f;
		float returnRange = startRange + perRange * (lv - 1);
		return returnRange;
	}

	public void AddRifleBulletDamageLv () {
		rifleLevelProperties.rifleBulletDamageLv++;
		rifleProperties.rifleBulletDamage = GetRifleBulletDamage (rifleLevelProperties.rifleBulletDamageLv);
		WriteNewLv ();
		MouseInfoReset ();
	}

	int GetRifleBulletDamage (int lv) {
		int startDamage = 10;
		int perDamage = 2;
		int returnDamage = startDamage + perDamage * (lv - 1);
		return returnDamage;
	}

	public void AddRifleBundleBulletNumberLv () {
		rifleLevelProperties.rifleBundleBulletNumberLv++;
		rifleProperties.rifleBundleBulletNumber = GetRifleBundleBulletNumber (rifleLevelProperties.rifleBundleBulletNumberLv);
		WriteNewLv ();
		MouseInfoReset ();
	}

	int GetRifleBundleBulletNumber (int lv) {
		return lv;
	}

	public void AddRifleReloadBulletNumberLv () {
		rifleLevelProperties.rifleMaxReloadBulletNumberLv++;
		rifleProperties.rifleMaxReloadBulletNumber = GetRifleReloadBulletNumber (rifleLevelProperties.rifleMaxReloadBulletNumberLv);
		uiAmmo.ChangeImgs (rifleProperties.rifleMaxReloadBulletNumber, rifleProperties.rifleCurrentReloadBulletNumber, rifleProperties.rifleMaxReloadBulletNumber);
	}

	int GetRifleReloadBulletNumber (int lv) {
		int startNumber = 5;
		int perNumber = 1;
		int returnNumber = startNumber + perNumber * (lv - 1);
		return returnNumber;
	}

	public bool IsFirable () {
		if (rifleProperties.rifleCurrentReloadBulletNumber > 0 && currentFireTime >= 1) {
			return true;
		} else {
			return false;
		}
	}

	public void SetFireInfo () {
		currentFireTime = 0;
		currentReloadTime = 0;
		audioSource.clip = audioClips [Random.Range (0, audioClips.Length)];
		audioSource.Play ();
		//rifleProperties.rifleCurrentReloadBulletNumber--;
		//uiAmmo.ChangeImgs (rifleProperties.rifleCurrentReloadBulletNumber);
	}
		
	void Update () {
		currentFireTime += Time.deltaTime / rifleProperties.rifleFixedFireDelayTime;
		currentFireTime = Mathf.Clamp01 (currentFireTime);

		if (rifleProperties.rifleCurrentReloadBulletNumber == rifleProperties.rifleMaxReloadBulletNumber) {
			currentReloadTime = 0;
		} else {
			currentReloadTime += Time.deltaTime / rifleProperties.rifleFixedReloadTime;
			currentReloadTime = Mathf.Clamp01 (currentReloadTime);

			if (currentReloadTime >= 1) {
				currentReloadTime = 0;
				rifleProperties.rifleCurrentReloadBulletNumber++;
				//uiAmmo.ChangeImgs (rifleProperties.rifleCurrentReloadBulletNumber);
			}
		}

		Cheat ();
	}

	public RifleProperties GetRifleProperties () {
		return rifleProperties;
	}

	void Cheat () {
		// test code
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (Input.GetKeyDown (KeyCode.Q)) {
				AddRifleBulletRangeLv ();
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				AddRifleBulletDamageLv ();
			}

			if (Input.GetKeyDown (KeyCode.E)) {
				AddRifleBundleBulletNumberLv ();
			}

			if (Input.GetKeyDown (KeyCode.P)) {
				//AddRifleReloadBulletNumberLv ();
			}
		}
	}
}
