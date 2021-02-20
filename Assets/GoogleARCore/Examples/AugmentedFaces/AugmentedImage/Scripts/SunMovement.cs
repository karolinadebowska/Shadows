using UnityEngine;
using System;

public class SunMovement : MonoBehaviour
{
    [HideInInspector]
    public GameObject sun;
    [HideInInspector]
    public Light sunLight;

    [Range(0, 15)] //from 6am till 9pm
    public float timeOfDay = 0;
    [HideInInspector]
    public float degToRadConv = 0.0174532925f;
    public float secondsPerMinute = 60;
    [HideInInspector]
    public float secondsPerHour;
    [HideInInspector]
    public float secondsPerDay;

    public float timeMultiplier = 1;

    void Start()
    {
        Debug.Log("start");
        sun = gameObject;
        sunLight = gameObject.GetComponent<Light>();
        secondsPerHour = secondsPerMinute * 60;
        secondsPerDay = secondsPerHour * 12;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
        SunUpdate();
        timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;

        if (timeOfDay > 15)
        {
            timeOfDay = 0;
        }
    }
    public void setTime(float x) {
        timeOfDay = x;
        Debug.Log("i'm here "+x);
       // SunUpdate(timeOf);
    }

    public void SunUpdate()
    {
        sun.transform.localRotation = Quaternion.Euler(((timeOfDay / 15) * 180f) - 90, 90, 0);
        sun.transform.localPosition = new Vector3((timeOfDay / 15 * 0.3f), 0.05f * (float)Math.Sin((float)((timeOfDay / 15) * 180f * degToRadConv)), 0);
    }
}