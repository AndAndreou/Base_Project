using UnityEngine;
using System.Collections;

public class LoginGUI : MonoBehaviour {

	public Vector2 titleSizeForLogin; // % of screen range 0-1
	public Vector2 titleForLoginOffset;

	public Vector2 loginGroupSize;
	public float textboxPercentage; //percentage for groupsize width
	//public Vector2 sizeButtonLogin;
	//public Vector2 sizeTextLogin;
	
	public Vector2 sizeLoadingBar;
	public Vector2 loadingBarOffset;
	
	public float fontSize; //0.02
	public float titlefontSize ; //0.04
	
	//private int numOfButtons = 3;
	private enum LoginState
	{
		Login,
		SingUP,
		LoadScene,
		Exit
	}

	private LoginState loginState;
	private string title;
	
	public GUISkin loginSkin;
	public GUISkin errorSkin;
	
	public Texture backgroundTexture;

	
	public Texture2D emptyProgressBar; // for loading
	public Texture2D fullProgressBar; // for loading
	
	public AudioClip buttonClickAudio;
	
	private AsyncOperation async = null;

	private string userName = "";
	private string password = "";
	private DBManager dbManager;
	
	private bool showErrorMsg = false;
	private string errorMsg = "";
	private bool showWaitMsg = false;
	private string waitmsg="Loading";
	private float timeWaitMsg;
	private float delayWaitMsg = 1.0f;
	private float timeErrorMsg;
	private float delayErrorMsg = 5.0f;
	// Use this for initialization
	void Start () {
	
		loginState = LoginState.Login;
		title = "Log In";

		dbManager = GameObject.FindWithTag (GameRepository.GetDBManagerTag()).GetComponent<DBManager>();

		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {

		//time for change waiting msg
		if (showWaitMsg) {
			if (timeWaitMsg > delayWaitMsg) {
				if (waitmsg == "Loading.....") {
					waitmsg = "Loading";
					timeWaitMsg = 0.0f;
				} else {
					waitmsg = waitmsg + ".";
					timeWaitMsg = 0.0f;
				}
			} else {
				timeWaitMsg += Time.deltaTime;
			}
		}

		//time to show error msg
		if (showErrorMsg) {
			if (timeErrorMsg > delayErrorMsg) {
				timeErrorMsg = 0.0f;
				showErrorMsg = false;
			} else {
				timeErrorMsg += Time.deltaTime;
			}
		}
	}

	void OnGUI () {
		//draw background
		DrawBackground ();
		
		//load-set skin
		GUI.skin = loginSkin;
		loginSkin.button.fontSize = Mathf.RoundToInt (Screen.width * fontSize);
		loginSkin.textArea.fontSize = Mathf.RoundToInt (Screen.width * fontSize);
		loginSkin.box.fontSize = Mathf.RoundToInt (Screen.width * fontSize);
		loginSkin.label.fontSize = Mathf.RoundToInt (Screen.width * titlefontSize);
		//loginSkin.button.fixedWidth = Screen.width * sizeButtonLogin.x;
		//loginSkin.button.fixedHeight = Screen.height * sizeButtonLogin.y;
		
		//draw title
		Vector2 titleSize = new Vector2 (Screen.width * titleSizeForLogin.x, Screen.height * titleSizeForLogin.y);
		Vector2 titlePosition = new Vector2 (((Screen.width / 2) - (titleSize.x / 2)) + titleForLoginOffset.x, titleForLoginOffset.y);
		Rect titleRect = new Rect (titlePosition, titleSize);
		GUI.Label (titleRect, title);
		
		if (loginState == LoginState.Login) {
			title = "Log In";
			int numOfrows = 3;
			Vector2 groupSize = new Vector2 (Screen.width * loginGroupSize.x, Screen.height * loginGroupSize.y);
			Vector2 groupPosition = new Vector2 (((Screen.width / 2) - (groupSize.x / 2)), ((Screen.height / 2) - (groupSize.y / 2)));
			Rect groupRect = new Rect (groupPosition, groupSize);

			GUI.BeginGroup (groupRect);

			Vector2 boxSize;
			boxSize.x = groupSize.x * (1.0f - textboxPercentage);
			boxSize.y = groupSize.y / numOfrows;
			Vector2 textAreaSize;
			textAreaSize.x = groupSize.x * textboxPercentage;
			textAreaSize.y = groupSize.y / numOfrows;
			int numOfButtons = 3;
			Vector2 buttonSize;
			buttonSize.x = groupSize.x / numOfButtons;
			buttonSize.y = groupSize.y / numOfrows;

			if(showWaitMsg) GUI.enabled =false;
			GUI.Box (new Rect (0, 0, boxSize.x, boxSize.y), "UserName");
			userName = GUI.TextArea (new Rect (boxSize.x, 0, textAreaSize.x, textAreaSize.y), userName);

			if(showWaitMsg) GUI.enabled =false;
			GUI.Box (new Rect (0, boxSize.y, boxSize.x, boxSize.y), "Password");
			password = GUI.TextArea (new Rect (boxSize.x, boxSize.y, textAreaSize.x, textAreaSize.y), password);

			if(showWaitMsg) GUI.enabled =false;
			if (GUI.Button (new Rect (0, boxSize.y * 2, buttonSize.x, buttonSize.y), "Sing Up")) {
			}

			if(showWaitMsg) GUI.enabled =false;
			if (GUI.Button (new Rect (buttonSize.x * 2, boxSize.y * 2, buttonSize.x, buttonSize.y), "Log In")) {
				CheckLogin (userName, password);
			}

			GUI.enabled =true;
			GUI.EndGroup ();

			if(showErrorMsg){
				GUI.skin = errorSkin;
				errorSkin.label.fontSize = Mathf.RoundToInt (Screen.width * fontSize);
				GUI.Label (new Rect (groupPosition.x, groupPosition.y + groupSize.y,groupSize.x,groupSize.y/2.0f),errorMsg);
			}

		} 
		else if (loginState == LoginState.SingUP) {
		} 
		else if (loginState == LoginState.LoadScene) {
			title = "Loading...";
			
			Vector2 sizeLoadingTuxture;
			Vector2 positionLoadingTuxture;
			
			sizeLoadingTuxture.x = Screen.width * sizeLoadingBar.x;
			sizeLoadingTuxture.y = Screen.height * sizeLoadingBar.y;
			
			positionLoadingTuxture.x = (Screen.width / 2) - (sizeLoadingTuxture.x / 2);
			positionLoadingTuxture.y = (Screen.height / 2) - (sizeLoadingTuxture.y / 2);
			
			if (async != null) {
				GUI.DrawTexture (new Rect (positionLoadingTuxture.x, positionLoadingTuxture.y, sizeLoadingTuxture.x, sizeLoadingTuxture.y), emptyProgressBar);
				GUI.DrawTexture (new Rect (positionLoadingTuxture.x, positionLoadingTuxture.y, sizeLoadingTuxture.x * async.progress, sizeLoadingTuxture.y), fullProgressBar);
			}
		}

		if (showWaitMsg) {
			loginSkin.box.fontSize = Mathf.RoundToInt (Screen.width * titlefontSize);
			GUI.Box(new Rect (0.0f,0.0f,Screen.width,Screen.height),waitmsg);
			loginSkin.box.fontSize = Mathf.RoundToInt (Screen.width * fontSize);
		}
	}

	/*---------------------------------------------------------------------------------------------------------------*/
	
	private void DrawBackground()
	{
		//draw background
		Rect backgroundTextureRect = new Rect (0, 0, Screen.width, Screen.height);
		Graphics.DrawTexture (backgroundTextureRect, backgroundTexture); 
	}

	/*---------------------------------------------------------------------------------------------------------------*/
	private void CheckLogin(string userName,string password){
		string msg;
		showWaitMsg = true;
		msg = dbManager.CheckLogin (userName, password);
		if (msg == "OK") {
			loginState = LoginState.LoadScene;
			showWaitMsg = false;
			StartCoroutine (LoadScene ("menu(main)_scene"));
		} 
		else {
			errorMsg = msg;
			showErrorMsg = true;
			showWaitMsg = false;
		}
	}

	/*---------------------------------------------------------------------------------------------------------------*/
	//function for load scene
	private IEnumerator LoadScene(string name)
	{
		async = Application.LoadLevelAsync(name);
		yield return async;
		
	}
	
}