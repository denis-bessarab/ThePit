using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : Singleton<InputManager>
{
    [Header("Components")]
    [SerializeField] private InputActionAsset inputActionAsset;
    public Stack<InputState> stateStack = new();
    private InputState currentState;

    [Header("Actions")]
    public InputAction left;
    public InputAction right;
    public InputAction jump;
    public InputAction down;
    public InputAction up;
    public InputAction shift;
    public InputAction lmbAction;
    public InputAction rmbAction;
    public InputAction restart;
    public InputAction inventory;
    public InputAction qab_1;
    public InputAction qab_2;
    public InputAction qab_3;
    public InputAction qab_4;
    public InputAction qab_5;
    public InputAction qab_6;

    public Action onLeftPress;
    public Action onLeftHold;
    public Action onLeftRelease;
    public Action onRightPress;
    public Action onRightHold;
    public Action onRightRelease;

    public enum InputState
    {
        Empty,
        Character,
        PauseMenu,
        MainMenu,
        Character1
    }

    public InputState CurrentState
    {
        get => currentState;
        set
        {
            currentState = value;
            Debug.Log($"Updating Input State to {value}");
            var s = stateStack.ToArray();
            string str = string.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                str += s[i] + " ";
            }
            Debug.Log(str);
            SwitchActionMap(currentState);
        }
    }

    private void Reset()
    {
        inputActionAsset = GetInputActionAsset();
    }

    private void Start()
    {
        EnterState(InputState.Empty);
    }

    private void Update()
    {
        if (left != null && left.WasPressedThisFrame()) onLeftPress?.Invoke();
        if (right != null && right.WasPressedThisFrame()) onRightPress?.Invoke();

        if (Input.GetKeyDown(KeyCode.V))
        {
            Test1();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Test2();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Test3();
        }
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }

    public void EnterState(InputState state)
    {
        stateStack.Push(state);
        UpdateState();
    }

    public void ExitState()
    {
        if(stateStack.Count > 1)
        {
            stateStack.Pop();
            UpdateState();
        }
    }

    public void UpdateState()
    {
        CurrentState = stateStack.Peek();
    }

    private void SwitchActionMap(InputState state)
    {
        switch (state)
        {
            case InputState.Empty:
                SetActions("Empty");
                break;
            case InputState.Character:
                SetActions("Character");
                break;
            case InputState.PauseMenu:
                SetActions("PauseMenu");
                break;
            case InputState.MainMenu:
                SetActions("MainMenu");
                break;
            case InputState.Character1:
                SetActions("Character1");
                break;
        }
    }

    private void Test1()
    {
        EnterState(InputState.Character1);
    }

    private void Test2()
    {
        EnterState(InputState.Character);
    }

    private void Test3()
    {
        ExitState();
    }

    private void SetActions(string actionMap)
    {
        Debug.Log($"Switch Map to {actionMap}");

        left = inputActionAsset.FindActionMap(actionMap).FindAction("Left");
        right = inputActionAsset.FindActionMap(actionMap).FindAction("Right");
        jump = inputActionAsset.FindActionMap(actionMap).FindAction("Jump");
        down = inputActionAsset.FindActionMap(actionMap).FindAction("Down");
        up = inputActionAsset.FindActionMap(actionMap).FindAction("Up");
        shift = inputActionAsset.FindActionMap(actionMap).FindAction("Sprint");
        lmbAction = inputActionAsset.FindActionMap(actionMap).FindAction("LMBAction");
        rmbAction = inputActionAsset.FindActionMap(actionMap).FindAction("RMBAction");
        restart = inputActionAsset.FindActionMap(actionMap).FindAction("Restart");
        inventory = inputActionAsset.FindActionMap(actionMap).FindAction("Inventory");
        qab_1 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-1");
        qab_2 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-2");
        qab_3 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-3");
        qab_4 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-4");
        qab_5 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-5");
        qab_6 = inputActionAsset.FindActionMap(actionMap).FindAction("QAB-6");
    }
}
