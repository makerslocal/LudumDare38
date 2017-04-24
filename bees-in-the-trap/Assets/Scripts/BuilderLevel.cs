using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderLevel : MonoBehaviour {

	public CameraManager camera;
	public GameObject boardContainer;
	public GameObject cutsceneUi;

	void startTakeoffCutscene() {
		Debug.Log ("doing it");
		StartCoroutine (doTakeoffCutscene ());
	}
	IEnumerator doTakeoffCutscene() {
		GameObject.FindGameObjectWithTag ("Cursor").GetComponent<SpriteRenderer> ().enabled = false;
		foreach (GameObject thing in GameObject.FindGameObjectsWithTag("LevelUI")) {
			thing.SetActive (false);
		}

		float time = 1.5f;
		Vector3 pos = camera.transform.position;
		pos.y += -10;
		//camera.scootTo (pos, time);
		camera.rotateTo (new Vector3 (0, 0, 180), time);
		camera.zoomTo (15, time);

		yield return new WaitForSeconds (time);
		GameObject.FindGameObjectWithTag ("Rocket").GetComponent<Animator> ().SetBool ("BlazeIt", true);
		yield return new WaitForSeconds (0.25f);

		BoardGeneration bg = GameObject.FindGameObjectWithTag ("GameController").GetComponent<BoardGeneration> ();
		bg.TakeOff ();
		yield return new WaitForSeconds (2f);

		camera.zoomTo (20, 0);
		GameObject space = GameObject.FindGameObjectWithTag ("SpaceBG");
		space.transform.position = boardContainer.transform.position;
		space.GetComponent<Scroller> ().StartScrolling ();
		camera.transform.position = new Vector3(boardContainer.transform.position.x + Mathf.FloorToInt(BoardGeneration.ROW_LENGTH/2), boardContainer.transform.position.y - Mathf.FloorToInt(BoardGeneration.ROW_COUNT/2), -10);
		camera.zoomTo (25, 30);
		yield return new WaitForSeconds (1f);

		cutsceneUi.SetActive (true);

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
