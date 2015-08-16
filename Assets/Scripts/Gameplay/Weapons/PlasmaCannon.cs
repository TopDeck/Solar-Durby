using UnityEngine;
using System.Collections;

public class PlasmaCannon : MonoBehaviour {

	public Transform projectile;
	public float end = 0.6f;
	public float velocity = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void fire()
	{
		Transform newProjectile  = Instantiate (projectile,
		                                        new Vector3(transform.position.x, transform.position.y+0.25f, transform.position.z+(transform.forward.z * end)),
		                                        transform.rotation) as Transform;
		newProjectile.GetComponent<Rigidbody> ().AddForce (transform.forward * velocity);
	}
}
