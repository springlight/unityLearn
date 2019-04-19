using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform hourArm;
    public Transform minitueArm;
    public Transform secondArm;

    private float degreesPerHour = 30f;//一周12小时，所以每小时的度数是30度
    private float degreesPerMinute = 6f;//一小时60分，所以每分钟度数是6
    private float degreesPerSec = 6f;//一分钟60秒,所以没秒度数是6
    public bool continuous;//秒针是否平滑过渡，还是一秒一秒的旋转
    private DateTime date;
  
    // Update is called once per frame
    void Update()
    {
        if (continuous)
        {
            UpdateContinuous();
        }
        else
        {
            UpdateDiscrete();
        }
    }

    void UpdateContinuous()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hourArm.localRotation =
            Quaternion.Euler(0f, (float)time.TotalHours * degreesPerHour, 0f);
        minitueArm.localRotation =
            Quaternion.Euler(0f, (float)time.TotalMinutes * degreesPerMinute, 0f);
        secondArm.localRotation =
            Quaternion.Euler(0f, (float)time.TotalSeconds * degreesPerSec, 0f);
    }
    void UpdateDiscrete()
    {
        DateTime time = DateTime.Now;
        hourArm.localRotation =
            Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
        minitueArm.localRotation =
            Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        secondArm.localRotation =
            Quaternion.Euler(0f, time.Second * degreesPerSec, 0f);
    }
}



