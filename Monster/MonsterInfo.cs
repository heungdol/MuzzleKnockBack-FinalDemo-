using UnityEngine;
using UnityEngine.UI;

public class MonsterInfo : MonoBehaviour {
	public int maxHp;

	public Slider hpSlider;
	public ShakeProperties shakeProperties;
	public ShakeObject shakeObject;
	public GameObject explosionEffectPrefab;

	public GameObject dropItemPrefab;	//0: exp, 1: gold, 2: life

	private AudioSource audioSource;
	public AudioClip hurtClip;

	private int currentHp;
	private Rigidbody monsterRigidbody;

	void Start () {
		currentHp = maxHp;
		monsterRigidbody = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();

		hpSlider.maxValue = maxHp;
		hpSlider.value = currentHp;
	}

	public void GetHurt (int damage, Vector3 bulletForward) {
		float buffer = 0.25f;
		monsterRigidbody.velocity = bulletForward * damage * buffer;

		currentHp -= damage;
		hpSlider.value = currentHp;
		shakeObject.StartShake (shakeProperties);

		if (currentHp <= 0) {
			Instantiate (dropItemPrefab, transform.position, Quaternion.identity);
			Instantiate (explosionEffectPrefab, transform.position, Quaternion.identity);
			Destroy (gameObject);
		} else {
			audioSource.clip = hurtClip;
			audioSource.Play ();
		}
	}
}
