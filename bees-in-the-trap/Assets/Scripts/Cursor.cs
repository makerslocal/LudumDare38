using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

	public enum Direction
	{
		LEFT, UPLEFT, UPRIGHT, RIGHT, BACK
	}

	private Board b;
	public CameraManager camera;

	private string moves;
	private float backPressTimestamp;
	private Hex selectedHex;

	void Start () {
		b = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Board>();

		moves = "";
		backPressTimestamp = 0f;
	}

	void Update () {
		
		// we usually would let people configure the keys,
		// but not for this hex-oriented control scheme.

		// also, right now if you release multiple keys
		// simultaneously (same frame), it'll prioritize 
		// clockwise starting left, and only accept the 
		// first of those keys as valid input.
		if (Input.GetKeyUp ("v"))
			Move (Direction.LEFT);
		else if (Input.GetKeyUp ("g"))
			Move (Direction.UPLEFT);
		else if (Input.GetKeyUp ("h"))
			Move (Direction.UPRIGHT);
		else if (Input.GetKeyUp ("n"))
			Move (Direction.RIGHT);
		else if (Input.GetButtonUp ("Back")) {
			if (Time.time - backPressTimestamp > 1f) {
				backPressTimestamp = 0f;
				moves = "";
				Move (Direction.LEFT); // whatever, direction doesn't matter
				Move (Direction.BACK); // just resets everything
			}
			else
				Move (Direction.BACK);
		}


		if (Input.GetButtonDown ("Back"))
			backPressTimestamp = Time.time;
	}

	void Move (Direction d) {
		if (b.isLegalMovement (moves, d)) {
			if (d == Direction.BACK) {
				moves = moves.Substring (0, moves.Length - 1);
			} else {
				moves += ConvertDirectionToChar (d);
			}
		}
		transform.position = b.SetCursorPosition (moves);
		selectedHex = b.GetHexAtCursorPosition (moves);
		camera.scootTo (this.transform.position);
	}

	char ConvertDirectionToChar (Direction d) {
		if (d == Direction.LEFT)
			return 'v';
		if (d == Direction.UPLEFT)
			return 'g';
		if (d == Direction.UPRIGHT)
			return 'h';
		if (d == Direction.RIGHT)
			return 'n';
		throw new UnityException ("Given direction has no associated character mapping. Direction: " + d);
	}

	public Hex GetSelectedHex() {
		return selectedHex;
	}
}
