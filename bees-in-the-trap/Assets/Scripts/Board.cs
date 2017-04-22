using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public GameObject activeHex;
	public GameObject inactiveHex;
	public GameObject cursor;

	public GameObject boardContainer;

	private static int ROW_COUNT = 3;
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

}
