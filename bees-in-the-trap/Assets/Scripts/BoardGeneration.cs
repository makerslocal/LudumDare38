using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGeneration : MonoBehaviour {

	public GameObject basicHex;
	public GameObject roboHex;
	public GameObject nightHex;
	public GameObject rainbowHex;
	public GameObject zombeeHex;
	public GameObject beeardHex;
	public GameObject beachHex;
	public GameObject gumHex;
	public GameObject redHatHex;
	public GameObject visorHex;
	public GameObject beeBallHex;
	public GameObject buzzFeedHex;
	public GameObject schoolBuzzHex;
	public GameObject cursor;
	public GameObject rocketPrefab;

	public GameObject boardContainer;

	public static int ROW_LENGTH = 11;
	public static int ROW_COUNT= 7;

	private GameObject[] hexes;
	private List<Hex> purchasableHexes;
	public GameObject rocketHex;

	private int[] upgradeSpawnPoints;
	private static int UPGRADE_COUNT = 9;



	// Use this for initialization
	void Start () {
		// identify center cell on bottom
		int startingHexIndex = Mathf.FloorToInt(ROW_LENGTH / 2);
		GameObject hex;
		hexes = new GameObject[ROW_LENGTH * ROW_COUNT + Mathf.FloorToInt(ROW_COUNT / 2)];
		purchasableHexes = new List<Hex> ();

		upgradeSpawnPoints = new int[UPGRADE_COUNT];

		// how to guarantee spawn points are all unique?
		for (int u = 0; u < UPGRADE_COUNT; u++) {
			int random = Random.Range (0, hexes.Length);

			// horrible horrible efficiency and a chance we 
			// loop infinitely but we'll never have
			// more than like n = 100 so w/e
			while (isRandomInUpgradeSpawnPoints (random)) {
				random = Random.Range (0, hexes.Length);
			}
			upgradeSpawnPoints [u] = random;
		}

		for (int i = 0; i < hexes.Length; i++) {
			if (i == startingHexIndex) {
				hex = Instantiate (basicHex);
				hex.GetComponent<Hex> ().ActivateHex ();
				GameObject bee = hex.transform.GetChild (0).gameObject;
				bee.GetComponent<SpriteRenderer> ().color = hex.GetComponent<SpriteRenderer> ().color;
				bee.GetComponent<Animator> ().SetBool ("IsFocused", true);
				bee.GetComponent<Animator> ().SetBool ("IsPurchased", true);
			} else if (IsAnUpgradeSpawnPoint (i)) {
				hex = SpawnUpgrade (i);
			} else {
				hex = Instantiate (basicHex);
			}
			hex.transform.position = CalculateHexPosition (i);
			hex.transform.SetParent(boardContainer.transform);
			hexes [i] = hex;

			if (i == startingHexIndex)
				cursor.transform.position = hex.transform.position;
		}
		//i is now the last row.

		int rocketHexIndex = Mathf.FloorToInt( hexes.Length - Mathf.FloorToInt(ROW_LENGTH/2) - 1 );
		Debug.Log ("rocket edition: " + rocketHexIndex);
		GameObject rocket = Instantiate (rocketPrefab);
		rocketHex = hexes [rocketHexIndex];
		Vector3 rocketHexPosition = hexes [rocketHexIndex].transform.GetChild (0).gameObject.transform.position;
		rocket.transform.position = new Vector3 (rocketHexPosition.x, rocketHexPosition.y + 1.5f, rocketHexPosition.z);
		rocket.transform.eulerAngles = new Vector3 (0, 0, 180);
		rocket.transform.SetParent (hexes [rocketHexIndex].transform);

		AddPurchasableHexesAdjacentTo (startingHexIndex);

		cursor.GetComponent<Cursor> ().FixCamera (); //forces the camera to snap to the cursor immediately

	}

	public void TakeOff() {
		StartCoroutine (GTFO());
	}

	IEnumerator GTFO(float speed = 1.1f) { 
		float increment = 0.01f;
		while ( increment < 20 ) {
			Vector3 temp = boardContainer.transform.position;
			temp.y -= increment;
			boardContainer.transform.position = temp;

			increment = increment * speed;
			yield return null;
		}
		Debug.Log ("Done GTFOing");
	}

	bool IsAnUpgradeSpawnPoint(int i) {
		foreach (int p in upgradeSpawnPoints)
			if (i == p)
				return true;
		return false;
	}
	GameObject SpawnUpgrade(int i) {
		Upgrade u = (Upgrade)0;
		GameObject hex;

		for (int upgradeIndex = 0; upgradeIndex < upgradeSpawnPoints.Length; upgradeIndex++) {
			if(i == upgradeSpawnPoints[upgradeIndex])
				u = (Upgrade)upgradeIndex;
		}
		switch (u) {
		case Upgrade.BEACH:
			hex = Instantiate (beachHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Beeach Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "Each bee does more for the hive, but time files when you're having fun.\n(Improves bee worth, but diminishes turn worth.)";
			break;
		case Upgrade.BEEARD:
			hex = Instantiate (beeardHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Beearded Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "You got more for your pollen when he was younger. Oh, how the years go by.\n(Improves pollen worth, but diminishes turn worth.)";
			break;
		case Upgrade.BEEBALL:
			hex = Instantiate (beeBallHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Bee Ball";
			hex.GetComponent<Hex> ().upgradeDescription = "More money, more honey. Ballin'!\n(Improves pollen and bee worth.)";
			break;
		case Upgrade.BUZZFEED:
			hex = Instantiate (buzzFeedHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Buzzfeed";
			hex.GetComponent<Hex> ().upgradeDescription = "26 pollen you won't believe! #13 will surprise you.\n(Improves pollen worth.)";
			break;
		case Upgrade.GUM:
			hex = Instantiate (gumHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Gum Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "Fun and flexible!\n(Improves pollen and turn worth.)";
			break;
		case Upgrade.NIGHTVISION:
			hex = Instantiate (nightHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Night Vision Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "I can see for miles and miles!\n(Bees collect more pollen.)";
			break;
		case Upgrade.REDHAT:
			hex = Instantiate (redHatHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Red Hat Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "It may be boring, but it's very consistent.\n(Bees always come back with two pollen each.)";
			break;
		case Upgrade.ROBOT:
			hex = Instantiate (roboHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Robobee";
			hex.GetComponent<Hex> ().upgradeDescription = "BEEp\n(Improves turn worth, but diminishes pollen worth.)";
			break;
		case Upgrade.SCHOOLBUZZ:
			hex = Instantiate (schoolBuzzHex);
			hex.GetComponent<Hex> ().upgradeTitle = "School Buzz";
			hex.GetComponent<Hex> ().upgradeDescription = "There are just so many of them!\n(Bee count is increased.)";
			break;
		case Upgrade.UNICORN:
			hex = Instantiate (rainbowHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Unicorn";
			hex.GetComponent<Hex> ().upgradeDescription = "It is said that unicorns bring good luck.";
			break;
		case Upgrade.VISOR:
			hex = Instantiate (visorHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Visor Bee";
			hex.GetComponent<Hex> ().upgradeDescription = "Up for some beer pong, brah?\n(Bees come back with more pollen.)";
			break;
		case Upgrade.ZOMBIE:
			hex = Instantiate (zombeeHex);
			hex.GetComponent<Hex> ().upgradeTitle = "Zombee";
			hex.GetComponent<Hex> ().upgradeDescription = "Beerraaaaiiinnnssss...\n(Bee count is decreased, but pollen is increased.)";
			break;
		default:
			hex = Instantiate (basicHex);
			break;
		}
		hex.GetComponent<Hex> ().upgrade = u;
		return hex;
	}

	private Vector3 CalculateHexPosition(int i) {
		int row = 0;
		while (i > ROW_LENGTH + ((row % 2 == 0)? -1 : 0)) {
			i -= ROW_LENGTH + ((row % 2 == 0)? 0 : 1);
			row++;
		}
		return new Vector3 ((1.315f * i - (0.5f * ((row % 2 == 0)? 0 : 0.44f)) * 3f), row * 1.15f, 0);
	}

	public bool isLegalMovement (string moves, Cursor.Direction d) {


		// we had BETTER always be able to move back the way we came...
		if (d == Cursor.Direction.BACK)
			return moves.Length > 0;

		int x = Mathf.FloorToInt(ROW_LENGTH / 2); // this is always the starting position
		int r = 0; // current row

		foreach (char c in moves) {
			if (c == 'v')
				x--;
			if (c == 'g') {
				if (r % 2 == 1)
					x--;
				r++;
			}
			if (c == 'h') {
				if (r % 2 == 0)
					x++;
				r++;
			}
			if (c == 'n')
				x++;
		}

		// if we're at the top of the map, we know we can't go up any more
		if (r == ROW_COUNT - 1 && (d == Cursor.Direction.UPLEFT || d == Cursor.Direction.UPRIGHT))
			return false;
		
		// if we're in the leftmost hex in a row, we can't move left
		if (x == 0 && (d == Cursor.Direction.LEFT || (d == Cursor.Direction.UPLEFT && (r % 2 == 1))))
			return false;

		// if we're in the rightmost hex in a row, we can't move right
		if (x == (ROW_LENGTH + ((r % 2 == 0)? 0 : 1)) - 1 && (d == Cursor.Direction.RIGHT || (d == Cursor.Direction.UPRIGHT && (r % 2 == 1))))
			return false;

		return true;
	}

	public bool isHexPurchasable(Hex h) {
		if (purchasableHexes.Contains (h))
			return true;
		return false;
	}

	public Vector3 SetCursorPosition (string moves) {

		int i = Mathf.FloorToInt(ROW_LENGTH / 2); // this is always the starting position
		int r = 0; // current row

		foreach (char c in moves) {
			if (c == 'v')
				i--;
			if (c == 'g') {
				i += ROW_LENGTH;
				r++;
			}
			if (c == 'h') {
				i += ROW_LENGTH + 1;
				r++;
			}
			if (c == 'n')
				i++;
		}

		return CalculateHexPosition(i);
	}
	public Hex GetHexAtCursorPosition (string moves) {
		int i = Mathf.FloorToInt(ROW_LENGTH / 2); // this is always the starting position
		int r = 0; // current row

		foreach (char c in moves) {
			if (c == 'v')
				i--;
			if (c == 'g') {
				i += ROW_LENGTH;
				r++;
			}
			if (c == 'h') {
				i += ROW_LENGTH + 1;
				r++;
			}
			if (c == 'n')
				i++;
		}

		return hexes[i].GetComponent<Hex>();
	}

	// i: board index of the target hex
	public void AddPurchasableHexesAdjacentTo (int i) {
		// check each possible hex -- left, upleft, upright, right, downright, downleft

		// left
		if (GetRowForIndex (i) == GetRowForIndex (i - 1) && i > 0) {
			if (isPurchasableHex(i - 1)) {
				purchasableHexes.Add (hexes [i - 1].GetComponent<Hex>());
				hexes [i - 1].GetComponent<Hex> ().IndicatePurchasableHex ();
			}	
		}
		// upleft
		if (i + ROW_LENGTH < hexes.Length) {
			if (isPurchasableHex(i + ROW_LENGTH)) {
				purchasableHexes.Add (hexes [i + ROW_LENGTH].GetComponent<Hex>());
				hexes [i + ROW_LENGTH].GetComponent<Hex> ().IndicatePurchasableHex ();
			}
		}
		// upright
		if (i + ROW_LENGTH + 1 < hexes.Length) {
			if (isPurchasableHex(i + ROW_LENGTH + 1)) {
				purchasableHexes.Add (hexes [i + ROW_LENGTH + 1].GetComponent<Hex>());
				hexes [i + ROW_LENGTH + 1].GetComponent<Hex> ().IndicatePurchasableHex ();
			}
		}
		// right
		if (GetRowForIndex (i) == GetRowForIndex (i + 1) && i + 1 < hexes.Length) {
			if (isPurchasableHex(i + 1)) {
				purchasableHexes.Add (hexes [i + 1].GetComponent<Hex>());
				hexes [i + 1].GetComponent<Hex> ().IndicatePurchasableHex ();
			}
		}
		// downright
		if (GetRowForIndex (i) > 0 && i - ROW_LENGTH >= 0) {
			if(isPurchasableHex(i - ROW_LENGTH)) {
				purchasableHexes.Add (hexes[i - ROW_LENGTH].GetComponent<Hex>());
				hexes [i - ROW_LENGTH].GetComponent<Hex> ().IndicatePurchasableHex ();
			}
		}
		// downleft
		if (GetRowForIndex (i) > 0 && i - (ROW_LENGTH + 1) >= 0) {
			if(isPurchasableHex(i - (ROW_LENGTH + 1))) {
				purchasableHexes.Add (hexes[i - (ROW_LENGTH + 1)].GetComponent<Hex>());
				hexes [i - (ROW_LENGTH + 1)].GetComponent<Hex> ().IndicatePurchasableHex ();
			}
		}

		// only doesn't contain it on the start of the game
		if(purchasableHexes.Contains(hexes [i].GetComponent<Hex>())) {
			purchasableHexes.Remove (hexes [i].GetComponent<Hex>());
		}
	}
	// i: board index of the target hex
	private bool isPurchasableHex (int i) {
		if (!hexes [i].GetComponent<Hex> ().isActive && !purchasableHexes.Contains (hexes [i].GetComponent<Hex>()))
			return true;
		return false;
	}

	public int GetRowForIndex (int i) {
		int r = 0;
		while(i >= ROW_LENGTH + ((r % 2 == 0)? 0 : 1)) {
			i -= ROW_LENGTH + ((r % 2 == 0)? 0 : 1);
			r++;
		}
		return r;
	}
	public int GetIndexOfHex (Hex h) {
		for (int i = 0; i < hexes.Length; i++) {
			if (hexes [i].GetComponent<Hex>() == h)
				return i;
		}
		return -1;
	}

	private bool isRandomInUpgradeSpawnPoints (int random) {
		foreach (int u in upgradeSpawnPoints) {
			if (random == u || random == Mathf.FloorToInt(ROW_LENGTH / 2))
				return true;
		}
		return false;
	}
}
