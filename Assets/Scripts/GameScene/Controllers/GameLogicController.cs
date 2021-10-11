using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameResult
{
    Win = 0,
    Loose = 1
}

[Serializable]
public class OnGameEndEvent : UnityEngine.Events.UnityEvent<GameResult> { }
[Serializable]
public class OnTrainingEndEvent : UnityEngine.Events.UnityEvent<int> { }

public class GameLogicController : Controller
{
    public OnGameEndEvent onGameEndEvent;

    [SerializeField] private GameControllersStorage gameControllersStorage;

    [SerializeField] private Expression expression;
    [SerializeField] private Operation[] trainingOperations;
    [SerializeField] private Operation[] mainOperations;
    [Space]
    [SerializeField] private IntegerRange startValueRange;
    [SerializeField] private IntegerRange generatedOperationsValueRange;
    [Space]
    [SerializeField] private int trainingOperationsCount;
    [SerializeField] private int mainOperationsCount;
    [SerializeField] private int rightAnswerScore;
    [SerializeField] private int winRequiredScore;
    [SerializeField] private int maxLevelCount;
    [SerializeField] private int nextLevelRequiredScore;
    [Space]
    [SerializeField] private int globalTimerSeconds;
    [SerializeField] private int operationTimerSeconds;
    [Space]
    [SerializeField] [ReadOnly] private int currentScore;
    [SerializeField] [ReadOnly] private int currentLevelScore;
    [SerializeField] [ReadOnly] private int currentLevelIndex;
    [Space]
    [SerializeField] [ReadOnly] private int currentOperationIndex;
    [SerializeField] [ReadOnly] private int currentRightValue;
    [SerializeField] [ReadOnly] private int currentInputValue;
    [Space]
    [SerializeField] [ReadOnly] private GameStates currentGameState;
    [SerializeField] [ReadOnly] private bool bezierTranslateStatus;
    [SerializeField] [ReadOnly] private bool cardsIsHidingStatus;

    private GameStates lastGameState;

    private Timer globalTimer;
    private Timer operationTimer;

    private GameUIController gameUIController;
    private BezierTranslateController bezierTranslateController;
    private OperationCardsController operationCardsController;


    private void GenerateOperationsBeforeTraining()
    {
        currentOperationIndex = 0;

        int startValue = startValueRange.GetRangomValue();

        trainingOperations = MathematicalOperationsManager.GetArrayOfRandomOperations(trainingOperationsCount, generatedOperationsValueRange.min, generatedOperationsValueRange.max, startValue);

        expression = new Expression(startValue, trainingOperations[0])
        {
            operation = trainingOperations[currentOperationIndex]
        };

        currentRightValue = expression.GetResult();
    }

    private void GenerateOperationsBeforeGame()
    {
        currentOperationIndex = 0;

        operationCardsController.HideAllCards();

        int startValue = startValueRange.GetRangomValue();

        mainOperations = MathematicalOperationsManager.GetArrayOfRandomOperations(mainOperationsCount, generatedOperationsValueRange.min, generatedOperationsValueRange.max, startValue);

        expression = new Expression(startValue, mainOperations[0])
        {
            operation = mainOperations[currentOperationIndex]
        };

        currentRightValue = expression.GetResult();
    }

    private void StartTraining()
    {
        GameStatesManager.SetCurrentState(GameStates.training);

        operationCardsController.SetCardsToPositions(expression, trainingOperations[1], trainingOperations[2]);

        gameUIController.HideEndGamePanel();

        gameUIController.TopPanelTabsChanger.SetTab(TopPanelTabsChanger.Tabs.training);
        gameUIController.TopPanelLabel.text = $"Разминочные раунды : {trainingOperationsCount}";
        gameUIController.CircleSlider.Value = 0;
        gameUIController.CircleSlider.ValueText = "0";
    }

    private void StartGame()
    {
        GameStatesManager.SetCurrentState(GameStates.game);

        currentScore = 0;
        currentLevelScore = 0;

        operationCardsController.SetCardsToPositions(expression, mainOperations[1], mainOperations[2]);

        gameUIController.HideEndGamePanel();

        gameUIController.TopPanelTabsChanger.SetTab(TopPanelTabsChanger.Tabs.main);
        gameUIController.TopPanelLabel.text = "Очки";
        gameUIController.CircleSlider.Value = 0;
        gameUIController.CircleSlider.ValueText = "0";
        gameUIController.LevelSlider.SliderValue = 0;
        gameUIController.LevelSlider.labelText = $"Уровень";
        gameUIController.LevelSlider.ValueText = $"0/{maxLevelCount}";
        gameUIController.TimerSlider.labelText = $"Время";
        gameUIController.TimerSlider.ValueText = globalTimer.timerTime.GetTimeUTCString(false);
    }


