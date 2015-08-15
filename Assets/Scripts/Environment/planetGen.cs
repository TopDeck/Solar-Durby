using UnityEngine;
using System.Collections;

public class planetGen : MonoBehaviour {

	public Transform planetPrefab;
    public Transform sunPrefab;
    public Transform satellitePrefab;

    public int minPlanets;
	public int maxPlanets;
    public int maxSatNum;
    public int maxPlanetSpeed;
    public int maxRadius;

    public int minSatRadius;
    public int maxSatRadius;
    public int maxSatSpeed;

    public int spawnFloor;

    public int minSunSize;
    public int maxSunSize;

    private Transform Sun;


	// Use this for initialization
	void Start ()
	{
        int planetNum;
        float sunSize;
	    Sun = Instantiate(sunPrefab);
	    Sun.position = Vector3.zero;
	    sunSize = minSunSize + (Random.value*maxSunSize);
        Sun.localScale = new Vector3(sunSize,sunSize,sunSize);
		
        planetNum = minPlanets + (int)(Random.value * maxPlanets);
		
        for (int i = 0; i < planetNum; i++)
		{
		    Transform spawn = spawnChild(planetPrefab, Sun);
		    int satNum = (int)(Random.value * maxSatNum);
		    for (int j = 0; j < satNum; j++)
		    {
		        spawnChild(satellitePrefab, spawn);
		    }
		}

	    foreach (Transform child in Sun.transform)
	    {
            child.RotateAround(child.parent.position, Vector3.down, Random.value * 360);
	    }
	}
	// Update is called once per frame
	void Update ()
	{
	    foreach (Transform child in Sun.transform)
	    {
	        child.RotateAround(Vector3.zero,
                               Vector3.down,
                               child.GetComponent<Planet>().rotationSpeed * Time.deltaTime);
	        foreach (Transform satellite in child)
	        {
                satellite.RotateAround(child.position,
                               Vector3.down,
                               satellite.GetComponent<Planet>().rotationSpeed * Time.deltaTime * 4);
	        }
	    }
	}

    Transform spawnChild(Transform type,Transform parent)
    {
        Transform spawn = Instantiate(type);
        float spawnScale = Random.value;
        float spawnSize = spawnScale * parent.lossyScale.x;
        spawn.GetComponent<Rigidbody>().mass = Sun.GetComponent<Rigidbody>().mass * spawnScale;
        spawn.localScale = new Vector3(spawnSize, spawnSize, spawnSize);
        if (parent.tag == "Planet")
        {
            spawn.position = new Vector3(parent.position.x, spawnFloor, parent.position.z + minSatRadius+(Random.value*maxSatRadius));
            spawn.GetComponent<Planet>().rotationSpeed = spawnScale * maxSatSpeed;
        }
        else
        {
            spawn.position = new Vector3(parent.position.x, spawnFloor, parent.position.z + (Random.value*maxRadius));
            spawn.GetComponent<Planet>().rotationSpeed = spawnScale * maxPlanetSpeed;
        }
        spawn.SetParent(parent);
        return spawn;
    }
}
