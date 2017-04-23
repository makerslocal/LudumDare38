using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderLevel : MonoBehaviour {

	public CameraManager camera;

	void startTakeoffCutscene() {
		Debug.Log ("doing it");
		StartCoroutine (doTakeoffCutscene ());
	}
	IEnumerator doTakeoffCutscene() {
		float time = 1.5f;
		Vector3 pos = camera.transform.position;
		pos.y += -10;
		//camera.scootTo (pos, time);
		camera.rotateTo (new Vector3 (0, 0, 180), time);
		camera.zoomTo (25, time);

		yield return new WaitForSeconds (time);

		yield return new WaitForSeconds (0.25f);

		GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardGeneration>().TakeOff ();
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("return")) {
			startTakeoffCutscene ();
		}
	}
}
