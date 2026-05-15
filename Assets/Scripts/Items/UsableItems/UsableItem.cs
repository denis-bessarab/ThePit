using UnityEngine;
public class UsableItem : MonoBehaviour
{
    [SerializeField] private PGS_Item itemReference;

    public virtual void LMBAction()
    {
        Debug.Log("No implementation for LMBAction");
    }

    public virtual void RMBAction()
    {
        Debug.Log("No implementation for RMBAction");
    }

    public virtual void LMBHoldAction()
    {
        Debug.Log("No implementation for LMBHoldAction");
    }

    public virtual void RMBHoldAction()
    {
        Debug.Log("No implementation for RMBHoldAction");
    }

    public virtual void LMBReleaseAction()
    {
        Debug.Log("No implementation for LMBReleaseAction");
    }

    public virtual void RMBReleaseAction()
    {
        Debug.Log("No implementation for RMBReleaseAction");
    }
}
