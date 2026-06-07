using System;
using System.Collections;
using UnityEngine;

public class CustomInventory : PGS_Inventory, IInputSubscription
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private ItemDynamicsController itemDynamicsController;

    private void OnEnable()
    {
        StartCoroutine(FindDynamicItemHolder());
        StartCoroutine(FindInputManager(SubscribeToInputManager));
    }

    private void OnDisable()
    {
        UnsubscribeFromInputManager();
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

    private IEnumerator FindInputManager(Action callback)
    {
        while (inputManager == null)
        {
            inputManager = InputManager.Instance;
            yield return null;
        }

        callback?.Invoke();
    }

    public void ToggleInventory()
    {
        IsInventoryOpen = !IsInventoryOpen;
    }

    public void SubscribeToInputManager()
    {
        if (inputManager == null) return;
        inputManager.onInventory.Insert(0, ToggleInventory);
    }

    public void UnsubscribeFromInputManager()
    {
        if (inputManager == null) return;
        inputManager.onInventory.Remove(ToggleInventory);
    }
}
