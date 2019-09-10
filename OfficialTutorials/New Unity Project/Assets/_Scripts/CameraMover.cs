using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {


    public Transform follw;
    public float distanceAway = 5.0f;//表示摄像机在目标对象后面的距离
    public float distanceUp = 2.0f;//表示摄像机目标对象上的高度
    public float smooth = 1.0f;//插值系数
    public Vector3 targetPos;//表示相机的移动位置

	// Use this for initialization
	void Start ()
    {
		
	}


    private void LateUpdate()
    {
        targetPos = follw.position + Vector3.up * distanceUp - follw.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
        transform.LookAt(follw);
    }


}
