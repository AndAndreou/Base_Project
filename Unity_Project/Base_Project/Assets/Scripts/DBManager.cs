﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;



public class DBManager : MonoBehaviour {

	/*
	 * LoginTable (UserID (int), UserName (char30), UserEmail (char50), Password (char20), Login_Timestamp (timestamp), Counter (int))
	 * 
	 */
	private MySqlConnection con = null;
	private MySqlDataReader reader = null;
	private string server;
	private string database;
	private string uid;
	private string password;

	private string playerUserName;
	private string playerPassword;
	private int playerUserID;

	private AsyncOperation async = null;

	// Use this for initialization
	void Start () {

		/*
		//cs.ucy DB Server (need vpn)
		server = "dbserver.in.cs.ucy.ac.cy";
		database = "basedb";
		uid = "basedb";
		password = "FMZBQbGpus";
		*/

		//Haris DB Server
		server = "192.185.119.216";
		database = "wwwbasep_unitydb";
		uid = "wwwbaseproject";
		password = "baseproject321#";

		string connectionString;
		connectionString = "SERVER=" + server + ";" + 
			"DATABASE=" + database + ";" + 
				"UID=" + uid + ";" + 
				"PASSWORD=" + password + ";";
		
		con = new MySqlConnection(connectionString);
	}

