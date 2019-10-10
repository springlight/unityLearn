using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMark : MonoBehaviour
{


    private int i;
    public LineRenderer obs;
    public GameObject run;
    Vector3 RunStart;
    Vector3 RunNext;

    // Use this for initialization
    void Start()
    {
        RunStart = run.transform.position;

        i = 1;
    }

    // Update is called once per frame  
    private float time = 0;
    void Update()
    {

        RunNext = run.transform.position;
       
        if (RunStart != RunNext)
        {
           
           
            i++;
            obs.SetVertexCount(i);//设置顶点数 
            obs.SetPosition(i - 1, run.transform.position);

        }

        RunStart = RunNext;
    }
}
