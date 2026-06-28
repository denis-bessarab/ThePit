using UnityEngine;
[CreateAssetMenu(fileName = "Start Location Parameters", menuName = "PCG/Parameters/Start Location Parameters")]
public class StartLocationParameters : ScriptableObject
{
    [SerializeField] public Vector2 startTilePosition;
    [SerializeField] public Vector2 bounds;
}
