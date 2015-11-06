using UnityEngine;
using System.Collections;
using MySql.Data.MySqlClient;


public class DBManager : MonoBehaviour {

	public KeyCode dbConnect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(dbConnect)) {
			string str = @"server=dbserver.in.cs.ucy.ac.cy;database=basedb;userid=basedb;password=FMZBQbGpus;";
			MySqlConnection con = null;
			MySqlDataReader reader = null;
			try
			{
				con = new MySqlConnection(str);
				con.Open(); //open the connection
				Debug.Log("Connect in DB");

				string cmdText1 = "INSERT INTO LoginTable (
				MySqlCommand cmd1 = new MySqlCommand(cmdText1,con);
				cmd1.ExecuteReader();

				string cmdText = "SELECT * FROM LoginTable";
				MySqlCommand cmd = new MySqlCommand(cmdText,con);
				reader = cmd.ExecuteReader(); //execure the reader
				/*The Read() method points to the next record It return false if there are no more records else returns true.*/
				while (reader.Read())
				{
					/*reader.GetString(0) will get the value of the first column of the table myTable because we selected all columns using SELECT * (all); the first loop of the while loop is the first row; the next loop will be the second row and so on...*/
					Debug.Log(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4) + " " +  reader.GetString(5));
				}
			}
			catch (MySqlException err) //We will capture and display any MySql errors that will occur
			{
				Debug.Log("Error: " + err.ToString());
			}
		}
	}
}
