using UnityEngine;

public class TrickBlock : MonoBehaviour {

	public bool isFirstBlock;

	public GameObject blockSpriteGameObject;
	private SpriteRenderer spriteRenderer;
	private BoxCollider boxCollider;

	public Sprite block1;	// safe
	public Sprite block2;

	void Start () {
		spriteRenderer = blockSpriteGameObject.GetComponent<SpriteRenderer> ();
		boxCollider = GetComponent<BoxCollider> ();
		if (isFirstBlock) {
			spriteRenderer.sprite = block1;
			boxCollider.enabled = true;
		} else {

			int i = Random.Range (0, 2);

			if (i == 0) {
				spriteRenderer.sprite = block1;
				boxCollider.enabled = true;
			} else {
				spriteRenderer.sprite = block2;
				spriteRenderer.sortingLayerName = "Player";
				spriteRenderer.sortingOrder = -1;
				boxCollider.enabled = false;
			}

		}
	}
}
