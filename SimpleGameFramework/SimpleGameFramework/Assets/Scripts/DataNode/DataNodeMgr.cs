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

}
