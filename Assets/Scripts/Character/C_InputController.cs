using System.Collections;
using UnityEngine;
using System;

public class C_InputController : MonoBehaviour, IInputSubscription
{
    [SerializeField] public InputManager inputManager;
    [SerializeField] public QuickAccessBar quickAccessBar;
    [SerializeField] public C_MovementDataCollector movementDataCollector;

    private void OnEnable()
    {
        StartCoroutine(FindInputManager(SubscribeToInputManager));
    }

    private void OnDisable()
    {
        UnsubscribeFromInputManager();
    }

    private IEnumerator FindInputManager(Action callback)
    {
        while(inputManager == null)
        {
            inputManager = InputManager.Instance;
            yield return null;
        }

        callback?.Invoke();
    }
    public void SubscribeToInputManager()
    {
        var im = InputManager.Instance;
        var mdc = movementDataCollector;

        im.onLeftHold.Insert(0, mdc.LeftHold);
        im.onLeftRelease.Insert(0, mdc.LeftRelease);

        im.onRightHold.Insert(0, mdc.RightHold);
        im.onRightRelease.Insert(0, mdc.RightRelease);

        im.onJump.Insert(0, mdc.Jump);
        im.onJumpHold.Insert(0, mdc.JumpHold);
        im.onJumpRelease.Insert(0, mdc.JumpRelease);

        im.onDownHold.Insert(0, mdc.DownHold);
        im.onDownRelease.Insert(0, mdc.DownRelease);

        im.onUpHold.Insert(0, mdc.UpHold);
        im.onUpRelease.Insert(0, mdc.UpRelease);

        im.onShiftHold.Insert(0, mdc.SprintHold);
        im.onShiftRelease.Insert(0, mdc.SprintRelease);


        im.onLMB.Insert(0, quickAccessBar.LMBAction);
        im.onRMB.Insert(0, quickAccessBar.RMBAction);
        im.onLMBHold.Insert(0, quickAccessBar.LMBHoldAction);
        im.onRMBHold.Insert(0, quickAccessBar.RMBHoldAction);
        im.onLMBRelease.Insert(0, quickAccessBar.LMBReleaseAction);
        im.onRMBRelease.Insert(0, quickAccessBar.RMBReleaseAction);
    }

    public void UnsubscribeFromInputManager()
    {
        var im = InputManager.Instance;
        var mdc = movementDataCollector;

        im.onLeft.Remove(mdc.LeftHold);
        im.onLeftRelease.Remove(mdc.LeftRelease);

        im.onRight.Remove(mdc.RightHold);
        im.onRightRelease.Remove(mdc.RightRelease);

        im.onJump.Remove(mdc.Jump);
        im.onJumpHold.Remove(mdc.JumpHold);
        im.onJumpRelease.Remove(mdc.JumpRelease);

        im.onDown.Remove(mdc.DownHold);
        im.onDownRelease.Remove(mdc.DownRelease);

        im.onUp.Remove(mdc.UpHold);
        im.onUpRelease.Remove(mdc.UpRelease);

        im.onShift.Remove(mdc.SprintHold);
        im.onShiftRelease.Remove(mdc.SprintRelease);


        im.onLMB.Remove(quickAccessBar.LMBAction);
        im.onRMB.Remove(quickAccessBar.RMBAction);
        im.onLMBHold.Remove(quickAccessBar.LMBHoldAction);
        im.onRMBHold.Remove(quickAccessBar.RMBHoldAction);
        im.onLMBRelease.Remove(quickAccessBar.LMBReleaseAction);
        im.onRMBRelease.Remove(quickAccessBar.RMBReleaseAction);
    }



































    //[SerializeField] public InputActionAsset _inputActions;
    //[SerializeField] public string activeActionMap;

    //public InputAction m_left;
    //public InputAction m_right;
    //public InputAction m_jump;
    //public InputAction m_down;
    //public InputAction m_up;
    //public InputAction m_sprint;
    //public InputAction lmbAction;
    //public InputAction rmbAction;
    //public InputAction restart;
    //public InputAction inventory;
    //public InputAction qab_1;
    //public InputAction qab_2;
    //public InputAction qab_3;
    //public InputAction qab_4;
    //public InputAction qab_5;
    //public InputAction qab_6;

    //private void Reset()
    //{
    //    _inputActions = GetInputActionAsset();
    //}

    //private void OnEnable()
    //{
    //    _inputActions.FindActionMap("Character").Enable();
    //}

    //private void OnDisable()
    //{
    //    _inputActions.FindActionMap("Character").Disable();
    //}

    //private void Awake()
    //{
    //    SetActions("Character");
    //}

    //public void SetActions(string actionMap)
    //{
    //    m_left = _inputActions.FindActionMap(actionMap).FindAction("Left");
    //    m_right = _inputActions.FindActionMap(actionMap).FindAction("Right");
    //    m_jump = _inputActions.FindActionMap(actionMap).FindAction("Jump");
    //    m_down = _inputActions.FindActionMap(actionMap).FindAction("Down");
    //    m_up = _inputActions.FindActionMap(actionMap).FindAction("Up");
    //    m_sprint = _inputActions.FindActionMap(actionMap).FindAction("Sprint");
    //    lmbAction = _inputActions.FindActionMap(actionMap).FindAction("LMBAction");
    //    rmbAction = _inputActions.FindActionMap(actionMap).FindAction("RMBAction");
    //    restart = _inputActions.FindActionMap(actionMap).FindAction("Restart");
    //    inventory = _inputActions.FindActionMap(actionMap).FindAction("Inventory");
    //    qab_1 = _inputActions.FindActionMap(actionMap).FindAction("QAB-1");
    //    qab_2 = _inputActions.FindActionMap(actionMap).FindAction("QAB-2");
    //    qab_3 = _inputActions.FindActionMap(actionMap).FindAction("QAB-3");
    //    qab_4 = _inputActions.FindActionMap(actionMap).FindAction("QAB-4");
    //    qab_5 = _inputActions.FindActionMap(actionMap).FindAction("QAB-5");
    //    qab_6 = _inputActions.FindActionMap(actionMap).FindAction("QAB-6");
    //    activeActionMap = actionMap;
    //}

    //private InputActionAsset GetInputActionAsset()
    //{
    //    return Resources.Load("InputSystem_Actions") as InputActionAsset;
    //}
}
