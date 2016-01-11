using UnityEngine;
using System.Collections;

public class DBInfo : MonoBehaviour {
	private static readonly DBInfo instance = new DBInfo();
	
	//static names
	private string username = null;
	private string pass = null;
	private int id = -1;
	private int gameRounId ;
	
	//private int currentLevel=1;
	
	// Explicit static constructor to tell C# compiler
	// not to mark type as beforefieldinit
	static DBInfo() 
	{
		
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/	
	
	private DBInfo () { }
	
	public static DBInfo Instance {
		get {
			return instance;
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/	

	public static string GetUsername() {
		return instance.username;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public static string SetUsername(string un) {
		 instance.username = un;
		return "";
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public static string GetPassword() {
		return instance.pass;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/
	
	public static string SetPassword(string ps) {
		instance.pass = ps;
		return "";
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public static int GetID() {
		return instance.id;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/
	
	public static string SetID(int id) {
		instance.id = id;
		return "";
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public static int GetGameRoundId() {
		return instance.gameRounId;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public static string SetGameRoundId(int gameRoundId) {
		instance.gameRounId = gameRoundId;
		return "";
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/
	
}
