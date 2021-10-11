using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderWithLabel : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Slider slider;

    public string labelText
    {
        get
        {
            return label.text;
        }
        set
        {
            label.text = value;
        }
    }

    public string ValueText
    {
        get
        {
            return valueText.text;
        }
        set
        {
            valueText.text = value;
        }
    }

    public float SliderValue
    {
        get
        {
            return slider.value;
        }
        set
        {
            slider.value = value;
        }
    }
}
