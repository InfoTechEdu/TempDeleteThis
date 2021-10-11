using UnityEngine;

public abstract class Controller : MonoBehaviour, IController
{
    protected GameDataStorage gameDataStorage;

    protected abstract void OnInitialize();

    protected abstract void OnActivate();

    protected abstract void OnDiactivate();

    void IController.OnInitialize(GameDataStorage gameDataStorage)
    {
        this.gameDataStorage = gameDataStorage;

        OnInitialize();
    }

    void IController.OnActivate()
    {
        OnActivate();
    }

    void IController.OnDiactivate()
    {
        OnDiactivate();
    }
}
