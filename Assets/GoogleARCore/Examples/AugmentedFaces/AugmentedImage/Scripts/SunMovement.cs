using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class SunMovement : MonoBehaviour
{
    [HideInInspector]
    public static GameObject sun;
    [HideInInspector]
    public Light sunLight;
    [HideInInspector]
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
    private int count, countStart = 0;
    public static RadialSlider mainSlider;
    public static GameObject cat;
    [HideInInspector]
    private float yValue, zValue;

    public static bool _drawLines = false;
    public static bool DrawLines
    {
        get
        {return _drawLines; }
        set
        {_drawLines = value;}
    }
    private static float _timeOfDay;
    public static float TimeOfDay
    {
        get
        {
            // Reads are usually simple
            return _timeOfDay;
        }
        set
        {
            // You can add logic here for race conditions,
            // or other measurements
            _timeOfDay = value;
        }
    }

    private static float _stopTime;
    public static float StopTime
    {
        get
        {
            // Reads are usually simple
            return _stopTime;
        }
        set
        {
            // You can add logic here for race conditions,
            // or other measurements
            _stopTime = value;
        }
    }

    void Awake()
    {
        Debug.Log("AWAKE IN SUN");
        if (GameObject.FindGameObjectWithTag("myRadialSlider"))
        {
            mainSlider = (RadialSlider)GameObject.FindObjectOfType(typeof(RadialSlider));
        }
        if (GameObject.FindGameObjectWithTag("cat"))
        {
            cat = (GameObject)FindObjectOfType(typeof(GameObject));
        }
        //initialize the position of the sun
        if (count == 0)
        {
            StopTime = 0;
        }
        StopTime = mainSlider.Value * numHours;
        TimeOfDay = mainSlider.Value * numHours;
       // Debug.Log("stop time is" + StopTime+" value from slider "+ mainSlider.Value * numHours);
        count = count + 1;
    }
    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        lr.material = new Material(material);
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = 0.002f;
        lr.endWidth = 0.002f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Destroy(myLine, Time.deltaTime);
    }

    void Start()
    {
        sun = gameObject;
        sunLight = gameObject.GetComponent<Light>();
        //get the initial position of the sun; in AR there are different values each time 
        if (countStart == 0)
        {
            yValue = sun.transform.position.y;
            zValue = sun.transform.position.z;
        }

        //time settings
        secondsPerHour = secondsPerMinute * 60;
        secondsPerDay = secondsPerHour * 12;

        countStart = countStart + 1;
    }
    public void ValueChanged(int value) {
        //start from the last position
        TimeOfDay = StopTime;
        StopTime = value/ 30;
    }

    public void randomPosition(int random) {
        mainSlider.UpdateRadialImage((float)random/12);
        TimeOfDay = random;
        StopTime = random;
        Update();
    }

    public void onClick() {
        DrawLines =! DrawLines;
    }
    // Update is called once per frame
    void Update()
    {
       // Debug.Log("position: " + sun.transform.position);
           // Debug.Log("stopTime in update: " + StopTime+" timeOfDay: "+TimeOfDay);
            if (Math.Floor((double)TimeOfDay) == Math.Floor((double)StopTime))
            {
                SunUpdate();
                return;
            }
            else if ((int)TimeOfDay > StopTime)
            {
                return;
            }
            else
            {
                SunUpdate();

                if (Math.Floor((double)TimeOfDay) >= 12)
                {
                    TimeOfDay = 0;
                }
                TimeOfDay += (Time.deltaTime / secondsPerDay) * timeMultiplier;
            }
        //}
    }

    public void SunUpdate()
    {
        Debug.Log("sun: " + sun + "TimeOfDay: " + TimeOfDay + "degr: " + degr + "numHours: " + numHours);
        sun.transform.localRotation = Quaternion.Euler((((TimeOfDay / numHours) * degr))/2 - 90, 90, 0);
        sun.transform.localPosition = new Vector3((TimeOfDay / numHours * 0.4f)-0.1f, 0.4f/2 * (float)Math.Sin((float)((TimeOfDay / numHours) * radians)), 0.1f);
        if (DrawLines)
        {
            //Debug.Log("drawing");
            //vertical
            DrawLine(new Vector3(sun.transform.position.x, yValue, zValue), sun.transform.position);
            //cat - sun
            DrawLine(cat.transform.position, sun.transform.position);
            //vertical - cat
            DrawLine(cat.transform.position, new Vector3(sun.transform.position.x, yValue, zValue));
        }
    }
}