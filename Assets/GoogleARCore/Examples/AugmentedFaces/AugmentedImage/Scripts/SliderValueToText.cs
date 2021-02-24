using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private Text textSliderValue;

    void Start()
    {
        textSliderValue = GetComponent<Text>();
    }

    public void ShowSliderValue(float value)
    {
        Debug.Log("slider value: "+value);
        string sliderMessage;
        if (value <= 12)
            sliderMessage = value + ":00 am";
        else
            sliderMessage = (value-12) + ":00 pm";
        textSliderValue.text = sliderMessage;
    }
}