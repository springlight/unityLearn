using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionTest : MonoBehaviour
{
    public GameObject pA;
    public GameObject cB;

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //经验证，只有一个物体的时候，tranform pos == localtransform
            Debug.LogErrorFormat("1111111111World Pos is --》{0} Local Pos ->{1}", pA.transform.position,pA.transform.localPosition);
            Debug.LogErrorFormat("子物体世界坐标系--》{0}/局部坐标系->{1}", cB.transform.position, cB.transform.localPosition);
            pA.transform.position = Vector3.up*2;
            Debug.LogErrorFormat("222222World Pos is --》{0} Local Pos ->{1}", pA.transform.position, pA.transform.localPosition);
            Debug.LogErrorFormat("子物体世界坐标系--》{0}/局部坐标系->{1}", cB.transform.position, cB.transform.localPosition);
            pA.transform.localPosition = Vector3.up * 3;

            Debug.LogErrorFormat("33333World Pos is --》{0} Local Pos ->{1}", pA.transform.position, pA.transform.localPosition);
            Debug.LogErrorFormat("子物体世界坐标系--》{0}/局部坐标系->{1}", cB.transform.position, cB.transform.localPosition);

            Debug.LogError("----------------------------");
            cB.transform.position = Vector3.up * 6;
            Debug.LogErrorFormat("子物体世界坐标系--》{0}/局部坐标系->{1}", cB.transform.position, cB.transform.localPosition);
            cB.transform.localPosition = Vector3.up * 9;
            Debug.LogErrorFormat("子物体世界坐标系--》{0}/局部坐标系->{1}", cB.transform.position, cB.transform.localPosition);
        }
        Debug.LogFormat("父物体world-->{0}/local->{1}", pA.transform.position, pA.transform.localPosition);
        Debug.LogFormat("子物体world-->{0}/local->{1}", cB.transform.position, cB.transform.localPosition);
    }
}
