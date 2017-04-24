using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGeneration : MonoBehaviour {

	public GameObject basicHex;
	public GameObject roboHex;
	public GameObject nightHex;
	public GameObject rainbowHex;
	public GameObject cursor;
	public GameObject rocketPrefab;

	public GameObject boardContainer;

	public static int ROW_LENGTH = 11;
	public static int ROW_COUNT= 7;

	private GameObject[] hexes;
	private List<Hex> purchasableHexes;

	private int[] upgradeSpawnPoints;
	private static int UPGRADE_COUNT = 4;

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

			upgradeSpawnPoints [u] = Random.Range (0, hexes.Length);
		}

		for (int i = 0; i < hexes.Length; i++) {
			if (i == startingHexIndex) {
				hex = Instantiate (basicHex);
				hex.GetComponent<Hex> ().ActivateHex ();
				GameObject bee = hex.transform.GetChild (0).gameObject;
				bee.GetComponent<SpriteRenderer> ().color = hex.GetComponent<SpriteRenderer> ().color;
				bee.GetComponent<Animator> ().SetBool ("IsFocused", true);
				bee.GetComponent<Animator> ().SetBool ("IsPurchased", true);
			} else if (i == upgradeSpawnPoints [0]) {
				hex = Instantiate (roboHex);
			} else if (i == upgradeSpawnPoints [1]) {
				hex = Instantiate (nightHex);
			} else if (i == upgradeSpawnPoints [2]) {
				hex = Instantiate (rainbowHex);
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
		Vector3 rocketHexPosition = hexes [rocketHexIndex].transform.GetChild (0).gameObject.transform.position;
		rocket.transform.position = new Vector3 (rocketHexPosition.x, rocketHexPosition.y + 1.5f, rocketHexPosition.z);
		rocket.transform.eulerAngles = new Vector3 (0, 0, 180);
		rocket.transform.SetParent (hexes [rocketHexIndex].transform);

		AddPurchasableHexesAdjacentTo (startingHexIndex);

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
