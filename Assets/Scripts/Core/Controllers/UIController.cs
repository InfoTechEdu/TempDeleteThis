using UnityEngine.Events;

[System.Serializable]
public class ButtonClickEvent : UnityEvent<string> { }

public class UIController : Controller
{
    public ButtonClickEvent onButtonClick;

    public void ClickButton(string buttonName)
    {
        onButtonClick?.Invoke(buttonName.Trim().ToLower());
    }

    protected override void OnInitialize() { }

    protected override void OnActivate() { }

    protected override void OnDiactivate() { }
}
