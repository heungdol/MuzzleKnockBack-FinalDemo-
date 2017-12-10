using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAttack : MonoBehaviour {

	public GameObject bossBulletPrefab;

	public float minFireCoolTime;
	public float maxFireCoolTime;

	public float bossBulletPower;

	private PlayerController playerController;
	private IEnumerator firing;
	private bool isFiring;

	void Start () {
		playerController = FindObjectOfType<PlayerController> ();	
		StartFiring ();
	}

	public void StartFiring () {
		isFiring = true;

		if (firing != null)
			StopCoroutine (firing);
		firing = KeepFiring ();
		StartCoroutine (firing);
	}

	public void StopFiring () {
		isFiring = false;

		StopCoroutine (firing);
	}

	IEnumerator KeepFiring () {
		while (isFiring) {
			float currentCoolTime = Random.Range (minFireCoolTime, maxFireCoolTime);
			yield return new WaitForSeconds (currentCoolTime);

			for (int i = 0; i < 5; i++) {
				Vector3 rotationVector = (playerController.transform.position - transform.position).normalized;

				GameObject bullet = Instantiate (bossBulletPrefab, transform.position, Quaternion.Euler (rotationVector));
				//bullet.GetComponent<Rigidbody> ().velocity = rotationVector * bossBulletPower;
				bullet.GetComponent<BossMissile>().SetInfo(playerController.transform.position, transform.position);

				yield return new WaitForSeconds (0.25f);
			}

		}
	}
}
