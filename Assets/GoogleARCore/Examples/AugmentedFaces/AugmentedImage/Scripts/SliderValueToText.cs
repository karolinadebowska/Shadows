using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public GameObject sliderUI;
    private Text textSliderValue;

    void Start()
    {
        textSliderValue = GetComponent<Text>();
    }

    public void ShowSliderValue(int value)
    {
        Debug.Log("Value: " + value);
        string sliderMessage;
        if (value <= 180)
            sliderMessage = "AM";
        else
            sliderMessage = "PM";
        textSliderValue.text = sliderMessage;
    }
}