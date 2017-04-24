using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

	public float home = -200;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartScrolling() {
		StartCoroutine (DoScrolling ());
	}
	IEnumerator DoScrolling() {
		float pos = home;	
		float seconds = 10; //higher == slower
		while (true) {
			float t = 0.0f;
			while (t < 1) {
				t += Time.deltaTime / seconds;
				this.transform.position = new Vector3(this.transform.position.x, Mathf.Lerp (home, home + 64, t), this.transform.position.z);
				yield return null;
			}
		}
	}
		
}
