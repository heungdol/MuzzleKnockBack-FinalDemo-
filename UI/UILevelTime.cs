using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelTime: MonoBehaviour {

	public Slider timeLifeSlider;

	public void StartValue (float maxTime) {
		timeLifeSlider.maxValue = maxTime;
		timeLifeSlider.value = maxTime;
	}

	public void SetValue (float currentTime) {
		timeLifeSlider.value = timeLifeSlider.maxValue - currentTime;
	}
}
