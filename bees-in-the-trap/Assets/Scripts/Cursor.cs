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

	void Start () {
		b = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Board>();

		moves = "";
	}

	void Update () {
		
		// we usually would let people configure the keys,
		// but not for this hex-oriented control scheme.

		// space is configurable tho.

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
		else if (Input.GetButtonUp ("Back"))
			Move (Direction.BACK);
	}

	void Move (Direction d) {
		if (b.isLegalMovement (moves, d)) {
			if (d == Direction.BACK) {
				moves = moves.Substring (0, moves.Length - 1);
				transform.position = b.SetCursorPosition (moves);
			} else {
				moves += ConvertDirectionToChar (d);
				transform.position = b.SetCursorPosition (moves);
			}
		}

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
}
