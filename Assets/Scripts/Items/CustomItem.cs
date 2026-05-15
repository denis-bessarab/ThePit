using UnityEngine;
[CreateAssetMenu(fileName = "Custom Item", menuName = "Custom Item")]
public class CustomItem : PGS_Item
{
    [SerializeField] public GameObject usableItemPrefab;
}
