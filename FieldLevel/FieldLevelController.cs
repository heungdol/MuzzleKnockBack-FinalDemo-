using UnityEngine;

public class FieldLevelController : MonoBehaviour {

	private LEVELTYPE levelType;
	private int levelTypeInt;

	private int levelWidthLength;
	private int levelHeightLength;

	private int levelDoorLength;
	private int levelCaveFillPercent;

	private PlayerController playerController;
	private CameraController cameraController;

	private bool playerIsIn;
	private string levelString;

	public GameObject cameraPositionGameObject;
	public GameObject shadowGameObject;
	public GameObject caveGameObject;
	public GameObject[] specialFieldLevels;

	public void SetInfo (LEVELTYPE lt, int y, int x, int door, int percent, int typeInt) {
		this.levelType = lt;
		this.levelTypeInt = typeInt;

		this.levelWidthLength = x;
		this.levelHeightLength = y;

		this.levelDoorLength = door;
		this.levelCaveFillPercent = percent;
	}

	void Start () {
		playerController = FindObjectOfType<PlayerController> ();
		cameraController = FindObjectOfType<CameraController> ();

		levelString = "";

		playerIsIn = false;
		shadowGameObject.SetActive (true);
		shadowGameObject.GetComponent<FieldLevelShadow> ().SetInfo (levelType, levelHeightLength, levelWidthLength);
		caveGameObject.GetComponent<FieldLevelCaveGenerator> ().SetInfo (levelType, levelHeightLength, levelWidthLength, levelCaveFillPercent, levelDoorLength);

		switch (levelType) {	// for shadow
		case LEVELTYPE._11:
			this.levelHeightLength *= 1;
			this.levelWidthLength *= 1;

			cameraPositionGameObject.transform.localPosition = new Vector3 (0, 0.5f, -2);
			break;
		case LEVELTYPE._12:
			this.levelHeightLength *= 1;
			this.levelWidthLength *= 2;

			cameraPositionGameObject.transform.localPosition = new Vector3 (0, 0.5f, -2);
			break;
		case LEVELTYPE._21:
			this.levelHeightLength *= 2;
			this.levelWidthLength *= 1;

			cameraPositionGameObject.transform.localPosition = new Vector3 (0, 1, -2);
			break;
		case LEVELTYPE._22:
			this.levelHeightLength *= 2;
			this.levelWidthLength *= 2;

			cameraPositionGameObject.transform.localPosition = new Vector3 (0, 1, -2);
			break;
		}

		for (int i = 0; i < specialFieldLevels.Length; i++) {
			specialFieldLevels [i].SetActive (false);
		}

		switch (levelTypeInt) {
		case 0:
			if (levelType == LEVELTYPE._11) {
				specialFieldLevels [3].SetActive (true);
			} else if (levelType == LEVELTYPE._21 || levelType == LEVELTYPE._12) {
				GameObject tbPoint = new GameObject ();
				tbPoint.transform.position = transform.position;
				tbPoint.tag = "TBPoint";
				tbPoint.name = "TeasuerBoxPoint";
			}
			break;
		case 1:// start
			specialFieldLevels [0].SetActive (true);
			caveGameObject.SetActive (false);
			levelString = "시작지점";
			break;
		case 2:// boss
			specialFieldLevels [1].SetActive (true);
			caveGameObject.SetActive (false);
			levelString = "보스방 앞";
			break;
		case 3:// vending

			break;
		case 4:// life vending
			specialFieldLevels [4].SetActive (true);
			caveGameObject.SetActive (false);
			levelString = "트리플 하트";
			break;
		case 5:// star vending
			specialFieldLevels [5].SetActive (true);
			caveGameObject.SetActive (false);
			levelString = "트리플 스타";
			break;
		}
	}

	void Update () {
		if (playerController == null)
			return;
		
		if (playerIsIn == false
			&& Mathf.Abs (transform.position.x - playerController.transform.position.x) < levelWidthLength / 2
			&& Mathf.Abs (transform.position.y - playerController.transform.position.y) < levelHeightLength / 2) {

			playerIsIn = true;
			SendCameraInfo ();

			if (levelString.Length > 0) {
				FindObjectOfType<UIInformLevel> ().StartInforming (levelString);
			}

			if (shadowGameObject)
				shadowGameObject.GetComponent<FieldLevelShadow> ().StartUnshadow ();
			
		} else if (playerIsIn == true
			&& (Mathf.Abs (transform.position.x - playerController.transform.position.x) > levelWidthLength / 2
				|| Mathf.Abs (transform.position.y - playerController.transform.position.y) > levelHeightLength / 2)) {

			playerIsIn = false;
		}
	}

	void SendCameraInfo () {
		cameraController.SetInfoAndStartMove (cameraPositionGameObject);
		if (levelType == LEVELTYPE._11 || levelType == LEVELTYPE._12) {
			cameraController.SetCameraSize (0);
		} else {
			cameraController.SetCameraSize (1);
		}
	}

}
