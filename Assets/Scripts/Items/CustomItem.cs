using UnityEngine;
[CreateAssetMenu(fileName = "Custom Item", menuName = "Custom Item")]
public class CustomItem : PGS_Item
{
    [SerializeField] public GameObject usableItemPrefab;

    public CustomItem(
        int id, 
        string itemName, 
        string description, 
        Sprite inventoryIcon, 
        int maxStackAmount,
        GameObject usableItemPrefab
        ) : base(
            id, 
            itemName, 
            description, 
            inventoryIcon, 
            maxStackAmount
            )
    {
        this.usableItemPrefab = usableItemPrefab;
    }
    public CustomItem(CustomItem item) : base(item)
    {
        usableItemPrefab = item.usableItemPrefab;
    }
}
