using UnityEngine;
using UnityEngine.Events;

public enum LOOKDIRECTION {
	LEFT, RIGHT
}

[RequireComponent (typeof (RifleInfo))]
public class RifleController : MonoBehaviour {
	
	public GameObject bulletBundlePrefab;
	public GameObject bulletShellPrefab;
	public GameObject rifleMuzzleGameObject;

	private GameObject mouseGameObject;

	public UnityEvent fireEvent;
	public UnityEvent fireEvent2;

	private RifleInfo rifleInfo;

	void Start () {
		rifleInfo = GetComponent<RifleInfo> ();
		mouseGameObject = GameObject.FindWithTag ("Mouse");
	}
		
	void FixedUpdate () {
		gameObject.transform.LookAt (mouseGameObject.transform, Vector3.up);

		if (Input.GetMouseButton (0)) {
			if (Input.GetMouseButton (1)) {
				if (rifleInfo.IsFirable ()) {
					fireEvent2.Invoke ();
				}
			} else {
				if (rifleInfo.IsFirable ()) {
					fireEvent.Invoke ();
				}
			}
		}
	}

	public LOOKDIRECTION wherePlayerLookAt () {
		if (transform.position.x < mouseGameObject.transform.position.x) {
			return LOOKDIRECTION.RIGHT;
		} else {
			return LOOKDIRECTION.LEFT;
		}
	}

	public void InstantiateBulletBundle () {
		GameObject bulletBundle = Instantiate (bulletBundlePrefab, rifleMuzzleGameObject.transform.position, rifleMuzzleGameObject.transform.rotation);
		bulletBundle.GetComponent<BulletBundle> ().SetInfo (
			rifleInfo.GetRifleProperties().rifleBundleBulletNumber
			, rifleInfo.GetRifleProperties().rifleFixedAngleBetBullets
			, rifleInfo.GetRifleProperties().rifleBulletRange
			/*, rifleInfo.GetRifleProperties().rifleFixedBulletTime*/
			, rifleInfo.GetRifleProperties().rifleBulletDamage);
	}

	public void InstantiateBulletShell () {
		Instantiate (bulletShellPrefab, transform.position, Quaternion.identity);
	}

	public GameObject GetMuzzleGameObject () {
		return rifleMuzzleGameObject;
	}
}
