using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossExplosion : MonoBehaviour {

	public GameObject explosionParticelPrefab;
	public AudioClip expClip;
//	public UnityEvent afterExplosionEvent;
	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void StartBossExplosion () {
		StartCoroutine (Explosion ());
	}

	IEnumerator Explosion () {
		yield return new WaitForSeconds (3f);

		GameObject particle = Instantiate (explosionParticelPrefab, transform.position, Quaternion.identity) as GameObject;
		Destroy (particle, particle.GetComponent<ParticleSystem> ().main.startLifetime.constantMax);

		audioSource.clip = expClip;
		audioSource.Play ();
		//afterExplosionEvent.Invoke();

		FindObjectOfType<UIScreenText> ().YouWin ();
		FindObjectOfType<CameraShake> ().BossExplosionShake ();

		gameObject.SetActive (false);
	}
}
