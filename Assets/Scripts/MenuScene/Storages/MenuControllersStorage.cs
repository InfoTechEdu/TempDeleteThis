using UnityEngine;

public class MenuControllersStorage : ControllersStorage
{
    public MenuLogicController menuLogicController;
    public MenuUIController menuUIController;

    public override void SetGameControllers()
    {
        controllers = new System.Collections.Generic.List<IController>
        {
            menuLogicController,
            menuUIController
        };
    }
}
