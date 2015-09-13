using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {


	public float speedNormal = 1.0f;
	public float speedFast   = 2.0f;
	
	public float mouseSensitivityX = 5.0f;
	public float mouseSensitivityY = 5.0f;
	
	float rotY = 0.0f;


	private Animator animator;
	// Use this for initialization
	void Start () {
	
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		float forward = Input.GetAxis("Vertical");
		float strafe  = Input.GetAxis("Horizontal");

		if (forward == 0.0f) 
		{
			animator.SetBool("Aiming", false);
			animator.SetFloat ("Speed", 0f);
		}

		if(forward!=0.0f)
		{
			animator.SetBool("Aiming", false);
			animator.SetFloat ("Speed", 0.5f);
		}


		if(forward!=0.0f & Input.GetKey(KeyCode.LeftShift))
		{
			animator.SetBool("Aiming", false);
			animator.SetFloat ("Speed", 1f);
		}

		if (Input.GetKey(KeyCode.Space)) 
		{
			//animator.SetBool ("Squat", false);
			//animator.SetFloat ("Speed", 0f);
			//animator.SetBool("Aiming", false);
			animator.SetTrigger ("Jump");
		}

		//camera rotation
		//if (Input.GetMouseButton(1)) 
		//{
			float rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivityX;
			//rotY += Input.GetAxis("Mouse Y") * mouseSensitivityY;
			//rotY = Mathf.Clamp(rotY, -89.5f, 89.5f);
			//transform.localEulerAngles = new Vector3(-rotY, rotX, 0.0f);
			transform.localEulerAngles = new Vector3(0.0f, rotX, 0.0f);
		//}
		

		// move forwards/backwards
		if (forward != 0.0f)  
		{
			float speed = Input.GetKey(KeyCode.LeftShift) ? speedFast : speedNormal;
			Vector3 trans = new Vector3(0.0f, 0.0f, forward * speed * Time.deltaTime);
			gameObject.transform.localPosition += gameObject.transform.localRotation * trans;
		}
		
		// strafe left/right
		if (strafe != 0.0f) 
		{
			float speed = Input.GetKey(KeyCode.LeftShift) ? speedFast : speedNormal;
			Vector3 trans = new Vector3(strafe * speed * Time.deltaTime, 0.0f, 0.0f);
			gameObject.transform.localPosition += gameObject.transform.localRotation * trans;
		}

	}

	public void teleport(Vector3 destination)
	{
		this.transform.position = destination + new Vector3(0,  this.GetComponent<BoxCollider>().size.y/2,0);
	}
}
