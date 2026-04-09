using UnityEngine;

public class CharacterHit : MonoBehaviour
{
    [SerializeField] private C_InputController cic;
    [SerializeField] private Weapon weapon;

    private void Update()
    {
        var hit = cic.rope.WasPressedThisFrame();

        if(hit)
        {
            weapon.action.Use();
        }
    }
}
