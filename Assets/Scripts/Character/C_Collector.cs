using UnityEngine;

public class C_Collector : MonoBehaviour
{
    [SerializeField] public int goodThingsCollected = 0;

    public void CollectGoodThing(int amount = 1)
    {
        goodThingsCollected++;
    }
}
