public class GameControllersStorage : ControllersStorage
{
    public GameLogicController gameLogicController;
    public BezierTranslateController bezierTranslateController;
    public GameUIController gameUIController;
    public OperationCardsController operationCardsController;
    public ScreenKeyboardController screenKeyboardController;

    public override void SetGameControllers()
    {
        controllers = new System.Collections.Generic.List<IController>
        {
            gameLogicController,
            bezierTranslateController,
            gameUIController,
            operationCardsController,
            screenKeyboardController
        };
    }
}
