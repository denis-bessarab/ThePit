using UnityEngine;

[CreateAssetMenu(fileName = "C_Appearance", menuName = "Appearances/C_Appearance")]

public class C_Appearance : ScriptableObject
{
    [SerializeField] public Sprite sprite;
    [SerializeField] public Color color;
}
