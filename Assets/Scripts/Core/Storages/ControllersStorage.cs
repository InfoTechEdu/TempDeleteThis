using System.Collections.Generic;
using UnityEngine;

public abstract class ControllersStorage : MonoBehaviour
{
    [SerializeField] protected List<IController> controllers;

    public abstract void SetGameControllers();

    public void InitializeGameControllers(GameDataStorage gameDataStorage)
    {
        foreach (var controller in controllers)
        {
            controller.OnInitialize(gameDataStorage);
        }
    }

    public void ActivateGameControllers()
    {
        foreach (var controller in controllers)
        {
            controller.OnActivate();
        }
    }

    public void DiactivateGameControllers()
    {
        foreach (var controller in controllers)
        {
            controller.OnDiactivate();
        }
    }
}
