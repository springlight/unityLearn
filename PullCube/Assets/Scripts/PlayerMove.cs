using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float xSpeed = 0;//当前x轴方向的速度
    public float zSpeed = 0;
    public float maxSpeed = 5;
    public GameObject foot;
    public ReleaseSkill skill;
	// Use this for initialization
	void Start () {
        Debug.LogError("PlayerMove ----》" + skill.GetHashCode());
    }
	
	// Update is called once per frame
	void Update ()
    {
    
        //没有释放技能才能移动
        if (!skill.pullStar)
        {
            if (Input.GetKey(KeyCode.A) && transform.position.x > -9) //按住A在边界范围内往设定x轴负方向的速度
            {
                if (xSpeed > -maxSpeed)
                    xSpeed -= Time.deltaTime * 10;
                else if (xSpeed < -maxSpeed)
                    xSpeed = -maxSpeed;
            }
            else if (Input.GetKey(KeyCode.D) && transform.position.x < 9) //按住D在边界范围内往设定x轴正方向的速度
            {
                if (xSpeed < maxSpeed)
                    xSpeed += Time.deltaTime * 10;
                else if (xSpeed > maxSpeed)
                    xSpeed = maxSpeed;
            }
            else //超出边界就停止移动
                xSpeed = 0;
            if (Input.GetKey(KeyCode.S) && transform.position.z > -9) //按住S在边界范围内往设定z轴负方向的速度
            {
                if (zSpeed > -maxSpeed)
                    zSpeed -= Time.deltaTime * 10;
                else if (zSpeed < -maxSpeed)
                    zSpeed = -maxSpeed;
            }
            else if (Input.GetKey(KeyCode.W) && transform.position.z < 9) //按住W在边界范围内往设定z轴正方向的速度
            {
                if (zSpeed < maxSpeed)
                    zSpeed += Time.deltaTime * 10;
                else if (zSpeed > maxSpeed)
                    zSpeed = maxSpeed;
            }
            else //超出边界就停止移动
                zSpeed = 0;
            transform.Translate(Time.deltaTime * xSpeed, 0, Time.deltaTime * zSpeed); //移动      
            foot.transform.Rotate(zSpeed * 2, 0, -xSpeed * 2, Space.World);  //脚旋转，当在X轴移动是，是以Y轴为旋转轴旋转...
        }
	}
}
