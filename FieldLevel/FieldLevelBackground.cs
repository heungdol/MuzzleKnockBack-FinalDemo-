using UnityEngine;

public enum LEVELTYPE {
	_11
	, _21
	, _12
	, _22
}


public class FieldLevelBackground : MonoBehaviour {

	public Sprite[] backgroundSprites;
	public Sprite backgroundSideSprite; // bottom
	private LEVELTYPE backgroundType;

	private int lengthOfWidth;
	private int lengthOfHeight;

	private float scaleRatio = 6.25f;

	public void SetInfo (int h, int w, LEVELTYPE t) {
		this.lengthOfHeight = h;
		this.lengthOfWidth = w;
		this.backgroundType = t;

		GenerateBackground ();
	}

	void GenerateBackground () {
		int xRatio = 1;
		int yRatio = 1;

		switch (backgroundType) {
		case LEVELTYPE._11:
			xRatio = 1;
			yRatio = 1;
		break;
		case LEVELTYPE._21 :
			xRatio = 1;
			yRatio = 2;
			break;
		case LEVELTYPE._12 :
			xRatio = 2;
			yRatio = 1;
			break;
		case LEVELTYPE._22 :
			xRatio = 2;
			yRatio = 2;
			break;
		}

		for (int y = 0; y < lengthOfHeight * yRatio; y++) {
			for (int x = 0; x < lengthOfWidth * xRatio; x++) {
				float localPositionX = (lengthOfWidth * xRatio/ -2.0f) + 0.5f + x * 1f;
				float localPositionY = (lengthOfHeight * yRatio/ -2.0f) + 0.5f + y * 1f;

				GameObject tile = new GameObject ();
				tile.transform.SetParent (gameObject.transform);
				tile.transform.localPosition = new Vector3 (localPositionX, localPositionY, 0);
				tile.transform.localScale = Vector3.one * scaleRatio;
				tile.name = "Tile \t\t[" + x + ", " + y + "]";

				SpriteRenderer tileSpriteRenderer = tile.AddComponent<SpriteRenderer> ();
				tileSpriteRenderer.sortingLayerName = "Background";

				int i = Random.Range (0, 20);
				switch (i) {
				case 0:
					tileSpriteRenderer.sprite = backgroundSprites [1];
					break;
				case 1:
					tileSpriteRenderer.sprite = backgroundSprites [2];
					break;
				case 2:
					tileSpriteRenderer.sprite = backgroundSprites [3];
					break;
				default :
					tileSpriteRenderer.sprite = backgroundSprites [0];
					break;
				}


				if (x == 0) {
					GameObject tileSide = new GameObject ();
					tileSide.transform.SetParent (gameObject.transform);
					tileSide.transform.localPosition = new Vector3 (localPositionX, localPositionY, 0);
					tileSide.transform.localScale = Vector3.one * scaleRatio;
					tileSide.name = "TileSide \t[" + x + ", " + y + "]";

					SpriteRenderer tileSideSpriteRenderer = tileSide.AddComponent<SpriteRenderer> ();
					tileSideSpriteRenderer.sortingLayerName = "Background";
					tileSideSpriteRenderer.sortingOrder = 1;

					tileSideSpriteRenderer.sprite = backgroundSideSprite;
					tileSideSpriteRenderer.transform.rotation = Quaternion.Euler (Vector3.forward * -90);

				}

				if (x == lengthOfWidth * xRatio - 1) {
					GameObject tileSide = new GameObject ();
					tileSide.transform.SetParent (gameObject.transform);
					tileSide.transform.localPosition = new Vector3 (localPositionX, localPositionY, 0);
					tileSide.transform.localScale = Vector3.one * scaleRatio;
					tileSide.name = "TileSide \t[" + x + ", " + y + "]";

					SpriteRenderer tileSideSpriteRenderer = tileSide.AddComponent<SpriteRenderer> ();
					tileSideSpriteRenderer.sortingLayerName = "Background";
					tileSideSpriteRenderer.sortingOrder = 1;

					tileSideSpriteRenderer.sprite = backgroundSideSprite;
					tileSideSpriteRenderer.transform.rotation = Quaternion.Euler (Vector3.forward * 90);
				}

				if (y == 0) {
					GameObject tileSide = new GameObject ();
					tileSide.transform.SetParent (gameObject.transform);
					tileSide.transform.localPosition = new Vector3 (localPositionX, localPositionY, 0);
					tileSide.transform.localScale = Vector3.one * scaleRatio;
					tileSide.name = "TileSide \t[" + x + ", " + y + "]";

					SpriteRenderer tileSideSpriteRenderer = tileSide.AddComponent<SpriteRenderer> ();
					tileSideSpriteRenderer.sortingLayerName = "Background";
					tileSideSpriteRenderer.sortingOrder = 1;

					tileSideSpriteRenderer.sprite = backgroundSideSprite;
					tileSideSpriteRenderer.transform.rotation = Quaternion.Euler (Vector3.forward * 0);
				}

				if (y == lengthOfHeight * yRatio - 1) {
					GameObject tileSide = new GameObject ();
					tileSide.transform.SetParent (gameObject.transform);
					tileSide.transform.localPosition = new Vector3 (localPositionX, localPositionY, 0);
					tileSide.transform.localScale = Vector3.one * scaleRatio;
					tileSide.name = "TileSide \t[" + x + ", " + y + "]";

					SpriteRenderer tileSideSpriteRenderer = tileSide.AddComponent<SpriteRenderer> ();
					tileSideSpriteRenderer.sortingLayerName = "Background";
					tileSideSpriteRenderer.sortingOrder = 1;

					tileSideSpriteRenderer.sprite = backgroundSideSprite;
					tileSideSpriteRenderer.transform.rotation = Quaternion.Euler (Vector3.forward * 180);
				}
			}
		}
	}
}
