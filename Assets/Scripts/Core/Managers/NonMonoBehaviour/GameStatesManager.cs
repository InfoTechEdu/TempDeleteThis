using System;

public enum GameStates
{
    preload = 0,
    menu = 1,
    training = 2,
    game = 3,
    stop = 4,
    end = 5
}

public static class GameStatesManager
{
    public static event Action<GameStates> OnChangeCurrentState;

    private static GameStates currentState;

    public static void SetCurrentState(GameStates state)
    {
        currentState = state;

        OnChangeCurrentState?.Invoke(currentState);
    }

    public static bool CheckCurrentState(GameStates state)
    {
        return currentState == state;
    }
}
