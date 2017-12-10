using UnityEngine;

public enum WALLTYPE {
	VERTICAL0
	, VERTICAL1
	, HORIZONTAL0
	, HORIZONTAL1
}

public class FieldLevelFrameWall : MonoBehaviour {

	//public Sprite 
	private int totalWidth;
	private int totalHeight; 

	private int positionX;
	private int positionY;

	private int length;
	private WALLTYPE wallType;

	private int doorLength = 3;
	private float scaleRatio = 6.25f;

	public Sprite frameSprite;		// bottom
	public Sprite frameSideSprite;
	public Sprite frameEdgeSprite;

	public void SetInfo (int w, int h, int x, int y, int l, int d, WALLTYPE wt) {
		this.totalWidth = w;
		this.totalHeight = h;

		this.positionX = x;
		this.positionY = y;

		this.length = l;
		this.doorLength = d;

		this.wallType = wt;
	}

	void Start () {
		Sprite s;
		float r;

		if (doorLength % 2 != length % 2)
			doorLength++;

		switch (wallType) {
		case WALLTYPE.HORIZONTAL0:	// width
			s = frameSprite;
			r = 0;
			if (positionY == 0) {
				s = frameSideSprite;
			} else if (positionY == totalHeight - 1) {
				s = frameSideSprite;
				r = 180;
			}
			for (int i = 0; i < length; i++) {
				GameObject g = new GameObject ();
				g.name = "Wall";
				g.tag = "Wall";

				g.transform.localScale = Vector3.one * scaleRatio;
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector3 ((length/-2f) + 0.5f + i, 0, 0);

				g.AddComponent <SpriteRenderer> ();
				g.GetComponent<SpriteRenderer> ().sprite = s;
				g.GetComponent<SpriteRenderer> ().sortingLayerName = "Level";
				g.GetComponent<SpriteRenderer> ().sortingOrder = 5;

				g.transform.rotation = Quaternion.Euler (Vector3.forward * r);

				g.AddComponent<BoxCollider> ();
				g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
			}
			break;
		case WALLTYPE.VERTICAL0:	// height
			s = frameSprite;
			r = -90;
			if (positionX == 0) {
				s = frameSideSprite;
				r = -90;
			} else if (positionX == totalWidth - 1) {
				s = frameSideSprite;
				r = 90;
			}
			for (int i = 0; i < length; i++) {
				GameObject g = new GameObject ();
				g.name = "Wall";
				g.tag = "Wall";

				g.transform.localScale = Vector3.one * scaleRatio;
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector3 (0, (length/-2f) + 0.5f + i, 0);

				g.AddComponent <SpriteRenderer> ();
				g.GetComponent<SpriteRenderer> ().sprite = s;
				g.GetComponent<SpriteRenderer> ().sortingLayerName = "Level";
				g.GetComponent<SpriteRenderer> ().sortingOrder = 5;

				g.transform.rotation = Quaternion.Euler (Vector3.forward * r);

				g.AddComponent<BoxCollider> ();
				g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
			}
			break;
		case WALLTYPE.HORIZONTAL1:
			r = 0;
			for (int i = 0; i < length; i++) {
				s = frameSprite;

				GameObject g = new GameObject ();
				g.name = "Wall";
				g.tag = "Wall";

				g.transform.localScale = Vector3.one * scaleRatio;
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector3 ((length/-2f) + 0.5f + i, 0, 0);

				int indexStart = 0;
				int indexEnd = 0;

				switch (length % 2) {
				case 1:
					indexStart = (length / 2) - (doorLength / 2);
					indexEnd = (length / 2) + (doorLength / 2);
					break;
				case 0:
					indexStart = (length / 2) - (doorLength / 2);
					indexEnd = (length / 2) + (doorLength / 2) - 1;
					break;
				}

				if (i < indexStart - 1 || i > indexEnd + 1) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * r);
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
				} else if (i == indexStart - 1 ) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * r);
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
					s = frameEdgeSprite;
				} else if (i == indexEnd + 1) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * (r + 180));
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
					s = frameEdgeSprite;
				} else {
					s = null;
				}

				g.AddComponent<SpriteRenderer> ();
				g.GetComponent<SpriteRenderer> ().sprite = s;
				g.GetComponent<SpriteRenderer> ().sortingLayerName = "Level";
				g.GetComponent<SpriteRenderer> ().sortingOrder = 5;
			}
			break;
		case WALLTYPE.VERTICAL1:
			r = -90;
			for (int i = 0; i < length; i++) {
				s = frameSprite;

				GameObject g = new GameObject ();
				g.name = "Wall";
				g.tag = "Wall";

				g.transform.localScale = Vector3.one * scaleRatio;
				g.transform.SetParent (this.transform);
				g.transform.localPosition = new Vector3 (0, (length/-2f) + 0.5f + i, 0);

				int indexStart = 0;
				int indexEnd = 0;

				switch (length % 2) {
				case 1:
					indexStart = (length / 2) - (doorLength / 2);
					indexEnd = (length / 2) + (doorLength / 2);
					break;
				case 0:
					indexStart = (length / 2) - (doorLength / 2);
					indexEnd = (length / 2) + (doorLength / 2) - 1;
					break;
				}

				if (i < indexStart - 1 || i > indexEnd + 1) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * r);
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
				} else if (i == indexStart - 1 ) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * (r + 180));
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
					s = frameEdgeSprite;
				} else if (i == indexEnd + 1) {
					g.transform.rotation = Quaternion.Euler (Vector3.forward * r);
					g.AddComponent<BoxCollider> ();
					g.GetComponent<BoxCollider> ().size = Vector3.one / scaleRatio;
					s = frameEdgeSprite;
				} else {
					s = null;
				}

				g.AddComponent <SpriteRenderer> ();
				g.GetComponent<SpriteRenderer> ().sprite = s;
				g.GetComponent<SpriteRenderer> ().sortingLayerName = "Level";
				g.GetComponent<SpriteRenderer> ().sortingOrder = 5;
			}
			break;
		}
	}
}
