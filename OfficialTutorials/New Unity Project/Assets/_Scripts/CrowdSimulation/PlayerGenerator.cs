using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject dude;
    public GameObject teddy;
    public int showCount = 0;
    public int maxPlayerCount = 50;
    static int count = 0;
    static float lastTime = 0;
    private float timeSpan = 1f;
	// Use this for initialization
	void Start () {
        lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(count < maxPlayerCount)
        {
            bool fired = Input.GetButton("Fire1");
            if(Time.time - lastTime > timeSpan)
            {
                if (dude != null && !fired)
                    Instantiate(dude, Vector3.zero, Quaternion.identity);
                if (teddy != null && fired)
                    Instantiate(teddy, Vector3.zero, Quaternion.identity);
                lastTime = Time.time;
                ++count;
                showCount = count;
            }
        }
	}
}
