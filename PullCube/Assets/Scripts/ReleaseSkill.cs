using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseSkill : MonoBehaviour
{
    public GameObject doubleHand;
    public float pullSpeed = 0;//拉伸速度
    public float maxPull = 10;//最大拉伸距离
    Vector3 handPos;//双手初始位置
    public bool pullStar = false;//技能是正在释放
    bool holdGo = false;//是否拿着东西
    private RaycastHit hit;//射线照射的点
    public LayerMask mask;//射线能打到的层
    Vector3 hitPos;//获取照射点坐标

	// Use this for initialization
	void Start () {
        Debug.LogError("ReleaseSkill");
    }
	
    //手掌的运动轨迹
	private void HandTrail(float trailTime)
    {
        //找到双手自身和它的子物体(两个手掌）
        Transform[] a = doubleHand.GetComponentsInChildren<Transform>();
       // Debug.LogError("双手和自身 Transform个数==="+ a.Length);
        for(int i = 0; i < a.Length; i++)
        {
          //  Debug.LogError("孩子名称。。" + a[i].name);
            //设定两个手掌运动轨迹存留时间，当为0时轨迹不显示
            TrailRenderer trailRenderer = a[i].GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.time = trailTime;
            }
        }
    }
    /// <summary>
    /// 释放技能
    /// </summary>
    private void Release()
    {
        HandTrail(1);//显示轨迹
        //获取刺客双手的当前坐标
        handPos = doubleHand.transform.position;
        //拉伸速度，方向向前
        pullSpeed = 50;
        //设置技能释放状态
        pullStar = true;
        //从双手位置发射射线
        if(Physics.Raycast(handPos,doubleHand.transform.forward,out hit, maxPull, mask))
        {
            //如果射线打到东西，获取照射点坐标
            hitPos = hit.point;
        }
    }
    /// <summary>
    /// 拉伸手臂
    /// </summary>
    private void Pull()
    {
        if (pullStar)
        {
            //双手在自身坐标Z轴移动，速度方向待定
            doubleHand.transform.Translate(0, 0, Time.deltaTime * pullSpeed);
            //如果射线没碰到东西，双手达到最长范围时候返回
            if(hit.transform == null && (doubleHand.transform.position - handPos).magnitude > maxPull)
            {
                //轨迹时间显示为0，即不显示轨迹
                HandTrail(0);
                pullSpeed = -50;//双手向后移动
            }
            //如果碰到东西，则双手接近改东西，抓住他返回
            else if(hit.transform != null &&(doubleHand.transform.position - hitPos).magnitude < 0.5f)
            {
                //抓住射线打到的cube(设为双手的子物体，跟随双手移动)
                hit.transform.parent = doubleHand.transform;
                holdGo = true;//表示正拿着cube
                HandTrail(0);//隐藏轨迹
                pullSpeed = -50;//双手往后运动
            }
            //如果双手返回时，接近出发时位置，则双手位置复位，停止移动
            if(pullSpeed < 0 && (doubleHand.transform.position - handPos).magnitude < 0.5f)
            {
                pullSpeed = 0;//移动停止
                doubleHand.transform.position = handPos;//位置复位
                pullStar = false;
            }
        }
    }
    /// <summary>
    /// 拿到Cube后保持让Cube在手里拿稳
    /// </summary>
    /// <param name="Cube"></param>
    private void KeepPos(Transform Cube)
    {
        //如果父物体不为空，表示抓住了
        if(Cube != null && Cube.parent != null)
        {
            Cube.position = doubleHand.transform.position;
            Cube.rotation = doubleHand.transform.rotation;
        }

    }

    private void Update()
    {
        
        //如果技能没有释放，也没有拿东西
        if (!pullStar && !holdGo)
        {
            if (Input.GetMouseButton(0))
            {
                Release();//释放技能
            }
        }
        else if (holdGo)
        {
            //点击鼠标右键，放下东西
            if (Input.GetMouseButton(1))
            {
                hit.transform.SetParent(null);
                holdGo = false;
            }
        }
        Pull();
        KeepPos(hit.transform);
    }
}
