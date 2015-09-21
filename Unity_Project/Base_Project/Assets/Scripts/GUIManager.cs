using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	private GameObject player;
	private CharacterController characterControllerScript;
	private GameObject[] teleportPoint;
	private GameObject parentTeleportPoints;
	private PauseGUI pauseGUI; 
	private GameManager gameManager;
	private GameObject maxMapCamera;

	public RenderTexture miniMapTexture;
	public Material miniMapMaterial;

	public Vector2 miniMapOffset;


	public Texture backgroundTexture;
	public RenderTexture maxMapTexture;

	public Vector2 maxMapSize; // % of screen range 0-1
	public Vector2 maxMapOffset;
	[HideInInspector]
	public bool maxMapShow;

	public Vector2 scrollViewOffset;

	public Material unSelectMatirial;
	public Material selectMatirial;
	
	public float buttonHeightSizeForMapScrollView = 0.05f;// % of screen range 0-1
	public Vector2 titleSizeForMap; // % of screen range 0-1
	public Vector2 titleForMapOffset;
	public float fontSizeButton = 0.01f; // % of screen range 0-1
	public float titlefontSize = 0.02f; // % of screen range 0-1

	public GUISkin maxMapSkin;
	public GUISkin teleportButtonSkin;

	private Vector2 scrollPosition;

	//hold last button selected
	private int lastButtonSelect ;

	//private bool showPauseMenu;

	// Use this for initialization
	void Start () {

		//showPauseMenu = false;

		teleportPoint = GameObject.FindGameObjectsWithTag (GameRepository.GetTeleportPointTag());
		player = GameObject.FindWithTag (GameRepository.GetPlayerTag());
		characterControllerScript = player.GetComponent<CharacterController> ();
		parentTeleportPoints = GameObject.FindWithTag (GameRepository.GetParentTeleportPointsTag());
		pauseGUI = this.GetComponent<PauseGUI> ();
		gameManager = GameObject.FindWithTag (GameRepository.GetGameManagerTag()).GetComponent<GameManager>();
		maxMapCamera = GameObject.FindWithTag (GameRepository.GetMapCameraTag ());

		SetMaxMapShow (false);
	}

/*---------------------------------------------------------------------------------------------------------------*/	

	// Update is called once per frame
	void Update () 
	{


	}

/*---------------------------------------------------------------------------------------------------------------*/	
	
	void OnGUI ()
	{
		if (maxMapShow == true) 
		{
			DrawMaxMap ();
		} 
		else 
		{
			if (pauseGUI.GetShowPauseMenu() == false)
			{
				DrawMinMap ();
			}
		}
	}

/*---------------------------------------------------------------------------------------------------------------*/	

	private void DrawMinMap()
	{
		Rect miniMapTextureRect = new Rect (Screen.width - miniMapTexture.width - miniMapOffset.x, miniMapOffset.y, miniMapTexture.width, miniMapTexture.height);
		Graphics.DrawTexture (miniMapTextureRect, miniMapTexture, miniMapMaterial);  
	}

