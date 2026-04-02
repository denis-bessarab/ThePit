using UnityEngine;
[RequireComponent(typeof(C_InputController))]
[RequireComponent(typeof(C_MovementDataCollector))]
public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Input Controller")]
    private C_InputController inputController;
    [SerializeField]
    [InspectorName("Movement Data Collector")]
    private C_MovementDataCollector movementDataCollector;

    private void Reset()
    {
        inputController = GetComponent<C_InputController>();
        movementDataCollector = GetComponent<C_MovementDataCollector>();
    }

}
