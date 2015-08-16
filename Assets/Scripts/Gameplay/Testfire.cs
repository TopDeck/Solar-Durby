using UnityEngine;
using System.Collections;

public class Testfire : MonoBehaviour {

	public Transform cannon;
	public Transform gattlingGun;
	public Transform plasmaCannon;

	// Use this for initialization
	void Start () {
		cannon.gameObject.SendMessage ("fire");
		gattlingGun.gameObject.SendMessage ("fire");
		plasmaCannon.gameObject.SendMessage ("fire");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
