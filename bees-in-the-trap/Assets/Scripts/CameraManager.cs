using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public GameObject cursor;
	private Camera camera;

	private IEnumerator currentMove;
	private IEnumerator currentRotate;
	private IEnumerator currentZoom;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.position.z = -10; //NO
	}

	public void scootTo(Vector3 endpos, double time = 0.6) {
		if (currentMove != null) {
			//if we're moving, fuck that
			StopCoroutine (currentMove);
		}
		Debug.Log ("Scoot TO:"); Debug.Log(endpos);
		currentMove = SmoothMove (this.transform.position, endpos, time);
		StartCoroutine (currentMove);
	}
	public void rotateTo(Vector3 endrot, double time = 2) {
		if (currentRotate != null) {
			StopCoroutine (currentRotate);
		}
		Debug.Log ("Rotate TO:"); Debug.Log (endrot);
		currentRotate = SmoothRotate (this.transform.rotation.eulerAngles, endrot, time);
		StartCoroutine (currentRotate);
	}
	public void zoomTo(float endzoom, double time = 2) {
		if (currentZoom != null) {
			StopCoroutine (currentZoom);
		}
		Debug.Log ("Zoom TO: " + endzoom);
		currentZoom = SmoothZoom (camera.orthographicSize, endzoom, time);
		StartCoroutine (currentZoom);
	}

	IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, double seconds) {
		double t = 0.0;
		
		endpos.z = this.transform.position.z; //NEVER move forward or backward (Hack because we are in 2D)
		while ( t <= 1.0 ) {
			t += Time.deltaTime/seconds;
			transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep((float) 0.0, (float) 1.0, (float) t));
			yield return null; //WHY 
		}
	}
	IEnumerator SmoothRotate(Vector3 startrot, Vector3 endrot, double seconds) {
		double t = 0.0;

		while ( t <= 1.0 ) {
			t += Time.deltaTime/seconds;
			this.transform.eulerAngles = Vector3.Lerp(startrot, endrot, Mathf.SmoothStep(0.0f, 1.0f, (float) t));
			yield return null;
		}
	}
	IEnumerator SmoothZoom(float startzoom, float endzoom, double seconds) {
		double t = 0.0;

		while (t < 1.0) {
			t += Time.deltaTime / seconds;
			camera.orthographicSize = Mathf.Lerp (startzoom, endzoom, Mathf.SmoothStep (0.0f, 1.0f, (float)t));
			yield return null;
		}
	}
}
