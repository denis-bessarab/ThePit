using UnityEngine;
public class UsableItem : MonoBehaviour
{
    [SerializeField] protected PGS_Item itemReference;

    [Header("Components")]
    [SerializeField] protected Character character;
    [SerializeField] protected CustomInventory inventory;
    [SerializeField] protected UsableItemUI usableItemUI;

    protected virtual void Start()
    {
        character = FindCharacter();
        inventory = FindCharactersInventory();
        usableItemUI = FindUsableItemUI();
    }
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

    protected virtual void RemoveItemFromInventory(int quantity)
    {
        Debug.Log("No implementation for RemoveItemFromInventory()");
    }

    protected Character FindCharacter()
    {
        return FindAnyObjectByType<Character>();
    }

    protected CustomInventory FindCharactersInventory()
    {
        return FindAnyObjectByType<CustomInventory>();
    }

    protected UsableItemUI FindUsableItemUI()
    {
        return FindAnyObjectByType<UsableItemUI>();
    }

}