/*---------------------------------------------------------------------------------------------------------------*/	

	private void DrawMaxMap()
	{
		//set font size 
		maxMapSkin.button.fontSize = teleportButtonSkin.button.fontSize = Mathf.RoundToInt (Screen.width * fontSizeButton);

		GUI.skin = maxMapSkin;

		//draw background
		DrawBackground ();
	
		//draw title
		maxMapSkin.label.fontSize = Mathf.RoundToInt (Screen.width * titlefontSize);
		Vector2 titleSize = new Vector2 (Screen.width * titleSizeForMap.x, Screen.height * titleSizeForMap.y);
		Vector2 titlePosition = new Vector2 (((Screen.width/2)-(titleSize.x/2)) + titleForMapOffset.x, titleForMapOffset.y);
		Rect titleRect = new Rect(titlePosition,titleSize);
		GUI.Label (titleRect,"Map");

		//draw map
		Vector2 size;
		size.x = Screen.width * maxMapSize.x ;
		size.y = Screen.height * maxMapSize.y ;  
		Rect maxMapTextureRect = new Rect (maxMapOffset.x, Screen.height - size.y + maxMapOffset.y, size.x, size.y);
		Graphics.DrawTexture (maxMapTextureRect, maxMapTexture);  

		//draw buttons
		//scroll bar panel


		Rect positionScrollView = new Rect  (maxMapTextureRect.x + maxMapTextureRect.width + scrollViewOffset.x, maxMapTextureRect.y + scrollViewOffset.y,  Screen.width - maxMapTextureRect.x - maxMapTextureRect.width - scrollViewOffset.x , maxMapTextureRect.height - scrollViewOffset.y);
		maxMapSkin.button.fixedHeight = positionScrollView.height * buttonHeightSizeForMapScrollView;

		Rect viewRectScrollView = new Rect (0, 0, positionScrollView.width -16.0f, (maxMapSkin.button.fixedHeight + maxMapSkin.button.margin.top*2)*teleportPoint.Length);

		maxMapSkin.button.fixedWidth = positionScrollView.width;

		scrollPosition = GUI.BeginScrollView (positionScrollView, scrollPosition, viewRectScrollView);


		int i; 
		for (i=0; i<teleportPoint.Length; i++) 
		{
			GUI.enabled = true;

			if (lastButtonSelect == i)
			{
				GUI.enabled = false;
				teleportPoint[i].GetComponent<Renderer>().material = selectMatirial;
			}
			else
			{
				teleportPoint[i].GetComponent<Renderer>().material = unSelectMatirial;
			}

			if (GUILayout.Button(teleportPoint[i].name))
			{
				lastButtonSelect = i;
			}

		}

		GUI.EndScrollView ();

		GUI.enabled = true;

		//teleport button
		Vector2 positionTeleportButton;
		Vector2 sizeTeleportButton;

		sizeTeleportButton.x = maxMapSkin.button.fixedWidth / 2;
		sizeTeleportButton.y = maxMapSkin.button.fixedHeight;
		positionTeleportButton.x = Screen.width - sizeTeleportButton.x;
		positionTeleportButton.y = positionScrollView.y - sizeTeleportButton.y - 10.0f;

		GUI.skin = teleportButtonSkin;
		teleportButtonSkin.button.fixedWidth = sizeTeleportButton.x ;
		teleportButtonSkin.button.fixedHeight = sizeTeleportButton.y;

		if (DrawButton (positionTeleportButton,"Teleport",sizeTeleportButton)) 
		{
			characterControllerScript.teleport(teleportPoint[lastButtonSelect].transform.position);
			SetMaxMapShow(false);
			SetMaxMapCameraState(false);
			gameManager.UnPause();
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

	private bool DrawButton (Vector2 position, string name,Vector2 size)
	{
		Rect recForButton = new Rect (position.x,position.y, size.x, size.y);
		if(GUI.Button(recForButton,name))
		{
			return true;
		}
		return false ;
	}

/*---------------------------------------------------------------------------------------------------------------*/	

	public void SetMaxMapShow(bool value)
	{
		if (value == true) 
		{
			SetTeleportPointsState(true);
			SetMaxMapCameraState(true);
		} 
		else 
		{
			SetMaxMapCameraState(false);
			SetTeleportPointsState(false);
			lastButtonSelect = -1;
		}

		maxMapShow = value;
	}

/*---------------------------------------------------------------------------------------------------------------*/	

	public bool GetMaxMapShow()
	{
		return maxMapShow; 
	}

/*---------------------------------------------------------------------------------------------------------------*/	
	/*
	public void SetShowPauseMenu(bool value)
	{
		showPauseMenu = value;
		//if (showPauseMenu == true) 
		//{
			pauseGUI.SetShowPauseMenu (showPauseMenu);
		//} 
		//else
		//{
		//	pauseGUI.SetShowPauseMenu (false);
		//}
	}
	*/
/*---------------------------------------------------------------------------------------------------------------*/	
	/*
	public bool GetShowPauseMenu()
	{
		return showPauseMenu;
	}
	*/
/*---------------------------------------------------------------------------------------------------------------*/	

	public void SetMaxMapCameraState(bool value)
	{
		maxMapCamera.SetActive(value);
		
	}

/*---------------------------------------------------------------------------------------------------------------*/

	public void SetTeleportPointsState(bool value)
	{
		parentTeleportPoints.SetActive (value);
	}
}
