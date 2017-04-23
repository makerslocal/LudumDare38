using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHider : MonoBehaviour {

	public GameObject tutorial;
	private bool isShowing = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp ("t")) {
			isShowing = !isShowing;
			tutorial.SetActive (isShowing);
		}
	}
}
