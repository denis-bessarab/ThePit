using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterHit characterHit;
    [SerializeField] private C_MovementSystem movementSystem;
    [SerializeField] private C_CombatSystem combatSystem;


    public void Clinch()
    {
        combatSystem.CombatStateParameter = C_CombatSystem.CombatState.Clinch;
        Debug.Log("Clinch");
    }

    public void ExitClinch()
    {
        combatSystem.CombatStateParameter = C_CombatSystem.CombatState.Free;
        Debug.Log("Exit Clinch");
    }

}
