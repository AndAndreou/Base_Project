using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Do_Question : MonoBehaviour {

	private GameManager gameManager;
	private DBManager dbManager;
	private CameraController cameraController;
	private CharacterController characterController;

	public float distanceFromChar;
	public float distanceForUpdate;

	private GameObject targetGameObject;
	private Transform targetTransform;

	//[HideInInspector]
	private bool qAndaGUIShow;
	private bool lastTimeqAndaGUIShow;

	//private bool speak;

	private bool flagDistance;
	private bool flagPressButton;

	private bool init = false;

	private Animator animator;

	private Rect interactTextRect;
	public string interactText = "Press F To Talk";
	public GUIStyle InteractTextStyle;

	public string extraText="";

	//section no for take questions from db
	public int sectionNo;

	//skins
	public GUISkin qAndaSkin;
	public float answerfontSize = 0.01f;
	public float questionfontSize = 0.015f ;

	public GUISkin qAndaNextButtonSkin;
	public float nextButtonfontSize = 0.01f ;


	private int selGridInt = 0;
	//num of question for wait answer
	private int currentQuestion;

	//list of questions and answer
	private List<Q_AStruct> q_a;
	//list of user answers (save by id answers)
	private int[] userAnswers;

	private float timeExtraMsg = 0 ;
	public float displayTimeExtraMsg = 5.0f;
	private bool showExtraMsg;

	private bool haveChange;
	private int lastUpdateQuestion;

	// Use this for initialization
	void Start () {
	
		//speak = false;
		flagDistance = false;
		flagPressButton = false;
		animator = GetComponent<Animator> ();
		targetGameObject = GameObject.FindWithTag (GameRepository.GetPlayerTag ());
		targetTransform = targetGameObject.transform;
		gameManager = GameObject.FindWithTag (GameRepository.GetGameManagerTag()).GetComponent<GameManager>();
		dbManager = GameObject.FindWithTag (GameRepository.GetDBManagerTag()).GetComponent<DBManager>();
		characterController = targetGameObject.GetComponent<CharacterController>();
		cameraController = GameObject.FindWithTag (GameRepository.GetMainCameraTag()).GetComponent<CameraController>();

		//Init Interact text Rect
		Vector2 textSize = InteractTextStyle.CalcSize(new GUIContent(interactText));
		interactTextRect = new Rect(Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);

		if (sectionNo >= 0) {
			q_a = dbManager.GetQandA (sectionNo);
			userAnswers = new int[q_a.Count];
		}


		lastTimeqAndaGUIShow = false;
		qAndaGUIShow = false;

		showExtraMsg = true;

		haveChange = false;
	
		currentQuestion = 0;
		lastUpdateQuestion = 0;

		init = true;

	}

/*---------------------------------------------------------------------------------------------------------------*/

	// Update is called once per frame
	void Update () {
		if (gameManager.GetIsPause () == false) {
			if (Mathf.Abs (Vector3.Distance (targetTransform.position, this.transform.position)) <= distanceFromChar) {
				flagDistance = true;
				if (Input.GetKeyDown (KeyCode.F)) {
					flagPressButton = true;
					SetQandAGUIShow(true);
					timeExtraMsg = Time.deltaTime;
				}
			} else {
				flagDistance = false;
				SetQandAGUIShow(false);
			}

			animator.SetBool ("Speak", flagPressButton);

			//close Q&A GUI
			if(qAndaGUIShow == true){
				if(Input.GetKeyDown(KeyCode.Escape)){
					flagPressButton = false;
					SetQandAGUIShow(false);
				}
			}

			//run if change show or not the qAnda GUI
			if(lastTimeqAndaGUIShow != qAndaGUIShow){
				lastTimeqAndaGUIShow =qAndaGUIShow;
				if(qAndaGUIShow == true){
					Cursor.visible = true;
					characterController.SetDontRunUptade(true);
					cameraController.SetDontRunUptade(true);
				}
				else{
					Cursor.visible = false; 
					characterController.SetDontRunUptade(false);
					cameraController.SetDontRunUptade(false);
				}
			}

			//set timer
			if ((timeExtraMsg != 0) && (showExtraMsg == true)){
				timeExtraMsg += Time.deltaTime;
			}

			//if display time pass msg disable
			if(showExtraMsg == true){

				if (timeExtraMsg > displayTimeExtraMsg) {
					showExtraMsg = false;
					if(sectionNo < 0 ){
						flagPressButton = false;
						qAndaGUIShow = false;
					}
				}
			}

			//Debug.Log(Mathf.Abs (Vector3.Distance (targetTransform.position, this.transform.position)) >= distanceForUpdate);
			//Debug.Log("havechange : " + haveChange);
			if (sectionNo>=0){
				if (Mathf.Abs (Vector3.Distance (targetTransform.position, this.transform.position)) >= distanceForUpdate) {
					if(haveChange == true){
						Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++");
						List<TwoInt> updateAnswerUser = new List<TwoInt>();
						for(int i=0; i < currentQuestion ; i++){
							updateAnswerUser.Add(new TwoInt(q_a[i].question.qno,userAnswers[i]));
						}
						lastUpdateQuestion = currentQuestion;
						haveChange = false;
						dbManager.AddUserAnswers(updateAnswerUser);
					}
				}
			}
		}
	}

/*---------------------------------------------------------------------------------------------------------------*/


	void OnGUI(){
		if (gameManager.GetIsPause () == false) {

			if (!init || !flagDistance)
				return;

			//show text if player is close
			if (!flagPressButton) {
				//Init Interact text Rect
				Vector2 textSize = InteractTextStyle.CalcSize (new GUIContent (interactText));
				interactTextRect = new Rect (Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
				GUI.Label (interactTextRect, interactText, InteractTextStyle);
			}


			//show question and answer panel
			if (qAndaGUIShow == true) 
			{
				if((currentQuestion == 0) && (extraText!="") && (showExtraMsg == true)){
					DrawExtraText();
				}
				else{
					if (sectionNo >= 0)
						DrawQandAGUI();
				}
			}
		}

	}

	/*---------------------------------------------------------------------------------------------------------------*/
	
	public void SetQandAGUIShow(bool value)
	{
		qAndaGUIShow = value;
	}
	
	/*---------------------------------------------------------------------------------------------------------------*/
	
	public void DrawQandAGUI(){
		GUI.skin = qAndaSkin;
		qAndaSkin.button.fontSize  = Mathf.RoundToInt (Screen.width * answerfontSize);
		qAndaSkin.label.fontSize = Mathf.RoundToInt (Screen.width * questionfontSize);

		string[] selStrings = new string[q_a[currentQuestion].answer.Count];
		for (int i=0; i < q_a[currentQuestion].answer.Count;i++){
			selStrings[i]=q_a[currentQuestion].answer[i].answer;
		}

		string question = q_a [currentQuestion].question.question;
		string buttonText;
		if(currentQuestion == q_a.Count){
			buttonText = "Finish";
		}
		else{
			buttonText = "Next";
		}


		//GUILayout.BeginArea(new Rect(0,0,qAndaSkin.label.fixedWidth,qAndaSkin.label.fixedHeight));
		GUILayout.Label (question);
		GUILayout.BeginVertical("Box");
			selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
		GUILayout.EndVertical();

		GUI.skin = qAndaNextButtonSkin;
		qAndaNextButtonSkin.button.fontSize  = Mathf.RoundToInt (Screen.width * nextButtonfontSize);

		if (GUILayout.Button (buttonText)) {
		//set user answers
			if(q_a[currentQuestion].answer.Count != 0 )
				userAnswers[currentQuestion] = q_a[currentQuestion].answer[selGridInt].ano;
			currentQuestion ++;
			haveChange = true;
			if(currentQuestion > q_a.Count-1){
				currentQuestion = 0;
				flagPressButton = false;
				qAndaGUIShow = false;
				}
				
		}

	}

/*---------------------------------------------------------------------------------------------------------------*/

	public void DrawExtraText(){
		GUI.skin = qAndaNextButtonSkin;
		qAndaNextButtonSkin.label.fontSize = Mathf.RoundToInt (Screen.width * questionfontSize);
		GUILayout.Label (extraText);
	}
}
