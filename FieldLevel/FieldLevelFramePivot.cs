using UnityEngine;

public class FieldLevelFramePivot : MonoBehaviour {

	public GameObject spriteGameObject;

	public Sprite sprite0;
	public Sprite sprite00;
	public Sprite sprite01;
	public Sprite sprite1;
	public Sprite sprite11;
	public Sprite sprite0_5;

	private SpriteRenderer spriteRenderer;

	private int intTop;
	private int intRight;
	private int intBottom;
	private int intLeft;

	void Start () {
		spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer> ();

		if (intTop == -1) {
			if (intRight == -1) {
				spriteRenderer.sprite = sprite0_5;
				transform.rotation = Quaternion.Euler (Vector3.forward * 90);
			} else if (intLeft == -1) {
				spriteRenderer.sprite = sprite0_5;
				transform.rotation = Quaternion.Euler (Vector3.forward * 180);
			} else if (intBottom == 0 || intBottom == 1) {
				spriteRenderer.sprite = sprite1;
				transform.rotation = Quaternion.Euler (Vector3.forward * 180);
			} else if (intBottom > 10) {
				spriteRenderer.sprite = sprite0;
				transform.rotation = Quaternion.Euler (Vector3.forward * 180);
			}
		} else if (intBottom == -1) {
			if (intRight == -1) {
				spriteRenderer.sprite = sprite0_5;
				transform.rotation = Quaternion.Euler (Vector3.forward * 0);
			} else if (intLeft == -1) {
				spriteRenderer.sprite = sprite0_5;
				transform.rotation = Quaternion.Euler (Vector3.forward * 270);
			} else if (intTop == 0 || intTop == 1) {
				spriteRenderer.sprite = sprite1;
			} else if (intTop > 10) {
				spriteRenderer.sprite = sprite0;
			}
		} else if (intRight == -1) {
			if (intLeft == 0 || intLeft == 1) {
				spriteRenderer.sprite = sprite1;
				transform.rotation = Quaternion.Euler (Vector3.forward * 90);
			} else if (intLeft > 10) {
				spriteRenderer.sprite = sprite0;
				transform.rotation = Quaternion.Euler (Vector3.forward * 90);
			}
		} else if (intLeft == -1) {
			if (intRight == 0 || intRight == 1) {
				spriteRenderer.sprite = sprite1;
				transform.rotation = Quaternion.Euler (Vector3.forward * -90);
			} else if (intRight > 10) {
				spriteRenderer.sprite = sprite0;
				transform.rotation = Quaternion.Euler (Vector3.forward * -90);
			}
		} else if ((intTop == 0 || intTop == 1) && intRight > 10 && (intBottom == 0 || intBottom == 1) && intLeft > 10) {
			spriteRenderer.sprite = sprite00;
			transform.rotation = Quaternion.Euler (Vector3.forward * -90);
		} else if (intTop > 10 && (intRight == 0 || intRight == 1) && intBottom > 10 && (intLeft == 0 || intLeft == 1)) {
			spriteRenderer.sprite = sprite00;
			transform.rotation = Quaternion.Euler (Vector3.forward * 0);
		} else if ((intTop == 0 || intTop == 1) && (intRight == 0 || intRight == 1) && (intBottom == 0 || intBottom == 1) && (intLeft == 0 || intLeft == 1)) {
			spriteRenderer.sprite = sprite11;
		} else {
			spriteRenderer.sprite = sprite01;
			if (intTop > 10) {
				transform.rotation = Quaternion.Euler (Vector3.forward * 180);
			} else if (intRight > 10) {
				transform.rotation = Quaternion.Euler (Vector3.forward * 90);
			} else if (intBottom > 10) {
				transform.rotation = Quaternion.Euler (Vector3.forward * 0);
			} else if (intLeft > 10) {
				transform.rotation = Quaternion.Euler (Vector3.forward * 270);
			}
		}
	}

	public void SetInfo (int top, int right, int bottom, int left) {
		this.intTop = top;
		this.intRight = right;
		this.intBottom = bottom;
		this.intLeft = left;
	}
}
