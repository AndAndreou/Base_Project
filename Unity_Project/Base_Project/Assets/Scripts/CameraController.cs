﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public Vector3 firstPersonPositionOffset = new Vector3(0.0f, 0.4f,  0.0f);
	public Vector3 firstPersonPivotOffset= new Vector3(0.0f, 0.4f,  0.0f);
	public Vector3 thirdPersonPositionOffset= new Vector3(0.0f, 0.7f, -3.0f);
	public Vector3 thirdPersonPivotOffset= new Vector3(0.0f, 1.0f,  0.0f);

	public float smooth = 10f;
	
	//public float horizontalAimingSpeed = 400f;
	public float verticalAimingSpeed = 400f;
	public float maxVerticalAngle = 30f;
	public float minVerticalAngle = -60f;
	
	public float mouseSensitivity = 0.3f;

	public float maxZoomFOV ; //defaultFOV
	public float minZoomFOV = 15f;
	public float changingZoomValueFOV = 5f;
	public float sprintFOV = 100f;

	//private PlayerControl playerControl;
	private GameManager gameManager;
	private GameObject targetGameObject;
	private CharacterController characterController;
	private Transform targetTransform;
	private float angleH = 0;
	private float angleV = 0;
	private Transform cam;
	
	private Vector3 relCameraPos;
	private float relCameraPosMag;
	
	private Vector3 smoothPivotOffset;
	private Vector3 smoothCamOffset;
	private Vector3 targetPivotOffset;
	private Vector3 targetCamOffset;
	
	private float defaultFOV;
	private float targetFOV;
	private float zoomFOV;
	//--



	private enum CameraState
	{
		FirstPerson,
		ThirdPerson
	}

	private CameraState cameraState;

	// Use this for initialization
	void Awake () {

		gameManager = GameObject.FindWithTag (GameRepository.GetGameManagerTag()).GetComponent<GameManager>();
		targetGameObject = GameObject.FindWithTag (GameRepository.GetPlayerTag ());
		characterController = targetGameObject.GetComponent<CharacterController>();
		targetTransform = targetGameObject.transform;

		cam = transform;

		cameraState = CameraState.ThirdPerson;

		smoothPivotOffset = thirdPersonPivotOffset;
		smoothCamOffset = thirdPersonPositionOffset;
		
		defaultFOV = cam.GetComponent<Camera>().fieldOfView;
		maxZoomFOV = defaultFOV;
		zoomFOV = (maxZoomFOV - minZoomFOV) / 2;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (gameManager.GetIsPause () == false) {
			//angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed * Time.deltaTime;
			angleH += Input.GetAxis ("Mouse X") * 5.0f;
			angleV += Mathf.Clamp (Input.GetAxis ("Mouse Y"), -1, 1) * verticalAimingSpeed * Time.deltaTime;

			angleV = Mathf.Clamp (angleV, minVerticalAngle, maxVerticalAngle);
			
			
			Quaternion aimRotation = Quaternion.Euler (-angleV, angleH, 0);
			Quaternion camYRotation = Quaternion.Euler (0, angleH, 0);
			cam.rotation = aimRotation;

			//Set camera PivotOffset and PositionOffset
			if (cameraState == CameraState.ThirdPerson) {
				targetPivotOffset = thirdPersonPivotOffset;
				targetCamOffset = thirdPersonPositionOffset;
			} else if (cameraState == CameraState.FirstPerson) {
				targetPivotOffset = firstPersonPivotOffset;
				targetCamOffset = firstPersonPositionOffset;
			}

			//set FOV
			if (characterController.IsZooming ()) {
				if (Input.GetAxis ("Mouse ScrollWheel") < 0)
					zoomFOV += changingZoomValueFOV;
				if (Input.GetAxis ("Mouse ScrollWheel") > 0)
					zoomFOV -= changingZoomValueFOV;

				targetFOV = Mathf.Clamp (zoomFOV, minZoomFOV, maxZoomFOV);
			} else if (characterController.IsSprinting ()) {
				targetFOV = sprintFOV;
			} else {
				targetFOV = defaultFOV;
			}

			cam.GetComponent<Camera> ().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera> ().fieldOfView, targetFOV, Time.deltaTime);

			// Test for collision
			if (cameraState == CameraState.ThirdPerson) {
				bool flag = true;
				Vector3 baseTempPosition = targetTransform.position + camYRotation * targetPivotOffset;
				Vector3 tempOffset = targetCamOffset;
				for (float zOffset = targetCamOffset.z; zOffset < 0; zOffset += 0.5f) {
					tempOffset.z = zOffset;
					if (DoubleViewingPosCheck (baseTempPosition + aimRotation * tempOffset)) {
						targetCamOffset.z = tempOffset.z;
						flag = false;
						break;
					}

				}
				//if not find some position set as first person 

				if (flag) {
					targetPivotOffset = firstPersonPivotOffset;
					targetCamOffset = firstPersonPositionOffset;
				}
			}

			//Set smooth  interpolation
			smoothPivotOffset = Vector3.Lerp (smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
			smoothCamOffset = Vector3.Lerp (smoothCamOffset, targetCamOffset, smooth * Time.deltaTime);

			//Set camera position
			cam.position = targetTransform.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
		}

	}

