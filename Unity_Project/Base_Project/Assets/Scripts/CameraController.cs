using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private GameObject player;
	//private CharacterController characterControllerScript;

	public Vector3 firstPersonPositionOffset;
	public Vector3 firstPersonRotationOffset;
	public Vector3 thirdPersonPositionOffset;
	public Vector3 thirdPersonRotationOffset;

	private Vector3 playerPosition;

	private enum CameraState
	{
		FirstPerson,
		ThirdPerson
	}

	private CameraState cameraState;

	// Use this for initialization
	void Start () {
	
		player = GameObject.FindWithTag (GameRepository.GetPlayerTag());
		//characterControllerScript = player.GetComponent<CharacterController> ();

		cameraState = CameraState.ThirdPerson;

	}
	
	// Update is called once per frame
	void Update () {

		playerPosition = new Vector3 (player.transform.position);

		if (cameraState == CameraState.FirstPerson) 
		{
			SetFirtPersonSettings();
		} 
		else if (cameraState == CameraState.ThirdPerson) 
		{
			SetThirdPersonSettings();
		}
	}

/*---------------------------------------------------------------------------------------------------------------*/

	private void SetFirtPersonSettings()
	{
		transform.position = playerPosition + firstPersonPositionOffset;
		transform.Rotate = playerPosition + firstPersonRotationOffset;
	}

/*---------------------------------------------------------------------------------------------------------------*/
	
	private void SetThirdPersonSettings()
	{
		transform.position = playerPosition + thirdPersonPositionOffset;
		transform.Rotate = playerPosition + thirdPersonRotationOffset;
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
}
