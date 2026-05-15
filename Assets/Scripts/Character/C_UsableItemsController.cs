using UnityEngine;

public class C_UsableItemsController : MonoBehaviour
{
    [SerializeField] private UsableItem currentUsableItem;
    [SerializeField] private GameObject itemSceneReference;

    public UsableItem CurrentUsableItem
    {
        get => currentUsableItem;
        set
        {
            currentUsableItem = value;
        }
    }

    public void LMBAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.LMBAction();
    }

    public void RMBAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.RMBAction();
    }

    public void LMBHoldAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.LMBHoldAction();
    }

    public void RMBHoldAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.RMBHoldAction();
    }

    public void LMBReleaseAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.LMBReleaseAction();
    }

    public void RMBReleaseAction()
    {
        if (currentUsableItem == null) return;
        currentUsableItem.RMBReleaseAction();
    }
}
