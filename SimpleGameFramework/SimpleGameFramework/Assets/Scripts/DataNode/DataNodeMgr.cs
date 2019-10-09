using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeMgr : MgrBase
{
    private static readonly string[] s_EmptyStringArray = new string[] { };

    public DataNode Root { get; private set; }
    private const string RootName = "<Root>";
    public DataNodeMgr()
    {
        Root = new DataNode(RootName, null);
    }

    public override void Init()
    {
       
    }

    public override void ShutDown()
    {
        Root.Clear();
        Root = null;
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
       
    }
    /// <summary>
    /// 数据路径分割点
    /// 这个方法能够将形如aaa.bbb.ccc的路径分割成{aaa,bbb,ccc}的字符串数组
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string [] GetSplitPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return s_EmptyStringArray;
        return path.Split(DataNode.s_PathSplit, StringSplitOptions.RemoveEmptyEntries);
    }

    public DataNode GetNode(string path,DataNode node = null)
    {
        DataNode cur = (node ?? Root);
        string[] splitPath = GetSplitPath(path);
        foreach(string childName in splitPath)
        {
            cur = cur.GetChild(childName);
            if(cur == null)
            {
                return null;
            }
        }
        return cur;
    }


    public DataNode GetOrAddNode(string path,DataNode node = null)
    {
        DataNode cur = (node ?? Root);
        string[] splithPath = GetSplitPath(path);
        foreach(string childName in splithPath)
        {
            cur = cur.GetOrAddChild(childName);
        }
        return cur;
    }

    /// <summary>
    /// 移除数据结点
    /// </summary>
    /// <param name="path">相对于 node 的查找路径</param>
    /// <param name="node">查找起始结点</param>
    public void RemoveNode(string path, DataNode node = null)
    {
        DataNode current = (node ?? Root);
        DataNode parent = current.Parent;
        string[] splitPath = GetSplitPath(path);
        foreach (string childName in splitPath)
        {
            parent = current;
            current = current.GetChild(childName);
            if (current == null)
            {
                return;
            }
        }

        if (parent != null)
        {
            parent.RemoveChild(current.Name);
        }
    }


    public T GetData<T> (string path,DataNode node = null)
    {
        DataNode cur = GetNode(path, node);
        if(cur == null)
        {
            return default(T);
        }
        return cur.GetData<T>();
    }

    public void SetData(string path,object data,DataNode node = null)
    {
        DataNode cur = GetOrAddNode(path, node);
        cur.SetData(data);
    }

}
