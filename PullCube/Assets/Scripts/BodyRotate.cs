using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotate : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    public LayerMask layerName;
    public GameObject body;
    public ReleaseSkill skill;
	// Use this for initialization
	void Start () {
        Debug.LogError("BodyRotate--->" + skill.GetHashCode());
	}
	
	// Update is called once per frame
	void Update () {
        //如果技能没有释放。则可以旋转主角
        if (!skill.pullStar)
        {
            body.transform.LookAt(InputMove());
        }
	}
    /// <summary>
    /// 获取鼠标在世界中的移动
    /// </summary>
    /// <returns></returns>
    private Vector3 InputMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);//镜头射出一条射线，跟随鼠标位置
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        if (Physics.Raycast(ray,out hit, 100, layerName))
        {
            //获取照射点的坐标（也是鼠标所在位置，身体的y值给y轴，让返回的坐标不用停留在地面上)
            return new Vector3(hit.point.x, hit.point.y + body.transform.position.y, hit.point.z);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
