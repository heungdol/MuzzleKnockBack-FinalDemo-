using UnityEngine;

public class CameraShake : MonoBehaviour {

	public ShakeProperties fireShake;
	public ShakeProperties hurtShake;
	public ShakeProperties levelChangeShake;
	public ShakeProperties bossExplosionShake;

	private ShakeObject shakeObject;

	void Start () {
		shakeObject = GetComponent<ShakeObject> ();
	}

	public void FireShake () {
		shakeObject.StartShake (fireShake);
	}

	public void HurtShake () {
		shakeObject.StartShake (hurtShake);
	}

	public void LevelChangeShake () {
		shakeObject.StartShake (levelChangeShake);
	}

	public void BossExplosionShake () {
		shakeObject.StartShake (bossExplosionShake);
	}
}
