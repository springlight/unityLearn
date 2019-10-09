using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FrameWorkEntry.Ins.GetMgr<EventMgr>().Subscribe(1, EventTestMethod);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            EventTestAgrs e = ReferencePool.Acquire<EventTestAgrs>();
            FrameWorkEntry.Ins.GetMgr<EventMgr>().Fire(this, e.Fill("浪子"));
        }
	}

    private void EventTestMethod(object sender,GlobalEventArgs e)
    {
        EventTestAgrs args = e as EventTestAgrs;
        Debug.LogError("参数名字---" + args.m_Name + "/"+ sender.GetType().FullName);
    }
}
