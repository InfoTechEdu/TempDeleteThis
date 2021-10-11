using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenKeyboardButton : MonoBehaviour
{
    public event Action<string> onPress;

    [SerializeField] private string text;
    [SerializeField] private string key;
    [SerializeField] private TMP_Text buttonText;

    private Button button;

    protected void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);

        buttonText.text = text;
    }

    protected void OnClick()
    {
        onPress?.Invoke(key.Trim().ToLower());
    }
}
