using UnityEngine;

public class DestroyObjectAfterSec : MonoBehaviour {
	public float sec = 0.5f;

	void Start () {
		Destroy (gameObject, sec);
	}
} 
	