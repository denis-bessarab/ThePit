using UnityEngine;

public class CharacterHit : MonoBehaviour
{
    [SerializeField] private C_InputController cic;
    [SerializeField] private Weapon weapon;

    private void Update()
    {
        var hit = cic.m_hit.WasPressedThisFrame();

        if(hit)
        {
            weapon.action.Use();
        }
    }
}
