using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldToScreen : MonoBehaviour {

    public Transform cube;
    public Text text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = cube.position+ Vector3.up;
        text.transform.position = Camera.main.WorldToScreenPoint(pos);
	}
}
