using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GUIManager guiManager;

	//keys
	public KeyCode exitApplication;
	public KeyCode mapKey;
	public KeyCode pauseKey;


	private bool isPause;

	// Use this for initialization
	void Start () {
	
		isPause = false;
		guiManager =  GameObject.FindWithTag (GameRepository.getGUIManagerTag()).GetComponent<GUIManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(exitApplication) ) {
			Application.Quit();
		}

		if (Input.GetKeyDown(mapKey) ) {
			if (guiManager.setMaxMapShow())
			{
				pause ();
			}
			else
			{
				unPause();
			}
		}

		if (Input.GetKeyDown(pauseKey) ) {
			if (!isPause)
			{
				pause();
			}
			else
			{
				unPause();
			}
		}

	}

	public void pause()
	{
			Time.timeScale = 0 ;
			isPause = true ;
		Debug.Log ("pause");
	}

	public void unPause()
	{
		Time.timeScale = 1 ;
		isPause = false ;
		Debug.Log ("unpause");
	}

	public bool getIsPause()
	{
		return isPause;
	}

}
