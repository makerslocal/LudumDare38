using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class Hex : MonoBehaviour {

	public Sprite roomHex;
	public Color active;
	public Color inactive;

	public static int DEFAULT_POLLEN_COST = 10;
	public static int DEFAULT_BEE_COST = 1;

	public int pollenCost = 10;
	public int beeCost = 1; 

	public int beeReward = 2;

	public bool isActive;

	public void ActivateHex () {
		if (!isActive) {
			GetComponent<SpriteRenderer> ().color = active;
			isActive = true;
		}
	}
}
