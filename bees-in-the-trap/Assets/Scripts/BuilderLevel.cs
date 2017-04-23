using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderLevel : MonoBehaviour {

	public CameraManager camera;

	public void doTakeoff() {
		double time = 1.5;
		Vector3 pos = camera.transform.position;
		pos.y += -10;
		//camera.scootTo (pos, time);
		camera.rotateTo (new Vector3 (0, 0, 180), time);
		camera.zoomTo (25, time);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("return")) {
			doTakeoff ();
		}
	}
}
