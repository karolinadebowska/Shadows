using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class SunMovement : MonoBehaviour
{
    [HideInInspector]
    public static GameObject sun;
    [HideInInspector]
    public Light sunLight;

    [Range(0, 12)] //from 6am till 9pm
    public float timeOfDay = 0;
    [HideInInspector]
    public float stopTime;
    public float secondsPerMinute = 60;
    [HideInInspector]
    public float secondsPerHour;
    [HideInInspector]
    public float secondsPerDay;

    public Material material; 

    public float timeMultiplier = 1;
    [HideInInspector]
    public static float numHours = 12;
    [HideInInspector]
    public float radius = 0.01f;
    [HideInInspector]
    public static float degr = 15 * numHours;
    // converting value to radians 
    public static double radians = 15 * numHours * (Math.PI) / 180;
    private int count = 0;
    public static Slider mainSlider;
    public static GameObject cat;
    [HideInInspector]
    private float yValue, zValue, xValue;

    List<int> storedTimeOfDays = new List<int>();

    void Awake()
    {
        Debug.Log("AWAKE IN SUN");
        if (GameObject.FindGameObjectWithTag("myRadialSlider"))
        {
            mainSlider = (Slider)GameObject.FindObjectOfType(typeof(Slider));
        }
        if (GameObject.FindGameObjectWithTag("cat"))
        {
            cat = (GameObject)FindObjectOfType(typeof(GameObject));
        }
        stopTime = 0;
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.material = new Material(material);
        lr.startColor = Color.gray;
        lr.endColor = Color.gray;
        lr.startWidth = 0.002f;
        lr.endWidth = 0.002f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, Time.deltaTime);
    }

    void Start()
    {
        //mainSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        sun = gameObject;
        sunLight = gameObject.GetComponent<Light>();
        //get initial values; in AR there are different values each time 
        if (count == 0)
        {
            xValue = sun.transform.position.x;
            yValue = sun.transform.position.y;
            zValue = sun.transform.position.z;
        }
        DrawLine(new Vector3(xValue, yValue, zValue), sun.transform.position);
        secondsPerHour = secondsPerMinute * 60;
        secondsPerDay = secondsPerHour * 12;
        count = count + 1;
    }

    public void ValueChanged(int value) {
        //Debug.Log("value from a slider:"+mainSlider.value);
        stopTime = (int)Math.Floor((double) value) / 30;
        Debug.Log("value here :"+ value);
        timeOfDay = stopTime;
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("position: " + sun.transform.position);
           // Debug.Log("stopTime in update: " + stopTime+" timeOfDay: "+timeOfDay+ " Math.Floor((double)timeOfDay): "+ Math.Floor((double)timeOfDay));
           // Debug.Log("inside update, stopTime =" + stopTime);
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

                if (Math.Floor((double)timeOfDay) >= 12)
                {
                    timeOfDay = 0;
                }
                timeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;
            }
        //}
    }

    public void SunUpdate()
    {
        sun.transform.localRotation = Quaternion.Euler((((timeOfDay / numHours) * degr))/2 - 90, 90, 0);
        sun.transform.localPosition = new Vector3((timeOfDay / numHours * 0.4f)-0.1f, 0.4f/2 * (float)Math.Sin((float)((timeOfDay / numHours) * radians)), 0.1f);
        DrawLine(new Vector3(sun.transform.position.x, yValue, zValue), sun.transform.position);
        DrawLine(cat.transform.position, sun.transform.position);
        DrawLine(cat.transform.position, new Vector3(sun.transform.position.x, yValue, zValue));
    }
}