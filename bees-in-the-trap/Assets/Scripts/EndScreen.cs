using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {

	private static float inputDelay = 0.1f;
	private float timeSinceStart = 0f;

	// Update is called once per frame
	void Update () {
		timeSinceStart += Time.deltaTime;
		if (timeSinceStart > 0.1f && Input.GetKeyDown ("b"))
			SceneManager.LoadScene (1);
	}

}
