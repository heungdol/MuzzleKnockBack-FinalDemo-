using UnityEngine;
using UnityEngine.Events;

public class BossInfo : MonoBehaviour {

	public UnityEvent bossHurtEvent;
	public UnityEvent bossDieEvent;

	public int maxLife = 5000;
	private int currentLife;

	public float minDeltaAngle = 30;
	public float maxDeltaAngle = 90;

	private BossMovement bossMovement;

	private UIBossLife uiBossLife;

	public AudioClip hurtClip;

	private AudioSource audioSource;

	void Start () {
		currentLife = maxLife;

		bossMovement = GetComponent<BossMovement> ();
		bossMovement.SetDeltaAngle (minDeltaAngle);

		uiBossLife = FindObjectOfType<UIBossLife> ();
		uiBossLife.StartValue (maxLife);

		audioSource = GetComponent<AudioSource> ();
	}

	public void SubHp (int s) {
		currentLife -= s;

		uiBossLife.SetValue (currentLife);

		bossHurtEvent.Invoke ();

		audioSource.clip = hurtClip;
		audioSource.Play ();

		if (currentLife <= 0) {
			bossDieEvent.Invoke ();
		}
	}

	public void GiveCurrentDeltaAngleToInfo () {
		float ratio = (maxLife - currentLife) / maxLife;
		float currentDeltaAngle = minDeltaAngle + (maxDeltaAngle - minDeltaAngle) * ratio;

		bossMovement.SetDeltaAngle (currentDeltaAngle);
	}

	public float GetCurrentLife () {
		return currentLife;
	}

	public float GetMaxLife () {
		return maxLife;
	}
}
