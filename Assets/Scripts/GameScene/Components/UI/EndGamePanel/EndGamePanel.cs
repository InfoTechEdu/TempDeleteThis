using UnityEngine;
using UnityEngine.UI;

public enum EndGamePanelButtons
{
    ExitButton = 0,
    RestartButton = 1
}

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button RestartButton;

    public void ShowButton(EndGamePanelButtons button)
    {
        switch (button) 
        {
            case EndGamePanelButtons.ExitButton:
                ExitButton.gameObject.SetActive(true);
                break;
            case EndGamePanelButtons.RestartButton:
                RestartButton.gameObject.SetActive(true);
                break;
        }

    }

    public void ShowAllButtons()
    {
        ExitButton.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
    }

    public void HideAllButtons()
    {
        ExitButton.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(false);
    }
}
