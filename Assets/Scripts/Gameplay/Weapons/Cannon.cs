using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public Transform projectile;
	public float end = 0.4f;
	public float velocity = 2f;
	public float fireInterval = 1f;

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
