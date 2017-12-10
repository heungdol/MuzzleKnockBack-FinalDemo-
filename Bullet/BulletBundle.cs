using UnityEngine;

public class BulletBundle : MonoBehaviour {
	public GameObject bulletPrefab;
	//public GameObject backgroundLightPrefab;

	private int numberOfBullets;
	private float angleGapBetBullets;

	private float maxBulletRange;
//	private float maxBulletTime;
	private int bulletDamage;

	void Start () {
		InstantiateBullets();
		//InstantiateBackgroundLight ();
		Destroy (this.gameObject, 5f);
	}

	public void SetInfo (int bullets, float angleGap, float range, /*float time,*/ int damage) {
		this.numberOfBullets = bullets;
		this.angleGapBetBullets = angleGap;
		this.maxBulletRange = range;
		//this.maxBulletTime = time;
		this.bulletDamage = damage;
	}

	public void InstantiateBullets () {
		switch (numberOfBullets % 2) {
		case 0:	// even
			for (int i = 0; i < (numberOfBullets) / 2; i++) {
				InstantiateABullet (Quaternion.identity * Quaternion.Euler(i * angleGapBetBullets, 0, 0) * Quaternion.Euler(angleGapBetBullets / 2, 0, 0));
			}
			for (int i = 0; i < (numberOfBullets) / 2; i++) {
				InstantiateABullet (Quaternion.identity * Quaternion.Euler(i * angleGapBetBullets * -1, 0, 0) * Quaternion.Euler(angleGapBetBullets / 2 * -1, 0, 0));
			}
			break;
		case 1:	// odd
			InstantiateABullet (Quaternion.identity);
			for (int i = 1; i <= (numberOfBullets - 1) / 2; i++) {
				InstantiateABullet (Quaternion.identity * Quaternion.Euler(i * angleGapBetBullets, 0, 0));
			}
			for (int i = 1; i <= (numberOfBullets - 1) / 2; i++) {
				InstantiateABullet (Quaternion.identity * Quaternion.Euler(i * angleGapBetBullets * -1, 0, 0));
			}
			break;
		}
	}

	public void GetDestroyTime (float t) {
		Destroy (gameObject, t);
	}

	void InstantiateABullet (Quaternion q) {
		GameObject bulletGameObject = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
		bulletGameObject.transform.SetParent (gameObject.transform);
		bulletGameObject.transform.localRotation = q;
		bulletGameObject.GetComponent<BulletController> ().SetInfo (maxBulletRange, /*maxBulletTime,*/ bulletDamage);
	}
}
