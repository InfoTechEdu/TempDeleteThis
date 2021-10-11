using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class PressKeyboardKeyEvent : UnityEvent<KeyCode> { }

[Serializable]
public class PressKeyboardNumberEvent : UnityEvent<int> { }

public class InputManager : MonoBehaviourManager<InputManager>
{
    public PressKeyboardNumberEvent onPressKeyboardNumber;
    public PressKeyboardKeyEvent onPressKeyboardKey;

    protected void Start()
    {
        Keyboard.current.onTextInput += OnPressKeyboard;
        //
        isStarted = true;
    }

    public void OnPressKeyboard(char keyChar)
    {
        if (char.IsDigit(keyChar))
        {
            onPressKeyboardNumber?.Invoke(int.Parse(keyChar.ToString()));
        }
        if (Keyboard.current.enterKey.isPressed)
        {
            onPressKeyboardKey?.Invoke(KeyCode.KeypadEnter);
        }
        if (Keyboard.current.backspaceKey.isPressed)
        {
            onPressKeyboardKey?.Invoke(KeyCode.Backspace);
        }
    }

    public void OnPressStringKey(string stringKey)
    {
        if (char.IsDigit(stringKey[0]))
        {
            onPressKeyboardNumber?.Invoke(int.Parse(stringKey.ToString()));
        }
            
        if(stringKey == "enter")
        {
            onPressKeyboardKey?.Invoke(KeyCode.KeypadEnter);
        }
        if(stringKey == "clean")
        {
            onPressKeyboardKey?.Invoke(KeyCode.Backspace);
        }
    }
}
