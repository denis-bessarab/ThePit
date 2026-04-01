using UnityEngine;
[CreateAssetMenu(fileName = "FistAction", menuName = "WeaponActions/FistAction")]
public class FistAction : WeaponAction
{
    public override void Use()
    {
        Debug.Log("Fist action");
    }
}
