using UnityEngine;
[CreateAssetMenu(fileName = "Rope_ParametersHolder", menuName = "Parameters/Rope_ParametersHolder")]

public class Rope_ParametersHolder : ScriptableObject
{
    [SerializeField] public float maxRopeLenght;
    [SerializeField] public float minRopeLenght;
    [SerializeField] public float swingPower;
    [SerializeField] public float jumpPower;
    [SerializeField] public float lenghtChangeSpeed;
}
