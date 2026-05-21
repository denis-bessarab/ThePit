using System;
using System.Collections;
using UnityEngine;

public class CustomInventory : PGS_Inventory
{
    [SerializeField] private DynamicItemHolder dynamicItemHolder;

    private void Start()
    {
        StartCoroutine(FindDynamicItemHolder());
    }
    public override Tuple<bool, int> AddItemToInventory(PGS_Item item, int quantity)
    {
        var result = base.AddItemToInventory(item, quantity);
        dynamicItemHolder.ItemAddedToInventoryEvent(item, quantity, result);
        return result;
    }

    private IEnumerator FindDynamicItemHolder()
    {
        while (dynamicItemHolder == null)
        {
            dynamicItemHolder = FindAnyObjectByType<DynamicItemHolder>();
            yield return null;
        }
    }
}
