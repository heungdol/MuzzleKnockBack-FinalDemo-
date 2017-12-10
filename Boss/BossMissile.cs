using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : MonoBehaviour {

	public GameObject missile;
	public GameObject explosionPrefab;

	private Rigidbody missileRigidbody;
	private Vector3 rot;

	public AudioClip startClip;
	public AudioClip fireClip;

	private AudioSource audioSource;

	public void SetInfo (Vector3 aimPosition, Vector3 missilePosition) {
		gameObject.transform.position = aimPosition;
		missile.transform.position = missilePosition;
		rot = gameObject.transform.position - missile.transform.position;
		rot = rot.normalized;
		//float angle = Vector3.Angle (rot, Vector3.right);
		//missile.transform.rotation = Quaternion.Euler (Vector3.right * angle * -1);
		missile.transform.LookAt(gameObject.transform, Vector3.forward);
	}

	void Start () {
		Destroy (gameObject, 10);

		missileRigidbody = missile.GetComponent<Rigidbody> ();
		missileRigidbody.useGravity = false;

		missile.SetActive (false);

		audioSource = GetComponent<AudioSource> ();

		StartCoroutine (Firing ());
	}

	void OnTriggerEnter (Collider other) {
		if (!other.CompareTag("Boss"))
			Explosion ();
	}

	IEnumerator Firing () {
		audioSource.clip = startClip;
		audioSource.Play ();

		yield return new WaitForSeconds(0.5f);

		audioSource.clip = fireClip;
		audioSource.Play ();

		missile.SetActive (true);

		while (Vector3.Distance (transform.position, missile.transform.position) > 0.1f) {
			missileRigidbody.velocity = rot * 10f;
			yield return null;
		}
		Explosion ();
	}

	void Explosion () {
		Instantiate (explosionPrefab, missile.transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
