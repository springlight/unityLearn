using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {

    [Range(0,100)]
    public float attractionForce;
    // Use this for initialization
    Rigidbody body;
	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        body.AddForce(transform.localPosition * -attractionForce);
	}
}
