﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeProperties {
	public float angle;		
	public float strength;	
	public float speed;		
	public float duration;	
	[Range(0, 1)]
	public float noisePercent;		
	[Range(0, 1)]
	public float dampingPercent;	
	//[Range(0, 1)]
	//public float rotationPercent;

	public ShakeProperties (float angle, float strength, float speed, float duration, float noisePercent, float dampingPercent, float rotationPercent)
	{
		this.angle = angle;
		this.strength = strength;
		this.speed = speed;
		this.duration = duration;
		this.noisePercent = noisePercent;
		this.dampingPercent = dampingPercent;
		//this.rotationPercent = rotationPercent;
	}
}

public class ShakeObject : MonoBehaviour {

	IEnumerator currentShakeCoroutine;

	public void StartShake (ShakeProperties properties) {
		if (currentShakeCoroutine != null) {	
			StopCoroutine (currentShakeCoroutine);
		}

		currentShakeCoroutine = Shaking (properties);
		StartCoroutine (currentShakeCoroutine);
	}

	const float maxAngle = 10;

	IEnumerator Shaking (ShakeProperties properties) {
		float completionPercent = 0;
		float movePercent = 0;
		float angleRadians = properties.angle * Mathf.Deg2Rad - Mathf.PI;	
		float moveDistance = 0;

		Vector3 previousWaypoint = Vector3.zero;
		Vector3 currentWaypoint = Vector3.zero;

		//Quaternion targetRotation = Quaternion.identity;
		//Quaternion previousRotation = Quaternion.identity;


		do {	
			if (movePercent >= 1 || completionPercent == 0) {
				float dampingFactor = DampingCurve (completionPercent, properties.dampingPercent);	
				float noiseAngle = (Random.value - 0.5f) * Mathf.PI;	

				angleRadians += Mathf.PI + noiseAngle * properties.noisePercent;
				previousWaypoint = transform.localPosition;
				currentWaypoint = new Vector3 (Mathf.Cos (angleRadians), Mathf.Sin (angleRadians)) * properties.strength * dampingFactor;

				moveDistance = Vector3.Distance (previousWaypoint, currentWaypoint);
				movePercent = 0;

				//targetRotation = Quaternion.Euler (new Vector3 (currentWaypoint.x, currentWaypoint.y).normalized * properties.rotationPercent * dampingFactor * maxAngle);
				//previousRotation = transform.localRotation;

			}

			completionPercent += Time.deltaTime / properties.duration;	
			movePercent += Time.deltaTime / moveDistance * properties.speed;	
			transform.localPosition = Vector3.Lerp (previousWaypoint, currentWaypoint, movePercent);	
			//transform.localRotation = Quaternion.Lerp (previousRotation, targetRotation, movePercent);	
			yield return null;

		}  while (moveDistance > 0);	

	}

	float DampingCurve (float x, float dampingPercent) {
		x = Mathf.Clamp01 (x);
		float a = Mathf.Lerp (2, 0.25f, dampingPercent);	
		float b = 1 - Mathf.Pow (x, a);
		return b * b * b;
	}


}