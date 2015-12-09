using UnityEngine;
using System.Collections;

public class Speak : MonoBehaviour {

	public float distanceFromChar;

	private GameObject targetGameObject;
	private Transform targetTransform;

	//private bool speak;

	private bool flagDistance;
	private bool flagPressButton;

	private bool init = false;

	private Animator animator;

	private Rect interactTextRect;
	public string interactText = "Press F To Talk";
	public GUIStyle InteractTextStyle;

	// Use this for initialization
	void Start () {
	
		//speak = false;
		flagDistance = false;
		flagPressButton = false;
		animator = GetComponent<Animator> ();
		targetGameObject = GameObject.FindWithTag (GameRepository.GetPlayerTag ());
		targetTransform = targetGameObject.transform;


		//Init Interact text Rect
		Vector2 textSize = InteractTextStyle.CalcSize(new GUIContent(interactText));
		interactTextRect = new Rect(Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
		init = true;

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Mathf.Abs (Vector3.Distance (targetTransform.position, this.transform.position)) <= distanceFromChar) {
			flagDistance = true;
			if (Input.GetKeyDown (KeyCode.F)) {
				flagPressButton = true;
			}
		} 
		else {
			flagDistance = false;
		}

		if (Input.GetKeyDown (KeyCode.C)) {
			flagPressButton = false ;
		}

		animator.SetBool ("Speak", flagPressButton);


	}

	void OnGUI(){
		if(!init || !flagDistance)
			return;

		if (!flagPressButton) {
			//Init Interact text Rect
			Vector2 textSize = InteractTextStyle.CalcSize (new GUIContent (interactText));
			interactTextRect = new Rect (Screen.width / 2 - textSize.x / 2, Screen.height - (textSize.y + 5), textSize.x, textSize.y);
			GUI.Label (interactTextRect, interactText, InteractTextStyle);
		}
	}
}
