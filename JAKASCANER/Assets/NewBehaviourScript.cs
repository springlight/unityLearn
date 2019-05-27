using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public Button btn;
    private AndroidJavaObject jo;
    // Use this for initialization
    void Start()
    {
        //获取Android的Java接口  
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        btn.onClick.AddListener(OnClickBtn);
    }

    public void OnClickBtn()
    {
        jo.Call("StartCan");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
