using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureTestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ProcedureMgr procedureMgr = FrameWorkEntry.Ins.GetMgr<ProcedureMgr>();

        ProcedureStart entrance = new ProcedureStart();
        procedureMgr.AddProcedure(entrance);
        procedureMgr.SetEntranceProcedure(entrance);

        procedureMgr.AddProcedure(new ProcedurePlay());
        procedureMgr.AddProcedure(new ProcedureOver());

        procedureMgr.CreateProceduresFsm();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
