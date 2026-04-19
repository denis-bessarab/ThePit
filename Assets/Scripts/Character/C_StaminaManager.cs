using UnityEngine;
using UnityEngine.UI;

public class C_StaminaManager : MonoBehaviour
{
    [SerializeField] private float stamina = 100f;
    [SerializeField] public Slider staminaUI;
    [SerializeField] public C_MovementParameters movementParameters;

    public float Stamina
    {
        get => stamina;
        set
        {
            if(value <= 0) value = 0;
            if(value >= movementParameters.maxStamina) value = movementParameters.maxStamina;
            stamina = value;
            UpdateUI(stamina);
        }
    }

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        staminaUI.maxValue = movementParameters.maxStamina;
        stamina = movementParameters.maxStamina;
    }

    private void UpdateUI(float value)
    {
        staminaUI.value = value;
    }
}
