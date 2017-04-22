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
		if (currentMove != null) {
			//if we're moving, fuck that
			StopCoroutine (currentMove);
		}
		Debug.Log ("Scoot TO:"); Debug.Log(endpos);
		currentMove = SmoothMove (this.transform.position, endpos, 5.0);
		StartCoroutine (currentMove);
	}

	IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, double seconds) {
		double t = 0.0;
		endpos.z = this.transform.position.z; //NEVER move forward or backward (Hack because we are in 2D)
		while ( t <= 1.0 ) {
			//Debug.Log ("t=" + t);
			t += Time.deltaTime/seconds;
			transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep((float) 0.0, (float) 1.0, (float) t));
			//Debug.Log (transform.position);
			yield return null; //WHY 
		}
	}
}
