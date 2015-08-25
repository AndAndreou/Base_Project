using UnityEngine;
using System.Collections;

public class MinimapCameraMovement : MonoBehaviour {

	//target is main camera
	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = new Vector3 (target.position.x, transform.position.y, target.position.z);

	}
}
