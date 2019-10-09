using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNodeTestMain : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        DataNodeMgr nodeMgr = FrameWorkEntry.Ins.GetMgr<DataNodeMgr>();
        nodeMgr.SetData("Player.Name", "Ellan");
        string playerName = nodeMgr.GetData<string>("Player.Name");

        Debug.LogError("1---------->"+playerName);

        DataNode playerNode = nodeMgr.GetNode("Player");
        nodeMgr.SetData("Level", 99,playerNode);

        int playerLv = nodeMgr.GetData<int>("Level", playerNode);
        Debug.LogError("2---------->" + playerLv);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
