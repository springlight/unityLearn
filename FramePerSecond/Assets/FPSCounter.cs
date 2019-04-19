using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    public int AverageFPS { get; set; }
    public int frameRange = 60;
    int[] fpsBuffer;
    int fpsBufIdx;
	// Use this for initialization
	void Start () {
		
	}


    void InitializeBuffer()
    {
        if (frameRange <= 0)
        {
            frameRange = 1;
        }
        fpsBuffer = new int[frameRange];
        fpsBufIdx = 0;
    }

    // Update is called once per frame
    void Update () {
        if(fpsBuffer == null || fpsBuffer.Length != frameRange)
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
    }

    void UpdateBuffer()
    {
        fpsBuffer[fpsBufIdx++] = (int)(1f / Time.unscaledDeltaTime);
        if (fpsBufIdx >= frameRange)
        {
            fpsBufIdx = 0;
        }
    }

    void CalculateFPS()
    {
        int sum = 0;
        for (int i = 0; i < frameRange; i++)
        {
            sum += fpsBuffer[i];
        }
        AverageFPS = sum / frameRange;
    }
}
