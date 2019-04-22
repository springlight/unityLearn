using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToWorld : MonoBehaviour {

    public Transform p;
    public Transform c;
	// Use this for initialization
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.LogError("子本地坐标系-》" + c.localPosition);
            Debug.LogError("子世界坐标系-->" + c.position);
            Debug.LogError("本地转世界--->" + p.TransformPoint(c.localPosition));

            Vector3 thePos = p.TransformPoint(2, 0, 0);
            Instantiate(p, thePos, p.rotation);
        }
        //世界转本地
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.LogError("子本地坐标系-》" + c.localPosition);
            Debug.LogError("子世界坐标系-->" + c.position);
            Debug.LogError("世界--->" + p.InverseTransformPoint(c.position));
        }
      
	}
	
	
}
