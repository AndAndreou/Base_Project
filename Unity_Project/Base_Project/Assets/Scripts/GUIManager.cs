using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

	public RenderTexture minimapTexture;
	public Material minimapMaterial;

	public Vector2 minimapOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{


	}

	void OnGUI ()
	{
		Rect minimapTextureRect = new Rect (Screen.width - minimapTexture.width - minimapOffset.x, minimapOffset.y, minimapTexture.width, minimapTexture.height);
		Graphics.DrawTexture (minimapTextureRect, minimapTexture, minimapMaterial);  
	}

}
