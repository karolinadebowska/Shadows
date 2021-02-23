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
    [Range(0, 12)] //from 6am till 9pm
    public float stopTime = 6;
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
        if ((int)timeOfDay == stopTime)
        {
            Debug.Log("if timeOfDay" + timeOfDay);
            SunUpdate();
            return;
        }
        else if ((int)timeOfDay > stopTime)
        {
            Debug.Log("else if timeOfDay" + timeOfDay);
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
    public void setTime(float x) {
        timeOfDay = x;
        Debug.Log("i'm here "+x);
    }
    public void SunUpdate()
    {
        sun.transform.localRotation = Quaternion.Euler((((timeOfDay / numHours) * degr))/2 - 90, 90, 0);
        Quaternion.Euler(((timeOfDay / 15) * 225f) - 90, 90, 0);
        sun.transform.localPosition = new Vector3((timeOfDay / numHours * 0.4f)-0.1f, 0.4f/2 * (float)Math.Sin((float)((timeOfDay / numHours) * radians)), 0.1f);
    }
}