using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static int STARTING_BEES = 3;
	public static int STARTING_POLLEN = 10;

	public static float TURNS_VALUE = 200;
	public static float BEES_VALUE = 150;
	public static float POLLEN_VALUE = 75;
	public static float LUCK = 70;

	public static int TURN_COUNT = 12;
	private int turn = 1;

	private int bees;
	private int usableBees;
	private int pollen;

	private float bPressTimeStamp;

	public GameObject cursorObject;
	private Cursor cursor;
	private BoardGeneration b;
	public BuilderLevel level;

	private List<Upgrade> upgrades;

	public Text turnText;
	public Text beeText;
	public Text pollenText;

	// Use this for initialization
	void Start () {
		b = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BoardGeneration> ();
		cursor = cursorObject.GetComponent<Cursor> ();
		upgrades = new List<Upgrade> ();

		bees = usableBees = STARTING_BEES;
		pollen = STARTING_POLLEN;

		beeText.text = usableBees + " / " + bees;
		pollenText.text = "" + pollen;
		turnText.text = "Turn " + turn + " / " + TURN_COUNT;

		bPressTimeStamp = 0f;
	}
	
	// Update is called once per frame
	public void Update () {
		if (bPressTimeStamp > 0f) {
			if (Time.time - bPressTimeStamp > 0.5f) {
				EndTurn ();
				bPressTimeStamp = 0f;
			} else if ( Input.GetKeyUp("b") ) {
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
			if (h.upgrade != default(Upgrade))
				upgrades.Add (h.upgrade);
			b.AddPurchasableHexesAdjacentTo (b.GetIndexOfHex(h));

			GameObject bee = h.transform.GetChild (0).gameObject;
			bee.GetComponent<Animator> ().SetBool ("IsPurchased", true);
			bee.GetComponent<SpriteRenderer> ().color = h.GetComponent<SpriteRenderer> ().color;

			bees += h.beeReward;
			usableBees -= h.beeCost;
			pollen -= h.pollenCost;
			beeText.text = usableBees + " / " + bees;
			pollenText.text = "" + pollen;

			if (h.transform == b.rocketHex.transform) { //HACK HACK HACK
				//The user bought the endgame tile.
				level.startTakeoffCutscene ();
			}
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

	public string GetScoreDescription() {
		double adjBees = bees;
		double adjPollen = pollen;
		double adjUnusedTurns = TURN_COUNT - turn;
		double adjLuck = LUCK;

		//space upgrayedds
		if (upgrades.Contains (Upgrade.BEEARD)) {
			adjUnusedTurns = adjUnusedTurns * 0.8;
			adjPollen = adjPollen * 1.2;
		}
		if (upgrades.Contains (Upgrade.ROBOT)) {
			adjPollen = adjPollen * 0.8;
			adjUnusedTurns = adjUnusedTurns * 1.2;
		}
		if (upgrades.Contains (Upgrade.UNICORN)) {
			adjLuck = adjLuck * 1.3;
		}
		Debug.Log ("adjBees: " + adjBees + ", adjPollen: " + adjPollen + ", adjUnusedTurns: " + adjUnusedTurns + ", adjLuck: " + adjLuck);

		double score = 0;
		bool hitAsteroid = false;
		string result = "Here's how your ship fared:\n";

		//here I will write some bullshit values that have nothing to do with anything because i hate math

		if (bees < 10) {
			result += "Only";
		} else if (bees > 15) {
			result += "A whopping";
		} else {
			result += "A total of";
		}
		result += " " + bees + " bees went to space, ";
		score += adjBees * BEES_VALUE;

		if (pollen < 10) {
			result += "but they had only";
		} else if (pollen > 20) {
			result += "and they had over";
		} else {
			result += "with";
		}
		result += " " + pollen + " pollen to spare. ";
		score += adjPollen * POLLEN_VALUE;

		if (adjUnusedTurns < 2) {
			result += "They barely made it out alive before the planet fell to ruins!";
		} else if (adjUnusedTurns > 5) {
			result += "They left with tons of time to spare!";
		}
		result += "\n";
		score += adjUnusedTurns * TURNS_VALUE;

		//ok so let's determine whether we hit an asteroid or nah
		double roll = Random.Range (0f, 100f);
		Debug.Log ("Rolled a " + roll);
		if (roll > adjLuck) {
			Debug.Log ("'WHACK!' -- asteroid");
			//so if luck is 70, and we roll over a 70, then we get hit by an asteroid. 30% chance of asteroid. u no what im sayin
			hitAsteroid = true;
			result += "Unfortunately, they were unlucky enough to be struck by an asteroid!";
			if (LUCK == adjLuck) {
				//user didn't buy anything affecting luck, let's hint about the unicorn
				result += " If only they could have had some sort of guardian...";
			}
			result += "\n";
			score = score * 0.75; //rekt
		}
		result += "\n";

		result += "Score: " + score;

		return result;

	}
}
