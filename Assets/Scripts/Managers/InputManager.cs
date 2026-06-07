using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputResolver))]
public class InputManager : Singleton<InputManager>
{
    [Header("Components")]
    [SerializeField] private InputResolver inputResolver;

    [Header("Resources")]
    [SerializeField] private InputActionAsset inputActionAsset;

    [Header("Global Actions")]
    public InputAction pause;
    public InputAction restart;

    public List<Action> onPauseMenu = new();
    public List<Action> onRestart = new();

    [Header("Character UI")]
    public InputAction inventory;
    public List<Action> onInventory = new();

    [Header("QAB")]
    public InputAction qab_1;
    public InputAction qab_2;
    public InputAction qab_3;
    public InputAction qab_4;
    public InputAction qab_5;
    public InputAction qab_6;

    public List<Action<int>> onQab1 = new();
    public List<Action<int>> onQab2 = new();
    public List<Action<int>> onQab3 = new();
    public List<Action<int>> onQab4 = new();
    public List<Action<int>> onQab5 = new();
    public List<Action<int>> onQab6 = new();


    [Header("Character Actions")]
    public InputAction left;
    public InputAction right;
    public InputAction jump;
    public InputAction down;
    public InputAction up;
    public InputAction shift;

    public List<Action> onLeft = new();
    public List<Action> onLeftHold = new();
    public List<Action> onLeftRelease = new();
    public List<Action> onRight = new();
    public List<Action> onRightHold = new();
    public List<Action> onRightRelease = new();
    public List<Action> onJump = new();
    public List<Action> onJumpHold = new();
    public List<Action> onJumpRelease = new();
    public List<Action> onDown = new();
    public List<Action> onDownHold = new();
    public List<Action> onDownRelease = new();
    public List<Action> onUp = new();
    public List<Action> onUpHold = new();
    public List<Action> onUpRelease = new();
    public List<Action> onShift = new();
    public List<Action> onShiftHold = new();
    public List<Action> onShiftRelease = new();

    [Header("Mouse Actions")]
    public InputAction lmbAction;
    public InputAction rmbAction;

    public List<Action> onLMB = new();
    public List<Action> onRMB = new();
    public List<Action> onLMBHold = new();
    public List<Action> onRMBHold = new();
    public List<Action> onLMBRelease = new();
    public List<Action> onRMBRelease = new();

    private void Reset()
    {
        inputActionAsset = GetInputActionAsset();
        inputResolver = GetComponent<InputResolver>();
    }

    protected override void Awake()
    {
        base.Awake();
        SetActions();
        EnableActions();
    }

    private void Update()
    {
        inputResolver.ResolveInput(Instance);
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }

    private void SetActions()
    {
        //GLOBAL ACTIONS
        pause = inputActionAsset.FindActionMap("GlobalActions").FindAction("Pause");
        restart = inputActionAsset.FindActionMap("GlobalActions").FindAction("Restart");

        //CHARACTER UI
        inventory = inputActionAsset.FindActionMap("CharacterUI").FindAction("Inventory");

        //QAB
        qab_1 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-1");
        qab_2 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-2");
        qab_3 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-3");
        qab_4 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-4");
        qab_5 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-5");
        qab_6 = inputActionAsset.FindActionMap("QAB").FindAction("QAB-6");

        //CHARACTER ACTIONS
        left = inputActionAsset.FindActionMap("Character").FindAction("Left");
        right = inputActionAsset.FindActionMap("Character").FindAction("Right");
        jump = inputActionAsset.FindActionMap("Character").FindAction("Jump");
        down = inputActionAsset.FindActionMap("Character").FindAction("Down");
        up = inputActionAsset.FindActionMap("Character").FindAction("Up");
        shift = inputActionAsset.FindActionMap("Character").FindAction("Sprint");

        //MOUSE ACTIONS
        lmbAction = inputActionAsset.FindActionMap("MouseActions").FindAction("LMBAction");
        rmbAction = inputActionAsset.FindActionMap("MouseActions").FindAction("RMBAction");
    }
    

    private void EnableActions()
    {
        //GLOBAL ACTIONS
        pause.Enable();
        restart.Enable();

        //CHARACTER UI
        inventory.Enable();

        //QAB
        qab_1.Enable();
        qab_2.Enable();
        qab_3.Enable();
        qab_4.Enable();
        qab_5.Enable();
        qab_6.Enable();

        //CHARACTER ACTIONS
        left.Enable();
        right.Enable();
        jump.Enable();
        down.Enable();
        up.Enable();
        shift.Enable();

        //MOUSE ACTIONS
        lmbAction.Enable();
        rmbAction.Enable();
    }

    public void TurnOnInputMap(string mapName)
    {
        var inputMap = inputActionAsset.FindActionMap(mapName);
        if (inputMap == null) return;
        inputMap.Enable();
    }

    public void TurnOffInputMap(string mapName)
    {
        var inputMap = inputActionAsset.FindActionMap(mapName);
        if (inputMap == null) return;
        inputMap.Disable();
    }
}
