using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static int STARTING_BEES = 3;
	public static int STARTING_POLLEN = 10;

	private int bees;
	private int usableBees;
	private int pollen;

	public GameObject cursorObject;
	private Cursor cursor;
	private BoardGeneration b;

	public Text beeText;
	public Text pollenText;

	// Use this for initialization
	void Start () {
		b = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BoardGeneration> ();
		cursor = cursorObject.GetComponent<Cursor> ();

		bees = usableBees = STARTING_BEES;
		pollen = STARTING_POLLEN;

		beeText.text = usableBees + " / " + bees;
		pollenText.text = "" + pollen;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("b")) {
			Buy ();
		}
	}

	void Buy () {
		Hex h = cursor.GetSelectedHex ();

		if (usableBees >= h.beeCost && pollen >= h.pollenCost && !h.isActive && b.isHexPurchasable(h)) {
			h.ActivateHex ();
			b.AddPurchasableHexesAdjacentTo (b.GetIndexOfHex(h));

			GameObject bee = h.transform.GetChild (0).gameObject;
			bee.GetComponent<Animator> ().SetBool ("IsPurchased", true);
			bee.GetComponent<SpriteRenderer> ().color = h.GetComponent<SpriteRenderer> ().color;

			bees += h.beeReward;
			usableBees -= h.beeCost;
			pollen -= h.pollenCost;
			beeText.text = usableBees + " / " + bees;
			pollenText.text = "" + pollen;
		}
	}
}
