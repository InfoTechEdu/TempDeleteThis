using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogicController : Controller
{
    [SerializeField] private MenuControllersStorage controllersStorage;

    private void OnButtonClick(string buttonName)
    {
        switch (buttonName)
        {
            case "play":
                SceneManager.LoadScene(gameDataStorage.gameSceneIndex);
                break;
            case "exit":
                Application.Quit();
                break;
        }
    }

    protected override void OnInitialize()
    {
        controllersStorage.menuUIController.onButtonClick.AddListener(OnButtonClick);
    }

    protected override void OnActivate()
    {

    }

    protected override void OnDiactivate()
    {
        controllersStorage.menuUIController.onButtonClick.RemoveListener(OnButtonClick);
    }
}
