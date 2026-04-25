using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public float distanceOfUse;
    public WeaponAction action;
    public AnimationClip animationClip;
}
