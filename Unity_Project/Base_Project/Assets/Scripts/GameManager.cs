using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GUIManager guiManager;
	private GameObject maxMapCamera;

	//keys
	public KeyCode exitApplication;
	public KeyCode mapKey;
	public KeyCode pauseKey;


	private bool isPause;

	// Use this for initialization
	void Start () {
	
		isPause = false;
		guiManager =  GameObject.FindWithTag (GameRepository.GetGUIManagerTag()).GetComponent<GUIManager>();
		maxMapCamera = GameObject.FindWithTag (GameRepository.GetMapCameraTag ());

		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(exitApplication) ) {
			if(guiManager.GetMaxMapShow())
			{
				guiManager.SetMaxMapShow(false);
				maxMapCamera.SetActive(false);
				UnPause();
			}
			else if (isPause)
			{
				guiManager.SetShowPauseMenu (false);
				UnPause();
			}
			else
			{
				Application.Quit();
			}
		}

		if (Input.GetKeyDown(mapKey) ) {
			if ((!guiManager.GetMaxMapShow()) && !isPause)
			{
				guiManager.SetMaxMapShow(true);
				maxMapCamera.SetActive(true);
				Pause ();
			}
			else if (guiManager.GetMaxMapShow())
			{
				guiManager.SetMaxMapShow(false);
				maxMapCamera.SetActive(false);
				UnPause();
			}
		}

		if (Input.GetKeyDown(pauseKey) ) {
			if (guiManager.GetMaxMapShow() == false) {
				if (!isPause)
				{
					Pause();
					guiManager.SetShowPauseMenu (true);
				}
				else
				{
					UnPause();
					guiManager.SetShowPauseMenu (false);
				}
			}
		}

	}

	public void Pause()
	{
			Time.timeScale = 0 ;
			isPause = true ;
			Debug.Log ("pause");
	}

	public void UnPause()
	{
		Time.timeScale = 1 ;
		isPause = false ;
		Debug.Log ("unpause");
	}

	public bool GetIsPause()
	{
		return isPause;
	}

}
