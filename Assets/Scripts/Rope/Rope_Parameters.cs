using UnityEngine;

public class Rope_Parameters : MonoBehaviour
{
    [SerializeField] public float maxRopeLenght;
    [SerializeField] public float minRopeLenght;
    [SerializeField] public float swingPower;
    [SerializeField] public float jumpPower;
    [SerializeField] public float lenghtChangeSpeed;

    private void Reset()
    {
        var parameters = GetParameters();
        ApplyParameters(parameters);
    }

    private Rope_ParametersHolder GetParameters()
    {
        return Resources.Load("Parameters/Rope_Parameters") as Rope_ParametersHolder;
    }

    private void ApplyParameters(Rope_ParametersHolder p)
    {
        maxRopeLenght = p.maxRopeLenght;
        minRopeLenght = p.minRopeLenght;
        swingPower = p.swingPower;
        jumpPower = p.jumpPower;
        lenghtChangeSpeed = p.lenghtChangeSpeed;
    }
}