    private void ShowNextGameOperation()
    {
        if (currentGameState != GameStates.game)
            return;

        if (!CheckOperation())
        {
            OnWrongAnswer();

            return;
        }

        this.RestartTimer(operationTimer);

        currentOperationIndex++;

        currentScore += rightAnswerScore;
        currentLevelScore += rightAnswerScore;

        UpdateCurrentValuesOnUI();

        expression.startValue = currentRightValue;

        if (mainOperations.Length > currentOperationIndex)
        {
            expression.operation = mainOperations[currentOperationIndex];

            currentRightValue = expression.GetResult();

            if (mainOperations.Length - currentOperationIndex > 2)
                operationCardsController.AddNewOperationCardInSequence(mainOperations[currentOperationIndex + 2]);
            else
                operationCardsController.AddEmptyTransformInSequence();

            if (currentLevelScore >= nextLevelRequiredScore)
            {
                currentLevelIndex++;

                currentLevelScore = 0;
            }

            if (currentScore >= winRequiredScore)
            {
                OnGameEndEvent(GameResult.Win);
            }
            else if (currentLevelIndex >= maxLevelCount)
            {
                OnGameEndEvent(GameResult.Win);
            }
        }
        else
        {
            OnGameEndEvent(GameResult.Win);
        }
    }
        
    private void ShowNextTrainingOperation()
    {
        if (currentGameState != GameStates.training)
            return;

        if (!CheckOperation())
        {
            OnWrongAnswer();

            return;
        }

        currentOperationIndex++;

        UpdateCurrentValuesOnUI();

        expression.startValue = currentRightValue;

        if (trainingOperations.Length > currentOperationIndex)
        {
            expression.operation = trainingOperations[currentOperationIndex];

            currentRightValue = expression.GetResult();

            if (trainingOperations.Length - currentOperationIndex > 2)
                operationCardsController.AddNewOperationCardInSequence(trainingOperations[currentOperationIndex + 2]);
            else
                operationCardsController.AddEmptyTransformInSequence();
        }
        else
        {
            OnTrainingEndEvent(currentOperationIndex);
        }
    }

    private void UpdateCurrentValuesOnUI()
    {
        switch (currentGameState)
        {
            case
                GameStates.game:
                gameUIController.CircleSlider.Value = (float)currentScore / winRequiredScore;
                gameUIController.CircleSlider.ValueText = currentScore.ToString();
                gameUIController.LevelSlider.ValueText = $"{currentLevelIndex}/{maxLevelCount}";
                gameUIController.LevelSlider.SliderValue = currentLevelScore > 0 ? (float)currentLevelScore / maxLevelCount : 0;
                break;
            case GameStates.training:
                gameUIController.TopPanelLabel.text = $"Разминочные раунды : {trainingOperationsCount - currentOperationIndex}";
                gameUIController.CircleSlider.Value = (float)currentOperationIndex / trainingOperationsCount;
                gameUIController.CircleSlider.ValueText = currentOperationIndex.ToString();
                break;
        }
    }

    private void ClearInputValue()
    {
        currentInputValue = 0;

        gameUIController.InputValueText.text = "";
    }

    private bool CheckOperation()
    {
        return currentInputValue == currentRightValue ? true : false;
    }


    public void OnPressKeyboardNumber(int number)
    {
        currentInputValue = int.Parse(currentInputValue.ToString() + number.ToString());

        gameUIController.InputValueText.text = currentInputValue.ToString();
    }

    public void OnPressKeyboardKey(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.KeypadEnter:
                switch (currentGameState)
                {
                    case GameStates.game:
                        ShowNextGameOperation();
                        break;
                    case GameStates.training:
                        ShowNextTrainingOperation();
                        break;
                }
                break;
            case KeyCode.Backspace:
                ClearInputValue();
                break;
        }

