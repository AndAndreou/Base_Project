using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public RenderTexture miniMapTexture;
	public Material miniMapMaterial;

	public Vector2 miniMapOffset;


	public Texture maxMapBackground;
	public RenderTexture maxMapTexture;

	public Vector2 maxMapSize; // % of screen range 0-1
	public Vector2 maxMapOffset;
	public bool maxMapShow;

	// Use this for initialization
	void Start () {

		maxMapShow = false;
	
	}
	
	// Update is called once per frame
	void Update () 
	{


	}

	void OnGUI ()
	{
		if (maxMapShow == true) 
		{
			drawMaxMap ();
		} 
		else 
		{
			drawMinMap ();
		}
	}

	private void drawMinMap()
	{
		Rect miniMapTextureRect = new Rect (Screen.width - miniMapTexture.width - miniMapOffset.x, miniMapOffset.y, miniMapTexture.width, miniMapTexture.height);
		Graphics.DrawTexture (miniMapTextureRect, miniMapTexture, miniMapMaterial);  
	}

	private void drawMaxMap()
	{
		//draw background
		Rect maxMapBackgroundRect = new Rect (0, 0, Screen.width, Screen.height);
		Graphics.DrawTexture (maxMapBackgroundRect, maxMapBackground); 

		//draw map
		Vector2 size;
		size.x = Screen.width * maxMapSize.x ;
		size.y = Screen.height * maxMapSize.y ;  
		Rect maxMapTextureRect = new Rect (maxMapOffset.x, Screen.height - size.y + maxMapOffset.y, size.x, size.y);
		Graphics.DrawTexture (maxMapTextureRect, maxMapTexture);  
	}

	public bool setMaxMapShow()
	{
		maxMapShow = !maxMapShow;
		return maxMapShow; 
	}
}
