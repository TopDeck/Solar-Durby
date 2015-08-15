using UnityEngine;


public class Planet : MonoBehaviour
{
    public float rotationSpeed;
    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision Detected");
        /*if (col.gameObject.tag == "Planet")
        {
            Debug.Log("Planet Spawn Collision");
            Destroy(col.gameObject);
        }
         */
        if (col.gameObject.tag == "Sun")
        {
            Debug.Log("Sun Spawn Collision");
            Destroy(gameObject);
        }
    }
}