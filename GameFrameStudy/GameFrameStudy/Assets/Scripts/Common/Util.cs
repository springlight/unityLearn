using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class Util 
{

    public static T AddComponent<T>(GameObject go)where T:Component
    {
        if(go != null)
        {
            T[] components = go.GetComponents<T>();
            for(int i =0; i < components.Length; i++)
            {
                if(components[i] != null)
                {
                    SafeDestroy(components[i].gameObject);
                }
            }
            return go.AddComponent<T>();
        }
        return null;
    }

    public static T AddComponent<T>(Transform trans)where T : Component
    {
        return AddComponent<T>(trans.gameObject);
    }
    public static Uri AppContentDataUri
    {
        get
        {
            string dataPath = Application.dataPath;

            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return new UriBuilder
                {
                    Scheme = "file",
                    Path = Path.Combine(dataPath, "Raw/")
                }.Uri;
            }
            if(Application.platform == RuntimePlatform.Android)
            {
                return new Uri("jar:file//" + dataPath + "!/assets/");
            }
            //编辑器模式
            return new UriBuilder
            {
                Scheme = "file",
                Path = Path.Combine(dataPath, "StreamAssets/")
            }.Uri;
        }
    }

    public static Uri AppPersistentDataUri
    {
        get
        {
            return new UriBuilder
            {
                Scheme = "file",
                Path = Application.persistentDataPath + "/"
            }.Uri;
        }
    }

    public static void SafeDestroy(GameObject go)
    {
        if(go != null)
        {
            UnityEngine.Object.DestroyImmediate(go);
        }
    }

    public static void SafeDestroy(Transform trans)
    {
        if(trans != null)
        {
            SafeDestroy(trans.gameObject);
        }
    }

    public static void ClearMemory()
    {
        GC.Collect();
        //清空资源
        Resources.UnloadUnusedAssets();
    }
}
