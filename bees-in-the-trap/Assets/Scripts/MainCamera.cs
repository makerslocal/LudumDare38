using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public GameObject cursor;
	private IEnumerator currentMove;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position.z = -10; //NO
	}

	public void scootTo(Vector3 endpos) {
		double time = .6;
		bool skipEaseIn = false;
		if (currentMove != null) {
			//if we're moving, fuck that
			StopCoroutine (currentMove);
			skipEaseIn = true;
		}
		Debug.Log ("Scoot TO:"); Debug.Log(endpos);
		currentMove = SmoothMove (this.transform.position, endpos, time, skipEaseIn);
		StartCoroutine (currentMove);
	}

	IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, double seconds, bool skipEaseIn = false) {
		double t = 0.0;
		if (skipEaseIn) {
			//DO SOMETHING???
			//t = 0.1;
		}

		endpos.z = this.transform.position.z; //NEVER move forward or backward (Hack because we are in 2D)
		while ( t <= 1.0 ) {
			t += Time.deltaTime/seconds;
			transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep((float) 0.0, (float) 1.0, (float) t));
			yield return null; //WHY 
		}
	}
}
