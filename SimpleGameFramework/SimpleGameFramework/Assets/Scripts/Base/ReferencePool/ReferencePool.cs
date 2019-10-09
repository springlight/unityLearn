using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferencePool
{
    private static Dictionary<string, ReferenceCollection> s_ReferenceCollections = new Dictionary<string, ReferenceCollection>();

    /// <summary>
    /// 获取引用数量
    /// </summary>
    public static int Count
    {
        get { return s_ReferenceCollections.Count; }
    }

    /// <summary>
    /// 获取引用结合
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>
    private static ReferenceCollection GetReferenceCollection(string fullName)
    {
        ReferenceCollection referenceCollection = null;
        lock (s_ReferenceCollections)
        {
            if(!s_ReferenceCollections.TryGetValue(fullName,out referenceCollection))
            {
                referenceCollection = new ReferenceCollection();
                s_ReferenceCollections.Add(fullName, referenceCollection);
            }
        }
        return referenceCollection;
    }

    /// <summary>
    /// 清除所有引用集合
    /// </summary>
    public static void ClearAll()
    {
        lock (s_ReferenceCollections)
        {
            foreach(KeyValuePair<string,ReferenceCollection> reference in s_ReferenceCollections)
            {
                reference.Value.RemoveAll();
            }
            s_ReferenceCollections.Clear();

        }
    }


    public static void Add<T>(int count)where T : class, IReference, new()
    {
        GetReferenceCollection(typeof(T).FullName).Add<T>(count);
    }

    public static void Remove<T>(int count) where T : class, IReference
    {
        GetReferenceCollection(typeof(T).FullName).Remove<T>(count);
    }

    public static void RemoveAll<T>()where T : class, IReference
    {
        GetReferenceCollection(typeof(T).FullName).RemoveAll();
    }



    public static T Acquire<T>()where T : class, IReference, new()
    {
        return GetReferenceCollection(typeof(T).FullName).Acquire<T>();
    }

    public static IReference Acquire(Type referecType)
    {
        return GetReferenceCollection(referecType.FullName).Acquire(referecType);
    }


    public static void Release<T>(T reference)where T : class, IReference
    {
        if(reference == null)
        {
            Debug.LogError("要归还的引用为空");
        }

        GetReferenceCollection(typeof(T).FullName).Release(reference);
    }
}
