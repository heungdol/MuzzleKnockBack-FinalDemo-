using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UITitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Button button;
	public Text text;
	public int nextScene;

	private string startText;

	void Start () {
		Button b = button.GetComponent<Button> ();
		b.onClick.AddListener (ButtonOnClick);

		startText = text.text;
	}
	
	void ButtonOnClick () {
		SceneManager.LoadScene (nextScene);
	}

	public void OnPointerEnter (PointerEventData ed) {
		text.text = "> " + startText + " <";
	}

	public void OnPointerExit (PointerEventData ed) {
		text.text = startText;
	}

}
