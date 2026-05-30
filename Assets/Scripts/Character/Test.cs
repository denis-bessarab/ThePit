using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] C_MovementDataCollector dataCollector;
    private void Start()
    {
        SubscribeToInputManager();
    }

    private void OnEnable()
    {
        InputManager.Instance.EnterState(InputManager.InputState.Character);
    }

    private void OnDisable()
    {
        InputManager.Instance.ExitState();
    }
    private void SubscribeToInputManager()
    {
        var im = InputManager.Instance;
    }
}
