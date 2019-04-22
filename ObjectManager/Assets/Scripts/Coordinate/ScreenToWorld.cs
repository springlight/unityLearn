using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToWorld : MonoBehaviour {

    public Transform cube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            Plane pla = new Plane(Camera.main.transform.forward,Camera.main.transform.position);
            float dis = pla.GetDistanceToPoint(cube.position);
            cube.position = Camera.main.ScreenToWorldPoint(new Vector3(pos.x,pos.y,cube.position.z));
        }
       
	}
}
