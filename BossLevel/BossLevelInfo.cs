using UnityEngine;

public class BossLevelInfo : MonoBehaviour {
	public GameObject playerStartPivot;
	public GRAVITY bossLevelGravity;
	public float bossLevelDuration = 15;

	private float currentTime;

	private BossLevelManager levelManager;
	private PlayerController playerController;
	private UILevelTime uiLevelTime;

	void Awake () {
		levelManager = FindObjectOfType<BossLevelManager> ();
		uiLevelTime = FindObjectOfType<UILevelTime> ();
		playerController = FindObjectOfType<PlayerController>();
	}

	void OnEnable () {
		if (playerController) {
			playerController.gameObject.transform.position = playerStartPivot.transform.position;
			playerController.SetPlayerGravity (bossLevelGravity);	// need fix here
		}

		uiLevelTime.StartValue (bossLevelDuration);
	}

	void FixedUpdate () {
		currentTime += Time.deltaTime;
		uiLevelTime.SetValue (currentTime);

		if (bossLevelDuration <= currentTime && playerController && levelManager) {
			levelManager.StartChangeLevel ();
			GetComponent<BossLevelInfo> ().enabled = false;
		}
	}

	public Vector3 GetPlayerStartPosition () {
		return playerStartPivot.transform.position;
	}
}
