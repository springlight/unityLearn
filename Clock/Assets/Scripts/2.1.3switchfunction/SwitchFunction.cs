using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchFunction : MonoBehaviour
{
    // Start is called before the first frame update
    //保证resolution的范围（10,100)的整数
    [Range(10, 100)]
    public int resolution = 10;
    public Transform pointPrefab;
    //[Range(0, 1)]
    //public int function;
    public GraphFunctionName function;
    Transform[] points;
    GraphVector3[] functions = 
    {
        GraphFunction.SinVectorFunction,
        GraphFunction.Sin2DVectorFunction,
        GraphFunction.MultiSineVectorFunction,
        GraphFunction.Ripple,
        GraphFunction.Cylinder
    };
    void Awake()
    {
        points = new Transform[resolution *resolution];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }
    void Start()
    {
        DoubleLoopAnimating();
    }
    private void Update()
    {
        float t = Time.time;
        GraphVector3 f = functions[(int)function];
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;
                points[i].localPosition = f(u, v, t);
            }
        }
        //for (int i = 0; i < points.Length; i++)
        //{


        //    Transform point = points[i];
        //    Vector3 pos = point.localPosition;

        //   pos.y = f(pos.x, pos.z,t);
        //    point.localPosition = pos;

        //}
    }
    /// <summary>
    /// 两层循环
    /// </summary>
    private void DoubleLoopAnimating()
    {
        Vector3 pos = Vector3.zero;

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0, z = 0; z < resolution; z++ )
        {
            pos.z = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                Transform point = Instantiate(pointPrefab);

                //保证x的范围在（-1,1）
                pos.x = (x + 0.5f) * step - 1f;
                point.localPosition = pos;
                point.localScale = scale;
                point.SetParent(transform, false);
                try
                {
                    points[i] = point;

                }
                catch (System.Exception e)
                {
                    Debug.LogError("i-->" + i);
                 
                }
               
            }
          

        }
    }

    /// <summary>
    /// 动画版f(x) = x;
    /// 一个循环
    /// </summary>
    private void AnimatingYequalX()
    {
        Vector3 pos = Vector3.zero;

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0,x = 0,z = 0; i < points.Length; i++,x++)
        {
            Transform point = Instantiate(pointPrefab);
            //
            if(x == resolution)
            {
                x = 0;
                z += 1;
            }
            //保证x的范围在（-1,1）
            pos.x = (x + 0.5f) * step - 1f;
            pos.z = (z + 0.5f) * step - 1f;
           // pos.y = pos.x * pos.x * pos.x;
            point.localPosition = pos;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;

        }
    }
    //f(x) = x*x*x
    private void YequalXXX()
    {
        Vector3 pos = Vector3.zero;

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //保证x的范围在（-1,1）
            // point.localPosition = Vector3.right *((i + 0.5f) / 5f - 1f);
            pos.x = (i + 0.5f) * step - 1f;
            pos.y = pos.x * pos.x * pos.x;
            point.localPosition = pos;
            point.localScale = scale;
            point.SetParent(transform, false);

        }
    }
    //动态调整点数的f(x) = x*x
    private void MultiYequalXX()
    {
        Vector3 pos = Vector3.zero;

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //保证x的范围在（-1,1）
            // point.localPosition = Vector3.right *((i + 0.5f) / 5f - 1f);
            pos.x = (i + 0.5f) * step - 1f;
            pos.y = pos.x * pos.x;
            point.localPosition = pos;
            point.localScale = scale;
            point.SetParent(transform, false);

        }
    }

    //f(x) = x*x图像
    private void YequalXX()
    {
        Vector3 pos = Vector3.zero;
        Vector3 scale = Vector3.one / 5f;
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //保证x的范围在（-1,1）
            // point.localPosition = Vector3.right *((i + 0.5f) / 5f - 1f);
            pos.x = (i + 0.5f) / 5f - 1f;
            pos.y = pos.x * pos.x;
            point.localPosition = pos;
            point.localScale = scale;
            point.SetParent(transform);
        }
    }

    //f(x) = x 图像
    private void YequalX()
    {

        Vector3 pos = Vector3.zero;
        Vector3 scale = Vector3.one / 5f;
        for (int i = 0; i < 10; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //保证x的范围在（-1,1）
            // point.localPosition = Vector3.right *((i + 0.5f) / 5f - 1f);
            pos.x = (i + 0.5f) / 5f - 1f;
            pos.y = pos.x;
            point.localPosition = pos;
            point.localScale = scale;
            point.SetParent(transform);
        }
    }


}
