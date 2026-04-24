using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class C_StaminaManager : MonoBehaviour
{
    [SerializeField] private float stamina;
    [SerializeField] public float dynamicMaxStamina;
    [SerializeField] public Slider staminaUI;
    [SerializeField] public C_MovementParameters movementParameters;
    [SerializeField] public float staminaSpent;

    private Coroutine staminaRegenerationCoroutine;

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
        dynamicMaxStamina = movementParameters.maxStamina;
        Stamina = dynamicMaxStamina;
    }

    private void UpdateUI(float value)
    {
        staminaUI.value = value;
    }

    public bool IsEnoughStamina(float stamina)
    {
        return Stamina >= stamina;
    }

    public void SpendStamina(float stamina)
    {
        Stamina -= stamina;
        staminaSpent += stamina;
        dynamicMaxStamina = UpdateDynamicStaminaMax(staminaSpent);

        TriggerStaminaRegeneration();
    }

    private IEnumerator StaminaRegenerationCoroutine()
    {
        yield return new WaitForSeconds(1);
        while(Stamina < dynamicMaxStamina)
        {
            Stamina += movementParameters.staminaRegenerationSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private float UpdateDynamicStaminaMax(float staminaSpent)
    {
        return movementParameters.maxStamina - (staminaSpent / movementParameters.everySpentStaminaDecreace);
    }

    public void ResetDynamicMaxStamina()
    {
        dynamicMaxStamina = movementParameters.maxStamina;
    }

    public void TriggerStaminaRegeneration()
    {
        if (staminaRegenerationCoroutine != null)
        {
            StopCoroutine(staminaRegenerationCoroutine);
        }

        staminaRegenerationCoroutine = StartCoroutine(StaminaRegenerationCoroutine());
    }
}
