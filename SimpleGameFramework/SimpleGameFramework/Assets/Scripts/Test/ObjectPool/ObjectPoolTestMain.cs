using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolTestMain : MonoBehaviour {
    private ObjectPool<TestObject> m_testPool;
	// Use this for initialization
	void Start () {
        ObjectPoolMgr m_objectPoolMgr = FrameWorkEntry.Ins.GetMgr<ObjectPoolMgr>();
        m_testPool = m_objectPoolMgr.CreateObjectPool<TestObject>();
        TestObject obj = new TestObject("hello objectpool", "test111");
        m_testPool.Register(obj, false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            TestObject testObject = m_testPool.Spawn("test111");
            Debug.Log(testObject.Target);
            m_testPool.Unspawn(testObject.Target);
        }


    }
}
