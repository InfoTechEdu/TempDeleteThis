using UnityEngine;

public class ScreenKeyboardController : Controller
{
    [SerializeField] private GameObject parent;

    private ScreenKeyboardButton[] buttons;

    private void GetAllButtons()
    {
        buttons = parent.GetComponentsInChildren<ScreenKeyboardButton>();
    }

    private void OnPress(string key)
    {
        InputManager.Instance.OnPressStringKey(key);
    }

    protected override void OnInitialize()
    {
        GetAllButtons();

        foreach (var button in buttons)
        {
            button.onPress += OnPress;
        }
    }

    protected override void OnActivate()
    {

    }

    protected override void OnDiactivate()
    {
        foreach (var button in buttons)
        {
            button.onPress -= OnPress;
        }
    }
}
