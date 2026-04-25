using UnityEngine;
using UnityEngine.InputSystem;

public class Rope_InputController : MonoBehaviour
{
    [SerializeField] public InputActionAsset _inputActions;

    public InputAction up;
    public InputAction down;
    public InputAction swingLeft;
    public InputAction swingRight;
    public InputAction jump;
    public InputAction rope;

    private void Reset()
    {
        _inputActions = GetInputActionAsset();
        enabled = false;
    }

    private void OnEnable()
    {
        _inputActions.FindActionMap("Rope").Enable();
        _inputActions.FindActionMap("Character").Disable();
        SetActions();
    }

    private void OnDisable()
    {
        _inputActions.FindActionMap("Rope").Disable();
        _inputActions.FindActionMap("Character").Enable();
    }

    private void SetActions()
    {
        up = _inputActions.FindActionMap("Rope").FindAction("Up");
        down = _inputActions.FindActionMap("Rope").FindAction("Down");
        swingLeft = _inputActions.FindActionMap("Rope").FindAction("SwingLeft");
        swingRight = _inputActions.FindActionMap("Rope").FindAction("SwingRight");
        jump = _inputActions.FindActionMap("Rope").FindAction("Jump");
        rope = _inputActions.FindActionMap("Rope").FindAction("Rope");
    }

    private InputActionAsset GetInputActionAsset()
    {
        return Resources.Load("InputSystem_Actions") as InputActionAsset;
    }
}
