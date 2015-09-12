using UnityEngine;
using System.Collections;

public class GameRepository : MonoBehaviour {

	private static readonly GameRepository instance = new GameRepository();
	
	//static names
	private string gameManagerTag = "GameManager";
	private string GUIManagerTag = "GUIManager";
	//private int currentLevel=1;
	
	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static GameRepository() 
	{
		
	}
	
	private GameRepository () { }
	
	public static GameRepository Instance {
		get {
			return instance;
		}
	}
	
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	
	public static string getGameManagerTag() {
		return instance.gameManagerTag;
	}

	public static string getGUIManagerTag() {
		return instance.GUIManagerTag;
	}


}
