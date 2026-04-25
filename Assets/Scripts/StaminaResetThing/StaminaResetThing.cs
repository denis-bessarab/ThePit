using UnityEngine;

public class StaminaResetThing : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Character")
        {
            var sm = collision.GetComponent<C_StaminaManager>();
            sm.staminaSpent = 0;
            sm.ResetDynamicMaxStamina();
            sm.TriggerStaminaRegeneration(true);
            Destroy(gameObject);
        }
    }
}
