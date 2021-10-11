using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourManager<GameManager>
{
    private ControllersStorage currentControllersStorage;

    public void QuitGame()
    {
        DiactivateControllerOnScene();

        Application.Quit();
    }

    protected void Start()
    {
        if (!isStarted)
        {
            SetCurrentControllersStorage(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            gameDataStorage.Initialize();
        }

        isStarted = true;

        StartScene();
    }

    private void SetCurrentControllersStorage(Scene scene, LoadSceneMode loadSceneMode)
    {
        currentControllersStorage = FindObjectOfType<ControllersStorage>();

        currentControllersStorage.SetGameControllers();

        SceneManager.sceneLoaded -= SetCurrentControllersStorage;
    }

    private void OnChangeScene(Scene firstScene, Scene secondScene)
    {
        if (firstScene != secondScene)
            DiactivateControllerOnScene();

        SetCurrentControllersStorage(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        StartScene();
    }

    private void StartScene()
    {
        InitializeControllersOnScene();

        ActivateControllersOnScene();

        SceneManager.activeSceneChanged += OnChangeScene;
    }

    private void InitializeControllersOnScene()
    {
        currentControllersStorage.InitializeGameControllers(gameDataStorage);
    }

    private void ActivateControllersOnScene()
    {
        Debug.Log(currentControllersStorage + " is started");

        currentControllersStorage.ActivateGameControllers();
    }

    private void DiactivateControllerOnScene()
    {
        currentControllersStorage.DiactivateGameControllers();
    }
}