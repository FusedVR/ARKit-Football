using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {
	public float shotForce;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * shotForce);

		Destroy (gameObject, 5f);
	}

	void Update() {
		// A hack for making the football fly more realistically
		transform.LookAt (rb.velocity);
	}
}
