using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Start is called before the first frame update
    //保证resolution的范围（10,100)的整数
    [Range(10,100)]
    public int resolution = 10;
    public Transform pointPrefab;

    Transform[] points;
    void Awake()
    {
        points = new Transform[resolution];
    }
    void Start()
    {
        //YequalX();
        // YequalXX();
        //MultiYequalXX();
        // YequalXXX();
        AnimatingYequalXXX();
    }
    private void Update()
    {
        for(int i = 0; i < points.Length; i++)
        {
          

            float step = 2f / resolution;
            Vector3 scale = Vector3.one * step;

            Transform point = points[i];
            Vector3 pos = point.localPosition;
            //pos.y = pos.x * pos.x * pos.x;
            pos.y = Mathf.Sin(Mathf.PI * (pos.x+Time.time));
            point.localPosition = pos;
           
        }
    }
    /// <summary>
    /// 动画版f(x) = x*x*x;
    /// </summary>
    private void AnimatingYequalXXX()
    {
        Vector3 pos = Vector3.zero;

        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            //保证x的范围在（-1,1）
            pos.x = (i + 0.5f) * step - 1f;
            pos.y = pos.x * pos.x * pos.x;
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
            point.SetParent(transform,false);

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
