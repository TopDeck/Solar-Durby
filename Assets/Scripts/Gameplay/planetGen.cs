using UnityEngine;
using System.Collections;

public class planetGen : MonoBehaviour {

	public Transform planet;

	public int maxPlanets;
	public int maxX;
	public int maxZ;
	public int spawnFloor;

	private int planetNum;

	// Use this for initialization
	void Start () {
		planetNum = (int)(Random.value * maxPlanets);
		for (int i = 0; i < planetNum; i++) 
		{
			Transform spawn = Instantiate(planet);
			spawn.position = new Vector3(Random.value * maxX,spawnFloor,Random.value*maxZ);
		}
	}
	// Update is called once per frame
	void Update () {		
	}
}
