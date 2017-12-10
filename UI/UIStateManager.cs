using UnityEngine;
using UnityEngine.UI;

public class UIStateManager : MonoBehaviour {
	public GameObject UIImgParent;
	public GameObject[] UIImgs;

	public Sprite onImgSprite;
	public Sprite offImgSprite;

	private float fixedImgWidth = 25;
	private float fixedImgHeight = 25;
	private float fixedImgHorizontalGap = 30;
	private float fixedImgVerticalGap = 20;

	private int numberPer = 3;

	public void ChangeImgs (int max, int current, int per) {
		numberPer = per;

		if (UIImgs.Length != max) {
			for (int i = 0; i < UIImgs.Length; i++) {
				Destroy (UIImgs [i].gameObject);
			}

			UIImgs = new GameObject[max];

			for (int i = 0; i < max; i++) {
				GameObject aSprite = new GameObject ();
				aSprite.AddComponent<RectTransform> ();
				aSprite.AddComponent<Image> ();
				aSprite.transform.SetParent (UIImgParent.transform);
				aSprite.transform.localPosition = 
					new Vector3 (((i % numberPer)+0.5f) * fixedImgHorizontalGap - GetComponent<RectTransform>().sizeDelta.x / 2
						, ((i / numberPer)+0.5f) * -fixedImgVerticalGap + GetComponent<RectTransform>().sizeDelta.y / 5, 0);
				aSprite.GetComponent<RectTransform>().sizeDelta = new Vector2 (fixedImgWidth, fixedImgHeight);

				UIImgs [i] = aSprite;
			}
		}

		for (int i = 0; i < UIImgs.Length; i++) {
			if (i < current) {
				UIImgs [i].GetComponent<Image> ().sprite = onImgSprite;
			} else {
				UIImgs [i].GetComponent<Image> ().sprite = offImgSprite;
			}
		}
	}

	public void ChangeImgs (int current) {
		for (int i = 0; i < UIImgs.Length; i++) {
			if (i < current) {
				UIImgs [i].GetComponent<Image> ().sprite = onImgSprite;
			} else {
				UIImgs [i].GetComponent<Image> ().sprite = offImgSprite;
			}
		}
	}
}
