using UnityEngine;
using UnityEngine.InputSystem;

public class C_InputController : MonoBehaviour
{
    [SerializeField] public InputActionAsset _inputActions;
    [SerializeField] public string activeActionMap;

    public InputAction m_left;
    public InputAction m_right;
    public InputAction m_jump;
    public InputAction m_down;
    public InputAction m_up;
    public InputAction m_sprint;
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

    private void Reset()
    {
        _inputActions = GetInputActionAsset();
    }

    private void OnEnable()
    {
        _inputActions.FindActionMap("Character").Enable();
    }

    private void OnDisable()
    {
        _inputActions.FindActionMap("Character").Disable();
    }

    private void Awake()
    {
        SetActions("Character");
    }

    public void SetActions(string actionMap)
    {
        m_left = _inputActions.FindActionMap(actionMap).FindAction("Left");
        m_right = _inputActions.FindActionMap(actionMap).FindAction("Right");
        m_jump = _inputActions.FindActionMap(actionMap).FindAction("Jump");
        m_down = _inputActions.FindActionMap(actionMap).FindAction("Down");
        m_up = _inputActions.FindActionMap(actionMap).FindAction("Up");
        m_sprint = _inputActions.FindActionMap(actionMap).FindAction("Sprint");
        lmbAction = _inputActions.FindActionMap(actionMap).FindAction("LMBAction");
        rmbAction = _inputActions.FindActionMap(actionMap).FindAction("RMBAction");
        restart = _inputActions.FindActionMap(actionMap).FindAction("Restart");
        inventory = _inputActions.FindActionMap(actionMap).FindAction("Inventory");
        qab_1 = _inputActions.FindActionMap(actionMap).FindAction("QAB-1");
        qab_2 = _inputActions.FindActionMap(actionMap).FindAction("QAB-2");
        qab_3 = _inputActions.FindActionMap(actionMap).FindAction("QAB-3");
        qab_4 = _inputActions.FindActionMap(actionMap).FindAction("QAB-4");
        qab_5 = _inputActions.FindActionMap(actionMap).FindAction("QAB-5");
        qab_6 = _inputActions.FindActionMap(actionMap).FindAction("QAB-6");
        activeActionMap = actionMap;
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }
}
