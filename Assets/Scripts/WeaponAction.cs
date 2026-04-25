using UnityEngine;

public class WeaponAction : ScriptableObject, IWeaponAction
{
    public virtual void Use()
    {
        throw new System.NotImplementedException();
    }
}
