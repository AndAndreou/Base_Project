using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public KeyCode exitApplication;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(exitApplication) ) {
			Application.Quit();
		}

	}
}
