using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数据节点
/// 将任意类型的数据以树状机构形式进行保存
/// 用于管理游戏运行时的各种数据
/// </summary>
public class DataNode
{
    public static readonly DataNode[] s_EmptyArray = new DataNode[] { };
    public static readonly string[] s_PathSplit = new string[] { ".", "/", "\\" };

    public string Name { get; private set; }

    public DataNode Parent { get; private set; }
    /// <summary>
    /// 结点全名
    /// </summary>
    public string FullName
    {
        get
        {
            return Parent == null ? Name : string.Format("{0}{1}{2}", Parent.FullName, s_PathSplit[0], Name);
        }
    }
    /// <summary>
    /// 结点数据
    /// </summary>
    private object m_Data;

    public List<DataNode> m_Childs;

    public int ChildCount
    {
        get
        {
            return m_Childs != null ? m_Childs.Count : 0;
        }
    }


    private static bool IsValidName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        foreach(string pathSplit in s_PathSplit)
        {
            if (name.Contains(pathSplit)) return false;
        }
        return true;
    }

    public DataNode(string name,DataNode parent)
    {
        if (!IsValidName(name))
        {

        }
        Name = name;
        m_Data = null;
        Parent = parent;
        m_Childs = null;
    }


    public T GetData<T>()
    {
        return (T)m_Data;
    }
	

    public void SetData(object data)
    {
        m_Data = data;
    }
    /// <summary>
    /// 根据索引获取子数据结点
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public DataNode GetChild(int idx)
    {
        return idx >= ChildCount ? null : m_Childs[idx];
    }


    public DataNode GetChild(string name)
    {
        if (!IsValidName(name)) return null;
        if (m_Childs == null) return null;
        foreach(DataNode child in m_Childs)
        {
            if (child.Name.Equals(name))
            {
                return child;
            }
        }
        return null;
    }

    public DataNode GetOrAddChild(string name)
    {
        DataNode node = GetChild(name);
        if (node != null) return node;

        node = new DataNode(name, this);
        if(m_Childs == null)
        {
            m_Childs = new List<DataNode>();
        }
        m_Childs.Add(node);
        return node;
    }


    public void RemoveChild(int idx)
    {
        DataNode node = GetChild(idx);
        if (node == null) return;
        node.Clear();
        m_Childs.Remove(node);
    }

    public void RemoveChild(string name)
    {
        DataNode node = GetChild(name);
        if (node == null) return;
        node.Clear();
        m_Childs.Remove(node);
    }


    public void Clear()
    {
        m_Data = null;
        if(m_Childs != null)
        {
            foreach(DataNode child in m_Childs)
            {
                child.Clear();
            }
            m_Childs.Clear();
        }
    }
}
