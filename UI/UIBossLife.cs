using UnityEngine;
using UnityEngine.UI;

public class UIBossLife : MonoBehaviour {

	public Slider bossLifeSlider;

	public void StartValue (float maxLife) {
		bossLifeSlider.maxValue = maxLife;
		bossLifeSlider.value = maxLife;
	}

	public void SetValue (float currentLife) {
		bossLifeSlider.value = currentLife;
	}
}
