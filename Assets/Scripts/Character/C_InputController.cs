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
    public InputAction m_hit;

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
        m_left = InputSystem.actions.FindAction("Left");
        m_right = InputSystem.actions.FindAction("Right");
        m_jump = InputSystem.actions.FindAction("Jump");
        m_down = InputSystem.actions.FindAction("Down");
        m_up = InputSystem.actions.FindAction("Up");
        m_sprint = InputSystem.actions.FindAction("Sprint");
        m_hit = InputSystem.actions.FindAction("Hit");
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }
}
