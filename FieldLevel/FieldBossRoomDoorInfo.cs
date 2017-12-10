using UnityEngine;

public class FieldBossRoomDoorInfo : MonoBehaviour {
	private int numberOfKey = -1;

	private float holeMaxAngle = 45;
	private float holeRadius = 1.5f;

	public GameObject keyHolePrefab;
	private GameObject[] keyHoles;

	void Update () {
		if (numberOfKey >= 0)
			return;

		if (FindObjectOfType<PlayerInfo> ()) {
			SetInfo (FindObjectOfType<PlayerInfo> ().GetNeedKey ());
		}
	}

	public void SetInfo (int k) {
		this.numberOfKey = k;
		GenerateHoles ();
	}

	public int GetKeyInfo () {
		return numberOfKey;
	}

	public void InsertKeys () {
		for (int i = 0; i < numberOfKey; i++) {
			keyHoles [i].GetComponent<KeyHole> ().StartInsert ();
		}
	}

	void GenerateHoles () {
		keyHoles = new GameObject[numberOfKey];
		for (int i = 0; i < numberOfKey; i++) {
			float angle = -holeMaxAngle + (holeMaxAngle * 2 / (numberOfKey - 1) * i);
			Vector3 localPosition = new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad) * holeRadius, Mathf.Cos (angle * Mathf.Deg2Rad) * holeRadius, 0);

			GameObject hole = Instantiate (keyHolePrefab, Vector3.zero, Quaternion.identity);
			hole.transform.localPosition = gameObject.transform.position + localPosition;
			hole.transform.SetParent (gameObject.transform);

			keyHoles [i] = hole;
		}
	}
}
