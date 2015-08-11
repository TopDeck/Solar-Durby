/**** WORK IN PROGRESS ****/


using UnityEngine;
using System.Collections;

public class BlockSpawner : MonoBehaviour {

	public Transform cube;
	public Transform cockPit;
	public Transform thruster;
	public Transform[][] allBlocks;

	// Use this for initialization
	void Start () {
		allBlocks = new Transform[][]{
			new Transform[]{cube,cube},
			new Transform[]{cockPit,cockPit},
			new Transform[]{thruster,thruster}
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/*public static Transform getBlock(int blockId, bool designMode)
	{
		Transform newBlock;
		int blockType = blockId % 100;
		int blockNumber = blockId - blockType*100;
		
		newBlock = Instantiate(allBlocks[blockType][blockNumber], new Vector3(0,0,0), Quaternion.identity) as Transform;

		return newBlock;
	}*/
}
