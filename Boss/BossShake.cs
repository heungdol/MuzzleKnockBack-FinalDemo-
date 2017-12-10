using UnityEngine;

public class BossShake : MonoBehaviour {

	public ShakeProperties bossHurtShake;
	public ShakeProperties bossExplosionShake;

	private ShakeObject shakeObject;

	void Start () {
		shakeObject = GetComponent<ShakeObject> ();
	}

	public void BossHurtShake () {
		shakeObject.StartShake (bossHurtShake);
	}

	public void BossExplosionShake () {
		shakeObject.StartShake (bossExplosionShake);
	}
}
