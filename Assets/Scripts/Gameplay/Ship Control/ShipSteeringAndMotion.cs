/*** WORK IN PROGRESS ***/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSteeringAndMotion : MonoBehaviour {

	//float turningSpeed = 2f;
	public Transform marker;
	public Vector3 shipCenterOfMass;

	private List<Transform> rotToLeftThrusters = new List<Transform>();
	private List<Transform> rotToRightThrusters = new List<Transform>();

	private List<Transform> frontThrusters = new List<Transform>();
	private List<Transform> leftThrusters = new List<Transform>();
	private List<Transform> rearThrusters = new List<Transform>();
	private List<Transform> rightThrusters = new List<Transform>();

	// Use this for initialization
	void Start () {
		findThrusterDirections ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hitinfo;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Tracks the mouse position and rotates the ship to point towards it
		if (Physics.Raycast(ray, out hitinfo)) //Dont understand why. But keyword out is required here, when the identical statement in javascript doesn't need it.
		{
			Vector3 targetDirection = hitinfo.point - transform.position;
			float step = 1f * Time.deltaTime;

			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0F);
			transform.rotation = Quaternion.LookRotation(newDirection);
		}

		if (Input.GetKey ("w")) { //loop through and fire the thrusters
			for(int i=0;i<frontThrusters.Count;i++)
			{
				(frontThrusters[i].GetComponent("Thrust") as Thrust).AddAccelForce();
			}
		}
		if (Input.GetKey ("a")) {
			for(int i=0;i<leftThrusters.Count;i++)
			{
				(leftThrusters[i].GetComponent("Thrust") as Thrust).AddAccelForce();
			}
		}
		if (Input.GetKey ("s")) {
			for(int i=0;i<rearThrusters.Count;i++)
			{
				(rearThrusters[i].GetComponent("Thrust") as Thrust).AddAccelForce();
			}
		}
		if (Input.GetKey ("d")) {
			for(int i=0;i<rightThrusters.Count;i++)
			{
				(rightThrusters[i].GetComponent("Thrust") as Thrust).AddAccelForce();
			}
		}
	}

	public void calculateCOM()
	{
		Transform[] allChildren = transform.parent.GetComponentsInChildren<Transform>(); //Collects all the transforms for the ship components and the parent gameobject
		shipCenterOfMass = new Vector3(0,0,0);	//Sets up a new vector ready so store the sum of all the COM vectors
		float totalMass = 0f;	//Sets up a float to store the sum of all object masses

		for(int i=0;i<allChildren.Length;i++)
		{
			if(allChildren[i].tag != "ShipParent") //Ignore parent object
			{
				shipCenterOfMass += allChildren[i].GetComponent<Rigidbody>().worldCenterOfMass*allChildren[i].GetComponent<Rigidbody>().mass;	//COM vector multiplied my its mass
				totalMass += allChildren[i].GetComponent<Rigidbody>().mass;
			}
		}
		shipCenterOfMass /= totalMass; //Calculate final COM by deviding by the total mass of all parts

		marker.position = shipCenterOfMass; //Visual ship COM marker for testing
	}

	//Sort thrusters based on groups
	void findThrusterDirections()
	{
		Transform[] allChildren = transform.GetComponentsInChildren<Transform>(); //Collects all the transforms for the ship components and the parent gameobject

		for (int i=0; i<allChildren.Length; i++) {
			if (allChildren [i].tag == "Thruster") //Select only thrusters
			{
				if(Mathf.Round(allChildren [i].eulerAngles.y) == 0)
				{
					frontThrusters.Add(allChildren [i]);
				}
				if(Mathf.Round(allChildren [i].eulerAngles.y) == 270)
				{
					leftThrusters.Add(allChildren [i]);
				}
				if(Mathf.Round(allChildren [i].eulerAngles.y) == 180)
				{
					rearThrusters.Add(allChildren [i]);
				}
				if(Mathf.Round(allChildren [i].eulerAngles.y) == 90)
				{
					rightThrusters.Add(allChildren [i]);
				}
			}
		}
	}

	void dampeningThrust()
	{

	}

	//~~~~~ Broken Methods ~~~~~//
	//~ Might get fixed later ~//

	/*public void rotationalThrusterGroups()
	{
		Transform[] allChildren = transform.parent.GetComponentsInChildren<Transform>(); //Collects all the transforms for the ship components and the parent gameobject

		for (int i=0; i<allChildren.Length; i++)
		{
			if (allChildren [i].tag == "Thruster") //Select only thrusters
			{
				//Debug.Log("Entered");
				// front left quad
				if(allChildren [i].localPosition.x < shipCenterOfMass.x);// && allChildren [i].localPosition.z > shipCenterOfMass.z)
				{
					//Debug.Log("Entered");
					Debug.Log(allChildren [i].name);
					Debug.Log(allChildren [i].localEulerAngles.y);
					if(allChildren [i].localEulerAngles.y == 0f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 90f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 180f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 270f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);Debug.Log("Entered");
					}

				}

				// back left quad
				if(allChildren [i].localPosition.x < shipCenterOfMass.x && allChildren [i].localPosition.z < shipCenterOfMass.z)
				{
					if(allChildren [i].localEulerAngles.y == 0f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 90f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 180f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 270f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
				}

				// front right quad
				if(allChildren [i].localPosition.x > shipCenterOfMass.x && allChildren [i].localPosition.z > shipCenterOfMass.z)
				{
					if(allChildren [i].localEulerAngles.y == 0f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 90f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 180f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 270f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
				}

				// back right quad
				if(allChildren [i].localPosition.x > shipCenterOfMass.x && allChildren [i].localPosition.z < shipCenterOfMass.z)
				{
					if(allChildren [i].localEulerAngles.y == 0f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 90f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 180f) //rot to left
					{
						rotToLeftThrusters.Add(allChildren [i]);
					}
					if(allChildren [i].localEulerAngles.y == 270f) //rot to right
					{
						rotToRightThrusters.Add(allChildren [i]);
					}
				}
			}
		}
	}*/
}
