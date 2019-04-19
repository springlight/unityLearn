using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate float GraphDelegate(float x,float z,float t);
public delegate Vector3 GraphVector3(float u, float v, float t);
public class GraphFunction
{
    const float pi = Mathf.PI;
    //public static float SineFunction(float x,float z, float t)
    //{
    //    return Mathf.Sin(Mathf.PI * (x + t));
    //}
    public static Vector3 Cylinder(float u, float v, float t)
    {
        Vector3 p;
        p.x = Mathf.Sin(pi * u);
        p.y = 0f;
        p.z = Mathf.Cos(pi * u);
        return p;

    }
    public static Vector3 SinVectorFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.z = z;
        return p;
    }
    //f(x,t)= sin(pi(x + t))+sin(2pi(x + t))/2
    //public static float MultiSineFunction(float x,float z, float t)
    //{
    //    float y = Mathf.Sin(pi * (x + t));
    //    y += Mathf.Sin(2f * pi * (x + t)) / 2f;
    //    //因为两个sin的范围都是（-1,1）,第二个sin取1/2值
    //    //则现在y的范围值在（-1.5,1.5），乘以2/3把y的范围调整到
    //    //(-1,1)
    //    y *= 2f / 3f;
    //    return y;
    //}

    public static Vector3 MultiSineVectorFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        p.z = z;
        return p;
    }

    public static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
        p.y += Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    public static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(pi * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = z;
        return p;
    }

    ////f(x,z,t)= sin(pi(x+t))/2 + sin(pi(z+t))/2
    //public static float Sin2DFunction(float x,float z,float t)
    //{
    //    float y = Mathf.Sin(pi * (x + t));
    //    y += Mathf.Sin(pi * (z + t));
    //    y *= 0.5f;//乘法中比除法运算快，所以不用/2
    //    return y;
    //}


    public static Vector3 Sin2DVectorFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(pi * (x + t));
        p.y += Mathf.Sin(pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;
    }

    //public static float Ripple(float x,float z,float t)
    //{
    //    float d = Mathf.Sqrt(x * x + z * z);
    //    float y = Mathf.Sin(pi * (4f * d - t));
    //    y /= 1f + 10f * d;
    //    return y;
    //}
}