	/*---------------------------------------------------------------------------------------------------------------*/
	 
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(dbConnect)) {
			string str = @"server=dbserver.in.cs.ucy.ac.cy;database=basedb;userid=basedb;password=FMZBQbGpus;";
			MySqlConnection con = null;
			MySqlDataReader reader = null;
			try
			{
				con = new MySqlConnection(str);
				con.Open(); //open the connection
				Debug.Log("Connect in DB");

				string cmdText1 = "INSERT INTO LoginTable (UserName, UserEmail, Password, Counter) VALUES('unityinsert', 'unity3d@test', '1234', 0)";
				MySqlCommand cmd1 = new MySqlCommand(cmdText1,con);
				cmd1.ExecuteNonQuery();

				string cmdText = "SELECT * FROM LoginTable";
				MySqlCommand cmd = new MySqlCommand(cmdText,con);
				reader = cmd.ExecuteReader(); //execure the reader
				//The Read() method points to the next record It return false if there are no more records else returns true.
				while (reader.Read())
				{
					//reader.GetString(0) will get the value of the first column of the table myTable because we selected all columns using SELECT * (all); the first loop of the while loop is the first row; the next loop will be the second row and so on...
					Debug.Log(reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4) + " " +  reader.GetString(5));
				}
			}
			catch (MySqlException err) //We will capture and display any MySql errors that will occur
			{
				Debug.Log("Error: " + err.ToString());
			}
		}*/
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	private bool OpenConnection()
	{
		try
		{
			con.Open();
			return true;
		}
		catch (MySqlException ex)
		{
			//When handling errors, you can your application's response based 
			//on the error number.
			//The two most common error numbers when connecting are as follows:
			//0: Cannot connect to server.
			//1045: Invalid user name and/or password.
			switch (ex.Number)
			{
			case 0:
				Debug.Log("Cannot connect to server.  Contact administrator");
				break;
				
			case 1045:
				Debug.Log("Invalid username/password, please try again");
				break;

			default:
				Debug.Log("Error Number : " + ex.Number);
				break;
			}
			return false;
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	//Close connection
	private bool CloseConnection()
	{
		try
		{
			con.Close();
			return true;
		}
		catch (MySqlException ex)
		{
			Debug.Log(ex.ToString());
			return false;
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	//Insert statement
	public void Insert(string query)
	//public IEnumerator Insert(string query)
	{
		//string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";
		
		//open connection
		if (this.OpenConnection() == true)
		{
			//create command and assign the query and connection from the constructor
			MySqlCommand cmd = new MySqlCommand(query, con);
			
			//Execute command
			cmd.ExecuteNonQuery();

			//close connection
			this.CloseConnection();
			//yield return async;
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	//Update statement
	public void Update(string query)
	{
		//string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";
		
		//Open connection
		if (this.OpenConnection() == true)
		{
			//create mysql command
			MySqlCommand cmd = new MySqlCommand();
			//Assign the query using CommandText
			cmd.CommandText = query;
			//Assign the connection using Connection
			cmd.Connection = con;
			
			//Execute query
			cmd.ExecuteNonQuery();
			
			//close connection
			this.CloseConnection();
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	//Delete statement
	public void Delete(string query)
	{
		//string query = "DELETE FROM tableinfo WHERE name='John Smith'";
		
		if (this.OpenConnection() == true)
		{
			MySqlCommand cmd = new MySqlCommand(query, con);
			cmd.ExecuteNonQuery();
			this.CloseConnection();
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	//Count statement
	public int Count()
	{
		string query = "SELECT Count(*) FROM tableinfo";
		int Count = -1;
		
		//Open Connection
		if (this.OpenConnection() == true)
		{
			//Create Mysql Command
			MySqlCommand cmd = new MySqlCommand(query, con);
			
			//ExecuteScalar will return one value
			Count = int.Parse(cmd.ExecuteScalar()+"");
			
			//close Connection
			this.CloseConnection();
			
			return Count;
		}
		else
		{
			return Count;
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public string CheckLogin(string username, string pass){

		string cmdText = "SELECT * FROM users ur WHERE ur.username = '" + username + "' AND ur.password = '" + pass + "'";

		//Open Connection
		if (this.OpenConnection() == true)
		{
			string returnMsg;
			MySqlDataReader reader = null;


			//Create Mysql Command
			MySqlCommand cmd = new MySqlCommand(cmdText,con);
			
			//execure the reader
			reader = cmd.ExecuteReader(); 


			if (reader.Read())
			{
				playerUserID = int.Parse(reader.GetString(4));
				Debug.Log(playerUserID);
				playerUserName = username;
				playerPassword = pass;
				returnMsg = "OK";

				DBInfo.SetUsername(playerUserName);
				DBInfo.SetPassword(pass);
				DBInfo.SetID(playerUserID);

			}
			else
			{
				returnMsg = "UserName or Password in not correct";
			}

			//close Connection
			this.CloseConnection();

			return returnMsg;
		
		}
		else
		{
			this.CloseConnection();
			return ("DataBase can not connect");
		}
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public string SingUp(string username, string pass){
		string query = "INSERT INTO users (username, password) VALUES('" + username + "', '" + pass + "')";
		try
		{
			Insert (query);
			this.CloseConnection();

			CheckLogin(username,pass);
			return "OK";
		}
		catch (MySqlException ex)
		{
			this.CloseConnection();
			return "UserName or Password not invalid";
		}
		//return "";
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public List<Q_AStruct> GetQandA(int seqno){

		//Dictionary<QStruct,List<AStruct>> QandA = new Dictionary<QStruct,List<AStruct>>();
		List<Q_AStruct> QandA = new List<Q_AStruct> ();
		List<QStruct> qstruct = new List<QStruct> ();


		string cmdText = "SELECT * FROM questions qs WHERE qs.section_section_id = '" + seqno + "'";
		
		//Open Connection
		if (this.OpenConnection () == true) {
			//string returnMsg;
			MySqlDataReader reader = null;
			
			
			//Create Mysql Command
			MySqlCommand cmd = new MySqlCommand (cmdText, con);
			
			//execure the reader
			reader = cmd.ExecuteReader (); 

			while (reader.Read()) {
				//reader.GetString(0) will get the value of the first column of the table myTable because we selected all columns using SELECT * (all); the first loop of the while loop is the first row; the next loop will be the second row and so on...
				//Debug.Log (reader.GetString (0) + " " + reader.GetString (1) + " " + reader.GetString (3)); 

				qstruct.Add(new QStruct (reader.GetString (1),int.Parse(reader.GetString (0))));

			}
		
			this.CloseConnection ();
		}

		//foreach (QStruct q in qstruct) {
		for (int i=0; i< qstruct.Count;i++){
			List<AStruct> astruct = new List<AStruct> ();
			//Open Connection
			if (this.OpenConnection () == true) {
				string cmdText2 = "SELECT * FROM answers aw WHERE aw.questions_question_id = '" + qstruct[i].qno + "'";
				MySqlDataReader reader2 = null;
				MySqlCommand cmd2 = new MySqlCommand (cmdText2, con);
				reader2 = cmd2.ExecuteReader ();
				while (reader2.Read()) {
					//Debug.Log ("-----> " + reader2.GetString (0) + " " + reader2.GetString (1));
					astruct.Add(new AStruct(reader2.GetString (1),int.Parse(reader2.GetString (0))));
				}
				this.CloseConnection ();
			}
			QandA.Add(new Q_AStruct(qstruct[i],astruct));
		}

		return QandA;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

	public void AddUserAnswers(List<TwoInt> userAnswers){
		for (int i=0; i<userAnswers.Count; i++) {
			Debug.Log(DBInfo.GetID());
			string query = "INSERT INTO game_round (users_user_id, questions_question_id, answers_answer_id) VALUES('" + DBInfo.GetID() + "', '" + userAnswers[i].questionNo + "', '" + userAnswers[i].answerNo + "')";
			try
			{
				Debug.Log(":::::::::::::::::::::::::::::::::::::");
				Insert(query);
				//StartCoroutine (Insert(query));
			}
			catch (MySqlException ex)
			{
				Debug.Log("Error Insert");
				this.CloseConnection();
				
			}
		}

	}
	
	/*---------------------------------------------------------------------------------------------------------------*/

}
