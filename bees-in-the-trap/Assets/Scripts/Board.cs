using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject activeHex;
	public GameObject inactiveHex;
	public GameObject cursor;

	public GameObject boardContainer;

	private static int BOTTOM_ROW_LENGTH = 9;
	private static int MIDDLE_ROW_LENGTH = 10;
	private static int TOP_ROW_LENGTH = 11;

	private GameObject[] board;

	// Use this for initialization
	void Start () {
		// identify center cell on bottom
		int startingHexIndex = Mathf.CeilToInt(BOTTOM_ROW_LENGTH / 2);
		GameObject hex;


		for (int i = 0; i < BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH + TOP_ROW_LENGTH; i++) {
			if (i == startingHexIndex) {
				hex = Instantiate (activeHex);
			} else {
				hex = Instantiate (inactiveHex);
			}
			hex.transform.position = CalculateHexPosition (i);
			hex.transform.SetParent(boardContainer.transform);

			if (i == startingHexIndex)
				cursor.transform.position = hex.transform.position;
		}

	}

	private Vector3 CalculateHexPosition(int i) {
		return new Vector3 (GetHexHorizontalPosition (i), GetHexRow (i), 0);
	}

	private int GetHexRow (int i) {
		if (i < BOTTOM_ROW_LENGTH) {
			return 0;
		} else if (i < BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH) {
			return 1;
		} else if (i < BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH + TOP_ROW_LENGTH) {
			return 2;
		}
		throw new UnityException ("Index exceeded bounds of boardspace. Largest board index: " 
			+ BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH + TOP_ROW_LENGTH + 
			", given index: "
			+ i);
	}
	private float GetHexHorizontalPosition(int i) {
		int row = GetHexRow (i);
		float shift = row * (-0.5f);
		if (row == 0)
			return i + shift;
		else if (row == 1)
			return i + shift - BOTTOM_ROW_LENGTH;
		else if (row == 2)
			return i + shift - BOTTOM_ROW_LENGTH - MIDDLE_ROW_LENGTH;
		throw new UnityException ("Index exceeded bounds of boardspace. Largest board index: " 
			+ BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH + TOP_ROW_LENGTH + 
			", given index: "
			+ i);
	}

	public bool isLegalMovement (string moves, Cursor.Direction d) {


		// we had BETTER always be able to move back the way we came...
		if (d == Cursor.Direction.BACK)
			return moves.Length > 0;

		int i = Mathf.CeilToInt(BOTTOM_ROW_LENGTH / 2); // this is always the starting position
		int r = 0; // current row

		foreach (char c in moves) {
			if (c == 'v')
				i--;
			if (c == 'g') {
				if (r == 0) i += BOTTOM_ROW_LENGTH;
				if (r == 1) i += MIDDLE_ROW_LENGTH;
				if (r == 2)
					throw new UnityException (
						"The given string of movements was never legal to begin with."
						+ "Movement string: " + moves + ", "
						+ "Erroneous move: " + c + ", "
						+ "Calculated index so far: " + i);
				
				r++;
			}
			if (c == 'h') {
				if (r == 0) i += BOTTOM_ROW_LENGTH + 1;
				if (r == 1) i += MIDDLE_ROW_LENGTH + 1;
				if (r == 2) 
					throw new UnityException (
						"The given string of movements was never legal to begin with."
						+ "Movement string: " + moves + ", "
						+ "Erroneous move: " + c + ", "
						+ "Calculated index so far: " + i);

				r++;
			}
			if (c == 'n')
				i++;
		}


		// if we're at the top of the map, we know we can't go up any more
		if (r == 2 && (d == Cursor.Direction.UPLEFT || d == Cursor.Direction.UPRIGHT))
			return false;

		// if we're in the leftmost hex in a row, we can't move left
		if ((i == 0 || i == BOTTOM_ROW_LENGTH || i == BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH) && d == Cursor.Direction.LEFT)
			return false;

		// if we're in the rightmost hex in a row, we can't move right
		if (
			(
				i == BOTTOM_ROW_LENGTH - 1 
				|| i == BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH - 1 
				|| i == BOTTOM_ROW_LENGTH + MIDDLE_ROW_LENGTH + TOP_ROW_LENGTH - 1
			) 
			&& d == Cursor.Direction.RIGHT)
			return false;

		return true;
	}

	public Vector3 SetCursorPosition (string moves) {

		int i = Mathf.CeilToInt(BOTTOM_ROW_LENGTH / 2); // this is always the starting position
		int r = 0; // current row

		foreach (char c in moves) {
			if (c == 'v')
				i--;
			if (c == 'g') {
				if (r == 0) i += BOTTOM_ROW_LENGTH;
				if (r == 1) i += MIDDLE_ROW_LENGTH;
				if (r == 2)
					throw new UnityException (
						"The given string of movements was never legal to begin with."
						+ "Movement string: " + moves + ", "
						+ "Erroneous move: " + c + ", "
						+ "Calculated index so far: " + i);
				r++;
			}
			if (c == 'h') {
				if (r == 0) i += BOTTOM_ROW_LENGTH + 1;
				if (r == 1) i += MIDDLE_ROW_LENGTH + 1;
				if (r == 2) 
					throw new UnityException (
						"The given string of movements was never legal to begin with."
						+ "Movement string: " + moves + ", "
						+ "Erroneous move: " + c + ", "
						+ "Calculated index so far: " + i);
				r++;
			}
			if (c == 'n')
				i++;
		}

		return CalculateHexPosition(i);
	}
}
