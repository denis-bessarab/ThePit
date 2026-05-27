using System;
using System.Collections;
using UnityEngine;

public class CustomInventory : PGS_Inventory
{
    [SerializeField] private ItemDynamicsController itemDynamicsController;

    private void Start()
    {
        StartCoroutine(FindDynamicItemHolder());
    }

    protected override void SendAddItemReport(AddResult result)
    {
        if (itemDynamicsController == null) return;
        itemDynamicsController.SyncItemsAmountBetweenQABAndInventoryAfterInventoryChanges(result, default);
    }

    protected override void SendRemoveItemReport(RemoveResult result)
    {
        if (itemDynamicsController == null) return;
        itemDynamicsController.SyncItemsAmountBetweenQABAndInventoryAfterInventoryChanges(default, result);
    }

    private IEnumerator FindDynamicItemHolder()
    {
        while (itemDynamicsController == null)
        {
            itemDynamicsController = FindAnyObjectByType<ItemDynamicsController>();
            yield return null;
        }
    }
}