        ClearInputValue();
    }

    private void OnChangeCurrentGameState(GameStates state)
    {
        currentGameState = state;
    }

    private void OnChangeTranslateStatus(bool flag)
    {
        bezierTranslateStatus = flag;

        if (bezierTranslateStatus)
        {
            lastGameState = currentGameState;

            GameStatesManager.SetCurrentState(GameStates.stop);
        }
        else
        {
            this.CallMethodOnTimer(() => GameStatesManager.SetCurrentState(lastGameState), 0.3f);
        }
    }

    private void OnTrainingEndEvent(int completedOperations)
    {
        currentOperationIndex = 0;

        Debug.Log($"You complete training with {completedOperations} completed operations");

        operationCardsController.HideAllCards();

        GenerateOperationsBeforeGame();

        GameStatesManager.SetCurrentState(GameStates.stop);

        this.StartTimer(globalTimer, new TimeUTC(globalTimerSeconds), 1);
        globalTimer.onTimerChanged += OnGlobalTimerChange;
        gameUIController.TimerSlider.ValueText = globalTimer.timerTime.GetAllTimeInSeconds().ToString();

        this.StartTimer(operationTimer, new TimeUTC(operationTimerSeconds), .1f);
        operationTimer.onTimerChanged += OnOperationTimerChange;
        gameUIController.TimerSlider.SliderValue = 1;

        this.CallMethodOnTimer(StartGame, 1);
    }

    private void OnGameEndEvent(GameResult gameResult)
    {
        currentOperationIndex = 0;

        this.CallMethodOnTimer(() => GameStatesManager.SetCurrentState(GameStates.end), 1);

        operationCardsController.HideAllCards();

        onGameEndEvent?.Invoke(gameResult);

        globalTimer.onTimerChanged -= OnGlobalTimerChange;
        operationTimer.onTimerChanged -= OnOperationTimerChange;

        this.StopTimer(globalTimer);
        this.StopTimer(operationTimer);

        gameUIController.ShowEndGamePanel(new List<EndGamePanelButtons> { EndGamePanelButtons.ExitButton, EndGamePanelButtons.RestartButton });
    }

    private void OnWrongAnswer()
    {
        if (!(currentGameState == GameStates.game || currentGameState == GameStates.training) || cardsIsHidingStatus)
            return;

        operationCardsController.ShowExpressionOnFocusCard($"{expression.startValue} {expression.operation.GetOperationSymbol()} {expression.operation.value} = {expression.GetResult()}");

        cardsIsHidingStatus = true;

        switch (currentGameState)
        {
            case GameStates.game:
                GameStatesManager.SetCurrentState(GameStates.stop);

                this.CallMethodOnTimer(() =>
                {
                    GenerateOperationsBeforeGame();
                    StartGame();
                }, 2);


                this.StopTimer(operationTimer);
                this.StopTimer(globalTimer);

                this.CallMethodOnTimer(() => 
                {
                    this.ResumeTimer(globalTimer);
                    this.ResumeTimer(operationTimer);
                    cardsIsHidingStatus = false;
                }, 4);


                break;

            case GameStates.training:
                GameStatesManager.SetCurrentState(GameStates.stop);
                this.CallMethodOnTimer(() => OnTrainingEndEvent(currentOperationIndex), 2);
                this.CallMethodOnTimer(() => cardsIsHidingStatus = false, 4);
                break;
        }
    }

    private void OnGlobalTimerChange(float time)
    {
        gameUIController.TimerSlider.ValueText = TimeUTC.GetTimeUTCStringFromTime(time, false);

        if (time <= 0)
            OnGameEndEvent(GameResult.Loose);
    }

    private void OnOperationTimerChange(float time)
    {
        gameUIController.TimerSlider.SliderValue = operationTimer.time / operationTimer.timerTime.GetAllTimeInSeconds();

        if (time <= 0)
            OnGameEndEvent(GameResult.Loose);
    }

    private void OnClickButton(string buttonName)
    {

        switch (buttonName)
        {
            case "restart":
                if (GameStatesManager.CheckCurrentState(GameStates.end))
                {
                    GameStatesManager.SetCurrentState(GameStates.game);

                    StartTraining();

                    Debug.Log("Start new game");
                }
                break;
            case "exit":
                SceneManager.LoadScene(gameDataStorage.menuSceneIndex);
                break;
        }
    }


    protected override void OnInitialize()
    {
        gameUIController = gameControllersStorage.gameUIController;
        bezierTranslateController = gameControllersStorage.bezierTranslateController;
        operationCardsController = gameControllersStorage.operationCardsController;

        gameUIController.onButtonClick.AddListener(OnClickButton);

        InputManager.Instance.onPressKeyboardNumber.AddListener(OnPressKeyboardNumber);
        InputManager.Instance.onPressKeyboardKey.AddListener(OnPressKeyboardKey);

        globalTimer = new Timer();
        operationTimer = new Timer();

        bezierTranslateController.OnChangeTranslateStatus += OnChangeTranslateStatus;
        GameStatesManager.OnChangeCurrentState += OnChangeCurrentGameState;
    }

    protected override void OnActivate()
    {
        GenerateOperationsBeforeTraining();
        StartTraining();
    }

    protected override void OnDiactivate()
    {
        gameUIController.onButtonClick.RemoveListener(OnClickButton);

        InputManager.Instance.onPressKeyboardNumber.RemoveListener(OnPressKeyboardNumber);
        InputManager.Instance.onPressKeyboardKey.RemoveListener(OnPressKeyboardKey);

        this.StopTimer(globalTimer);
        this.StopTimer(operationTimer);

        bezierTranslateController.OnChangeTranslateStatus -= OnChangeTranslateStatus;
        GameStatesManager.OnChangeCurrentState -= OnChangeCurrentGameState;
    }
}
