using UnityEngine;
using UnityEngine.InputSystem;

public class C_InputController : MonoBehaviour
{
    [SerializeField] public InputActionAsset _inputActions;

    public InputAction m_left;
    public InputAction m_right;
    public InputAction m_jump;
    public InputAction m_down;
    public InputAction m_up;
    public InputAction m_sprint;
    public InputAction rope;
    public InputAction restart;

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
        SetActions();
    }

    private void SetActions()
    {
        m_left = _inputActions.FindAction("Left");
        m_right = _inputActions.FindAction("Right");
        m_jump = _inputActions.FindAction("Jump");
        m_down = _inputActions.FindAction("Down");
        m_up = _inputActions.FindAction("Up");
        m_sprint = _inputActions.FindAction("Sprint");
        rope = _inputActions.FindAction("Rope");
        restart = _inputActions.FindAction("Restart");
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }
}
