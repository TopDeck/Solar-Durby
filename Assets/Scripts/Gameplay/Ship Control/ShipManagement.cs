/*** WORK IN PROGRESS ***/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class ShipManagement : MonoBehaviour {

	//Links to prefabs
	public Transform cube;
	public Transform cockPitPref;
	public Transform thruster;

	public Transform cockPit;

	// Use this for initialization
	void Start () {
		loadShip ();
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	//Creates the block specified by its id within the save file
	//then rotates it to match
	Transform createBlock(string blockID, Vector3 position, float rotation)
	{
		Transform newBlock = null;

		if(blockID == "101")
		{
			newBlock = Instantiate(cube, position, transform.rotation) as Transform;
			newBlock.Rotate(new Vector3(0,90*rotation,0)); //rotation = rotationMultiplier
		}
		if(blockID == "201")
		{
			newBlock = Instantiate(thruster, position, transform.rotation) as Transform;
			newBlock.Rotate(new Vector3(0,90*rotation,0));
		}
		if(blockID == "301")
		{
			newBlock = Instantiate(cockPitPref, position, transform.rotation) as Transform;
			newBlock.Rotate(new Vector3(0,90*rotation,0));
		}

		return newBlock;
	}

	// Builds the players ship from the save file
	// NEEDS LOST OF WORK
	void loadShip()
	{
		List<string> blockStrings = new List<string> (); //Stores the list of blocks to be created
		List<Transform> blocks = new List<Transform>();	//Stores the blocks after creation except the cockpit
		string blockString = "";

		//Sets up the search string for the ship file
		string fileName = "tempShip";
		string filePath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop)) + @"\" + fileName + ".txt";
		
		StreamReader theReader = new StreamReader (filePath); //Gets the ship from the save location

		//reads each line of the file and saves it into the build list (blockStrings)
		while (blockString != null) { 
			blockString = theReader.ReadLine();
			blockStrings.Add(blockString);
		}
		blockStrings.RemoveAt (blockStrings.Count-1);//remove the last element of the list as it will be null

		theReader.Close();

		//loops through the build list and rips the information from each string
		foreach (string block in blockStrings) {
			string blockID = block.Substring(0,3); //exact object to be created

			//takes the position as a char converts it to int then casts it to float
			//then I have some scale issues im accounting for (will be fixed later)
			Vector3 blockPosition = new Vector3(
				(float)Char.GetNumericValue(block[4])/2 + .54f,
				(float)Char.GetNumericValue(block[9])/2 + .54f,
				(float)Char.GetNumericValue(block[14])/2 + .54f);

			//same as above rotatonMultiplier is (1 - 4) range
			float rotatonMultiplier = (float)Char.GetNumericValue(block[18]);

			Transform newBlock = createBlock(blockID, blockPosition, rotatonMultiplier);

			if(blockID == "301")
			{
				cockPit = newBlock;
			}
			else
			{
				blocks.Add(newBlock);
			}
		}

		//Loops through the created blocks and sends out raycasts to detect the blocks to create links to
		foreach(Transform block in blocks)
		{
			RaycastHit hit;
			block.SetParent(cockPit);

			if(block.tag != "Thruster") //dont need links on thrusters
			{
				//send raycast forward
				if (Physics.Raycast(new Vector3(block.position.x, block.position.y + 0.25f,block.position.z), block.forward, out hit))
				{
					if(hit.rigidbody != block.gameObject.GetComponent<Rigidbody>()) //make sure the object found isnt its self
					{
						FixedJoint fixJoint = block.gameObject.AddComponent<FixedJoint>(); //attach a new joint to the block
						fixJoint.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>(); //make it link to the other object
					}
				}
				if (Physics.Raycast(new Vector3(block.position.x, block.position.y + 0.25f,block.position.z), -block.forward, out hit))
				{
					if(hit.rigidbody != block.gameObject.GetComponent<Rigidbody>())
					{
						FixedJoint fixJoint = block.gameObject.AddComponent<FixedJoint>();
						fixJoint.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
					}
				}
				if (Physics.Raycast(new Vector3(block.position.x, block.position.y + 0.25f,block.position.z), block.right, out hit))
				{
					if(hit.rigidbody != block.gameObject.GetComponent<Rigidbody>())
					{
						FixedJoint fixJoint = block.gameObject.AddComponent<FixedJoint>();
						fixJoint.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
					}
				}
				if (Physics.Raycast(new Vector3(block.position.x, block.position.y + 0.25f,block.position.z), -block.right, out hit))
				{
					if(hit.rigidbody != block.gameObject.GetComponent<Rigidbody>())
					{
						FixedJoint fixJoint = block.gameObject.AddComponent<FixedJoint>();
						fixJoint.connectedBody = hit.transform.gameObject.GetComponent<Rigidbody>();
					}
				}
			}
		}
	}
}
