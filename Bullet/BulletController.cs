using UnityEngine;

public class BulletController : MonoBehaviour {
	public GameObject bulletHitPrefab;

	private Vector3 startPosition;

	private float maxRange;
	private float currentRange;

	private float maxTime;
	private float currentTime;

	private int damage;
	private float a;

	private float maxScaleXRatio = 2.5f;
	private float minScaleXRatio = 1;

	private Vector3 startScale;
	private Vector3 currentScale;

	void Start () {
		startPosition = transform.position;
		startScale = transform.localScale;

		a = 12.5f;

		maxTime = maxRange / a;
		maxTime = Mathf.Sqrt (maxTime);

		transform.parent.GetComponent<BulletBundle> ().GetDestroyTime (maxTime);
	}

	public void SetInfo (float range, int damage) {
		this.maxRange = range;
		this.damage = damage;
	}

	void FixedUpdate () {
		currentTime += Time.deltaTime;
		currentRange = a * Mathf.Pow ((maxTime - currentTime), 2) * -1 + maxRange; 
		transform.position = startPosition + transform.forward * currentRange;

		currentScale = startScale;
		currentScale.z *= minScaleXRatio + (maxScaleXRatio - minScaleXRatio) * (maxTime - currentTime) / maxTime;
		transform.localScale = currentScale;

		RaycastHit hit;
		float raycastRange = 0.3f;

		Debug.DrawRay (transform.position, transform.forward, Color.blue);

		//float backwardAngle = Vector3.Angle (transform.forward, Vector3.right) + 180f;
		//Vector3 backward = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * backwardAngle), Mathf.Sin (Mathf.Deg2Rad * backwardAngle), 0);

		if (/*Physics.Raycast (transform.position, backward, out hit, raycastRange )
			||*/ Physics.Raycast (transform.position, transform.forward, out hit, raycastRange)) {
			if (hit.collider.gameObject.CompareTag ("Wall") || hit.collider.gameObject.CompareTag ("MovingPlatform")) {
				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<FieldLevelCavePixel> () != null) {
				hit.transform.GetComponent<FieldLevelCavePixel> ().Destroying (damage);

				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<FieldLevelGoldPiece> () != null) {
				hit.transform.GetComponent<FieldLevelGoldPiece> ().GetGold ();

				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<MonsterInfo> () != null) {
				hit.transform.GetComponent<MonsterInfo> ().GetHurt(damage, transform.forward.normalized);

				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<TreasureBoxController> () != null) {
				hit.transform.GetComponent<TreasureBoxController> ().Open();

				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<VendingMachine> () != null) {
				hit.transform.GetComponent<VendingMachine> ().StartVending();

				InstantiateHitEffect ();
				Destroy (gameObject);
			}

			if (hit.transform.GetComponent<BossInfo> () != null) {
				hit.transform.GetComponent<BossInfo> ().SubHp (damage);

				InstantiateHitEffect ();
				Destroy (gameObject);
			}
		}
	}

	public void InstantiateHitEffect () {
		Instantiate (bulletHitPrefab, transform.position, Quaternion.identity);
	}

	public int GetBulletDamage () {return damage;}

}
