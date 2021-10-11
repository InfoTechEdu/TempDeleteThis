using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircleSlider : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TMP_Text valueText;

    public float Value
    {
        get
        {
            return fill.fillAmount;
        }
        set
        {
            if (value >= 0)
            {
                fill.fillAmount = value;
            }
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
}
