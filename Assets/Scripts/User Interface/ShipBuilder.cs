/*** WORK IN PROGRESS ***/

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class ShipBuilder : MonoBehaviour {

	public float cellSize = 1.8f;
	public int cellsAcross = 10;
	public int cellsDown = 10;

	public Transform cell;
	public Transform cube;
	public Transform cockPit;
	public Transform thruster;

	public Transform shipManager;
	public Transform selectedBlock;
	
	Transform currentCell;

	private Transform[] grid;
	private int cellCount;

	// Use this for initialization
	void Start () {
		grid = new Transform[cellsAcross*cellsDown];//Initialize array
		
		//generate grid
		for (int i=0; i<cellsAcross; i++) { //x axis
			for (int j=0; j<cellsDown; j++) { //z axis
				grid[cellCount] = Instantiate(cell, new Vector3(1*i,0,1*j), Quaternion.identity) as Transform;
				cellCount++;
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {
		//Track the mouse position
		currentCell = targetTransform ();

		if (currentCell != null && selectedBlock != null) {
			selectedBlock.position = currentCell.position;
		}

		// Apply the selected block
		if (Input.GetMouseButtonDown (0)) {
			if(selectedBlock != null && targetTransform() != null){
				if(currentCell.tag != "Cell"){
					Destroy(currentCell.gameObject);
				}
				createBlock(selectedBlock, selectedBlock.position, selectedBlock.rotation);
			}
		}

		//Rotate the selected block if one is selected
		if(Input.GetAxis("Mouse ScrollWheel") > 0){
			if(selectedBlock != null){
				selectedBlock.transform.Rotate(new Vector3(0,90,0));
			}
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0){
			if(selectedBlock != null){
				selectedBlock.transform.Rotate(new Vector3(0,-90,0));
			}
		}

		//Remove the block in the current cell
		if (Input.GetMouseButtonDown (1)) {
			if (currentCell != null) {
				if (currentCell.childCount > 0) {
					Transform[] allChildren = currentCell.GetComponentsInChildren<Transform> (); //gets all children plus the parent
					if(allChildren[0].tag != "Cell") Destroy(allChildren[0].gameObject); //one of them will be the parent
					else Destroy(allChildren[1].gameObject);
				}
			}
		}
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (5, 5, 100, 30), "Blocks")) {
			if(selectedBlock != null){
				Destroy(selectedBlock.gameObject);
			}
			selectedBlock = createBlock(cube);
		}
		
		if (GUI.Button (new Rect (5, 35, 100, 30), "Extensions")) {
			if(selectedBlock != null){
				Destroy(selectedBlock.gameObject);
			}
			selectedBlock = createBlock(thruster);
		}
		
		if (GUI.Button (new Rect (5, 65, 100, 30), "Systems")) {
			if(selectedBlock != null){
				Destroy(selectedBlock.gameObject);
			}
			selectedBlock = createBlock(cockPit);
		}
		
		if (GUI.Button (new Rect (5, 205, 100, 30), "Save")) {
			saveShip();
		}
		
		if (GUI.Button (new Rect (5, 235, 100, 30), "Load")) {
			loadShip();
		}
	}

	//Create the selected prefab destroying any other object currently in the cell
	Transform createBlock(Transform prefab, Vector3 position, Quaternion rotation)
	{
		Transform newBlock = Instantiate(prefab, position, rotation) as Transform;
		if (currentCell != null) {
			if (currentCell.childCount > 0) {
				Transform[] allChildren = currentCell.GetComponentsInChildren<Transform> ();
				if(allChildren[0].tag != "Cell") Destroy(allChildren[0].gameObject);
				else Destroy(allChildren[1].gameObject);
			}
		}
		newBlock.SetParent (currentCell);
		return newBlock;
	}

	//Creates the newly selected block ready to be painted
	Transform createBlock(Transform prefab)
	{
		Transform newBlock = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity) as Transform;

		if (currentCell != null) {
			if (currentCell.childCount > 0) {
				Transform[] allChildren = transform.GetComponentsInChildren<Transform> ();
				Debug.Log (allChildren);
			}
		}
		newBlock.SetParent (currentCell);
		return newBlock;
	}

	//saves the ship design
	void saveShip()
	{
		string mapName = "tempShip";
		string blocksString = ""; //will store all the block strings then be saved to the file

		for (int i=0; i<grid.Length; i++) { //For every cell in the grid
			if (grid[i].childCount > 0) { //if the cell has a child
				Transform[] allChildren = grid[i].GetComponentsInChildren<Transform> (); //get the child (Block)
				string block = ""; //string that will define a single block and be added to the blocksString

				if(allChildren[0].tag != "Cell") //Because one of them will be the parent
				{
					block+=(allChildren[0].GetComponent("TypeID") as TypeID).blockID; //grab the blockId from the object
					block+=allChildren[0].position;	//add the position to the string
					block+= (allChildren[0].eulerAngles.y / 90.0f) + "\r\n"; //add the rotation multiplier
				}
				else
				{
					block+=(allChildren[1].GetComponent("TypeID") as TypeID).blockID;
					block+=allChildren[1].position;
					block+= (allChildren[1].eulerAngles.y / 90.0f) + "\r\n";
				}
				blocksString+=block;
			}
		}
		string filePath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop)) + @"\" + mapName + ".txt"; //define the file

		File.Create(filePath).Close(); //create the file

		File.WriteAllText(filePath, blocksString); //save the string to the file
	}

	//Not currently working
	void loadShip()
	{
		string mapName = "tempShip";
		string filePath = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop)) + @"\" + mapName + ".txt";

		StreamReader theReader = new StreamReader (filePath);

		Debug.Log (theReader.ReadLine());
		Debug.Log (theReader.ReadLine());
		Debug.Log (theReader.ReadLine());

		theReader.Close();
	}

	//Tracks the mouse position and determines the cell it is over.
	Transform targetTransform() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Project raycast into scene from the position of the mouse
		RaycastHit hitinfo;
		int layerMask = ~(1<<8); // Tells the Raycast to ignore layer 8 (Blocks)
		
		if(Physics.Raycast(ray, out hitinfo, Mathf.Infinity, layerMask)) //"out" is a C# keyword for use in special situations explained here:"https://msdn.microsoft.com/en-us/library/t3c3bfhx.aspx"
		{
			if(hitinfo.transform.tag != "Cell")
				Debug.Log(hitinfo.transform.name);
			return hitinfo.transform; //Returns the transform of the collider that is moused over
		}
		return null;
	}
}
