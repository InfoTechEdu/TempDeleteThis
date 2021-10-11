using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIController : UIController
{
    [SerializeField] private TMP_Text topPanelLabel;
    [SerializeField] private TMP_Text inputValueText;
    [SerializeField] private CircleSlider circleSlider;
    [SerializeField] private SliderWithLabel levelSlider;
    [SerializeField] private SliderWithLabel timerSlider;
    [SerializeField] private TopPanelTabsChanger topPanelTabsChanger;
    [SerializeField] private EndGamePanel endGamePanel;

    public TMP_Text TopPanelLabel
    {
        get
        {
            return topPanelLabel;
        }
    }

    public TMP_Text InputValueText
    {
        get
        {
            return inputValueText;
        }
    }

    public CircleSlider CircleSlider
    {
        get
        {
            return circleSlider;
        }
    }

    public SliderWithLabel LevelSlider
    {
        get
        {
            return levelSlider;
        }
    }

    public SliderWithLabel TimerSlider
    {
        get
        {
            return timerSlider;
        }
    }

    public TopPanelTabsChanger TopPanelTabsChanger
    {
        get
        {
            return topPanelTabsChanger;
        }
    }

    public void ShowEndGamePanel(List<EndGamePanelButtons> showingButtons)
    {
        endGamePanel.gameObject.SetActive(true);

        foreach (var showingButton in showingButtons)
        {
            endGamePanel.ShowButton(showingButton);
        }
    }

    public void HideEndGamePanel()
    {
        endGamePanel.HideAllButtons();

        endGamePanel.gameObject.SetActive(false);
    }

    protected override void OnInitialize()
    {

    }

    protected override void OnActivate()
    {
        HideEndGamePanel();
    }

    protected override void OnDiactivate()
    {

    }
}
