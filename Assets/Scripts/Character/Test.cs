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
    }

    private void OnDisable()
    {
    }
    private void SubscribeToInputManager()
    {
        var im = InputManager.Instance;
    }
}
