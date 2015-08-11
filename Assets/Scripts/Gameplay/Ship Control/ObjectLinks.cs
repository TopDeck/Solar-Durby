/*** WORK IN PROGRESS ***/
// This eventually will replace the fixed joint I am currently using to connect the blocks together

using UnityEngine;
using System.Collections;

public class ObjectLinks : MonoBehaviour {

	public Transform linkTo; // the object this one will link to
	private Vector3 localOffset;

	// Use this for initialization
	void Start () {
		localOffset = transform.position - linkTo.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Sets this objects position and rotation relative to the others without any form of parenting
		if (linkTo != null) {
			Vector3 worldOffset = linkTo.rotation * localOffset;
			Vector3 spawnPosition = linkTo.position + worldOffset;

			transform.position = spawnPosition;
			transform.rotation = linkTo.rotation;
		}
	}
}
