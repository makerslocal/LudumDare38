using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class Hex : MonoBehaviour {

	public Sprite roomHex;
	public GameObject purchasableIndicator;
	public Color active;
	public Color inactive;

	public static int DEFAULT_POLLEN_COST = 10;
	public static int DEFAULT_BEE_COST = 1;

	public int pollenCost = 10;
	public int beeCost = 1; 

	public int beeReward = 1;

	public bool isActive;

	public Upgrade upgrade;
	public string upgradeTitle = "Bee Home";
	public string upgradeDescription = "Two worker bees live here.";

	public void ActivateHex () {
		if (!isActive) {
			GetComponent<SpriteRenderer> ().color = active;
			isActive = true;
		}
	}
	public void IndicatePurchasableHex() {
		GameObject i = Instantiate (purchasableIndicator);
		i.transform.SetParent (transform);
		i.transform.position = i.transform.parent.transform.position;
	}
	public void PurchaseHex () {
		GameObject button = transform.FindChild ("Purchase Button(Clone)").gameObject;
		Destroy (transform.FindChild("Purchase Button(Clone)").gameObject);
	}
}
