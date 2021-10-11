using UnityEngine;

public class TopPanelTabsChanger : MonoBehaviour
{
    public enum Tabs
    {
        training = 0,
        main = 1
    }

    [SerializeField] private GameObject trainingTab; 
    [SerializeField] private GameObject mainTab; 

    public void SetTab(Tabs tab)
    {
        trainingTab.SetActive(false);
        mainTab.SetActive(false);

        switch (tab)
        {
            case Tabs.training:
                trainingTab.SetActive(true);
                break;
            case Tabs.main:
                mainTab.SetActive(true);
                break;
        }
    }
}
