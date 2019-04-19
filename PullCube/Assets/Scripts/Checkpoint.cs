using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    // Use this for initialization
    public GameObject[] points;//找到所有的灯，判断他们是否亮着
    private void Awake()
    {
        Time.timeScale = 1;//程序运行时设定系统时间为正常
    }
    private void Update()
    {
        if (AllStart())
        {
            Debug.LogError("通关啦。。");
            Time.timeScale = 0;//暂停游戏
        }
    }

    bool AllStart()
    {
        for(int i= 0; i < points.Length; i++)
        {
            //只要一个灯不亮，就返回false
            if (!points[i].GetComponent<StreetLamp>().lightStar)
                return false;
        }
        return true;
    }
}
