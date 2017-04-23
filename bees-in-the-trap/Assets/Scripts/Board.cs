using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject activeHex;
	public GameObject inactiveHex;
	public GameObject cursor;

	public GameObject boardContainer;

	private static int ROW_LENGTH = 9;
	public int numberOfRows = 5;

	private GameObject[] hexes;

	// Use this for initialization
	void Start () {
		// identify center cell on bottom
		int startingHexIndex = Mathf.FloorToInt(ROW_LENGTH / 2);
		GameObject hex;
		hexes = new GameObject[ROW_LENGTH * numberOfRows + Mathf.FloorToInt(numberOfRows / 2)];

		for (int i = 0; i < hexes.Length; i++) {
			if (i == startingHexIndex) {
				hex = Instantiate (activeHex);
			} else {
				hex = Instantiate (inactiveHex);
			}
			hex.transform.position = CalculateHexPosition (i);
			hex.transform.SetParent(boardContainer.transform);
			hexes [i] = hex;

			if (i == startingHexIndex)
				cursor.transform.position = hex.transform.position;
		}

	}

	public void TakeOff() {
		StartCoroutine (GTFO());
	}

	IEnumerator GTFO(float speed = 1.1f) { 
		float increment = 0.01f;
		while ( true ) {
			Vector3 temp = boardContainer.transform.position;
			temp.y -= increment;
			boardContainer.transform.position = temp;

			increment = increment * speed;
			yield return null;
		}
	}
		

	private Vector3 CalculateHexPosition(int i) {
		int row = 0;
		while (i > ROW_LENGTH + ((row % 2 == 0)? -1 : 0)) {
			i -= ROW_LENGTH + ((row % 2 == 0)? 0 : 1);
			row++;
		}
		return new Vector3 (i - (0.5f * ((row % 2 == 0)? 0 : 1)), row, 0);
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
		if (r == numberOfRows - 1 && (d == Cursor.Direction.UPLEFT || d == Cursor.Direction.UPRIGHT))
			return false;
		
		// if we're in the leftmost hex in a row, we can't move left
		if (x == 0 && (d == Cursor.Direction.LEFT || (d == Cursor.Direction.UPLEFT && (r % 2 == 1))))
			return false;

		// if we're in the rightmost hex in a row, we can't move right
		if (x == (ROW_LENGTH + ((r % 2 == 0)? 0 : 1)) - 1 && (d == Cursor.Direction.RIGHT || (d == Cursor.Direction.UPRIGHT && (r % 2 == 1))))
			return false;

		return true;
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
}
