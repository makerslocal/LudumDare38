using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static int STARTING_BEES = 3;
	public static int STARTING_POLLEN = 10;

	public static int TURN_COUNT = 10;
	private int turn = 1;

	private int bees;
	private int usableBees;
	private int pollen;

	private float bPressTimeStamp;

	public GameObject cursorObject;
	private Cursor cursor;
	private BoardGeneration b;

	public Text turnText;
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
		turnText.text = "Turn " + turn + " / " + TURN_COUNT;

		bPressTimeStamp = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("b")) {
			if (Time.time - bPressTimeStamp > 0.5f) {
				EndTurn ();
				bPressTimeStamp = 0f;
			} else {
				Buy ();
				bPressTimeStamp = 0f;
			}
		}
		if (Input.GetKeyDown ("b")) {
			bPressTimeStamp = Time.time;
		}
	}

	void Buy () {
		Hex h = cursor.GetSelectedHex ();

		if (usableBees >= h.beeCost && pollen >= h.pollenCost && !h.isActive && b.isHexPurchasable(h)) {
			h.ActivateHex ();
			h.PurchaseHex ();
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

	void EndTurn () {
		while (usableBees-- > 0) {
			int r = Random.Range (0, 4);
			pollen += r;
		}
		usableBees = bees;
		beeText.text = usableBees + " / " + bees;
		pollenText.text = "" + pollen;
		if (++turn > TURN_COUNT) {
			Application.LoadLevel (0); // end the game
		}
		turnText.text = "Turn " + turn + " / " + TURN_COUNT;
	}
}