/*---------------------------------------------------------------------------------------------------------------*/

	public void ChangeCameraState()
	{
		if (cameraState == CameraState.FirstPerson) 
		{
			cameraState = CameraState.ThirdPerson;
		}
		else 
		{
			cameraState = CameraState.FirstPerson;
		}
	}

/*---------------------------------------------------------------------------------------------------------------*/

	public bool GetIsFirtsPerson()
	{
		return (cameraState == CameraState.FirstPerson ? true : false);
	}

/*---------------------------------------------------------------------------------------------------------------*/

	// concave objects doesn't detect hit from outside, so cast in both directions
	bool DoubleViewingPosCheck(Vector3 checkPos)
	{
		//Debug.Log(ViewingPosCheck (checkPos) + "  |  " + ReverseViewingPosCheck (checkPos));
		return ViewingPosCheck (checkPos) && ReverseViewingPosCheck (checkPos);
	}

/*---------------------------------------------------------------------------------------------------------------*/

	//checkPos to targetPosition
	bool ViewingPosCheck (Vector3 checkPos)
	{
		bool isRayHit = false;
		RaycastHit RayHit;
		Vector3 startPosRay = checkPos; 
		Vector3 RayDirection = targetTransform.position - checkPos;//new vector 3 exi sxesi me to ipsos tou player
		
		isRayHit = Physics.Raycast (startPosRay, RayDirection, out RayHit, Vector3.Distance(targetTransform.position,checkPos) );

		if (isRayHit) 
		{
			//Debug.DrawLine (startPosRay, RayHit.transform.position , Color.red);
			//Debug.Log (RayHit.transform.tag );
			if (RayHit.transform.tag == GameRepository.GetMainCameraTag ())
			{
				return true;
			}
			if ((RayHit.transform.tag != GameRepository.GetPlayerTag ()) && (RayHit.collider.isTrigger == false) )
			{
				return false;
			}

		}

		return true;

	}

/*---------------------------------------------------------------------------------------------------------------*/

	//targetPosition to checkPos
	bool ReverseViewingPosCheck(Vector3 checkPos)
	{
		bool isRayHit2 = false;
		RaycastHit RayHit2;
		Vector3 startPosRay2 = targetTransform.position ; 
		Vector3 RayDirection2 = checkPos - targetTransform.position ;

		isRayHit2 = Physics.Raycast (startPosRay2, RayDirection2, out RayHit2, Vector3.Distance(checkPos,targetTransform.position));

		if (isRayHit2) 
		{
			Debug.DrawLine (startPosRay2, RayHit2.transform.position , Color.green);
			if ((RayHit2.transform.tag != GameRepository.GetMainCameraTag ()) && (RayHit2.collider.isTrigger == false) )
			{
				//Debug.Log(RayHit2.transform.tag);
				return false;
			}
		}
		return true;

	}
}
