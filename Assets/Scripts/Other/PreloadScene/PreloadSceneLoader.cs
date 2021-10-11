using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadSceneLoader : MonoBehaviour
{
    [SerializeField] private int loadSceneIndex;

    private void Awake()
    {
        SceneManager.LoadScene(loadSceneIndex);
    }
}
