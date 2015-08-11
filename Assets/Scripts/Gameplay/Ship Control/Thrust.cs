using UnityEngine;
using System.Collections;

public class Thrust : MonoBehaviour {

	public float thrusterPower = 1.0f; //If we want varied thruster power
	public float rotationalPower = 1.0f; //If we want varied rotation power
	public float dampeningStrength = 1.0f; //If we want varied dampening strength
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddAccelForce() //Adds thrusterPower to rigidbody velocity calculations
	{
		GetComponent<Rigidbody>().AddForce (transform.forward * thrusterPower);
	}

	public void AddRotateForce() //Adds thrusterPower to rigidbody velocity calculations
	{
		GetComponent<Rigidbody>().AddForce (transform.forward * rotationalPower);
	}

	public void AddDampeningForce(float powerPercentage) //Adds thrusterPower to rigidbody velocity calculations
	{
		GetComponent<Rigidbody>().AddForce (transform.forward * dampeningStrength);
	}
}
