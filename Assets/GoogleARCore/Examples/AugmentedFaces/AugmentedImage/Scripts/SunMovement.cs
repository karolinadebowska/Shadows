using UnityEngine;
using System;

public class SunMovement : MonoBehaviour
{
    [HideInInspector]
    public static GameObject sun;
    [HideInInspector]
    public Light sunLight;

    [Range(0, 12)] //from 6am till 9pm
    public float timeOfDay = 0;
    [HideInInspector]
    public float stopTime = -1;
    public float secondsPerMinute = 60;
    [HideInInspector]
    public float secondsPerHour;
    [HideInInspector]
    public float secondsPerDay;

    public float timeMultiplier = 1;
    [HideInInspector]
    public static float numHours = 12;
    [HideInInspector]
    public float radius = 0.01f;
    [HideInInspector]
    public static float degr = 15 * numHours;
    [HideInInspector]
    //public static bool readyToRender = false;
    // converting value to radians 
    public static double radians = 15 * numHours * (Math.PI) / 180;

    public void setStopTime(int x)
    {
        stopTime = x;
        Debug.Log("setStopTime" + x);
    }
    void Start()
    {
        sun = gameObject;
        sunLight = gameObject.GetComponent<Light>();
        secondsPerHour = secondsPerMinute * 60;
        secondsPerDay = secondsPerHour * 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTime != -1)
        {
            Debug.Log("inside update, stopTime =" + stopTime);
            if (Math.Floor((double)timeOfDay) == stopTime)
            {
                SunUpdate();
                return;
            }
            else if ((int)timeOfDay > stopTime)
            {
                return;
            }
            else
            {
                SunUpdate();
                timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;

                if (timeOfDay >= 12)
                {
                    timeOfDay = 0;
                }
            }
        }
    }
    public void setTime(float x) {
        int time = (int)Math.Floor(x);
        timeOfDay = 0;
        setStopTime(time-6);
        Debug.Log("time set to " + time+" index: "+(time-6));
    }
    public void SunUpdate()
    {
        sun.transform.localRotation = Quaternion.Euler((((timeOfDay / numHours) * degr))/2 - 90, 90, 0);
        sun.transform.localPosition = new Vector3((timeOfDay / numHours * 0.4f)-0.1f, 0.4f/2 * (float)Math.Sin((float)((timeOfDay / numHours) * radians)), 0.1f);
    }
}