using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DynamicItemHolder : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject imageGameObject;
    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private PGS_Item item;
    [SerializeField] private int quantity;
    [SerializeField] private PGS_Inventory inventory;
    [SerializeField] private QuickAccessBar quickAccessBar;
    [SerializeField] private List<PGS_InventoryCell> inventoryCells;
    [SerializeField] private List<QuickAccessCell> quickAccessBarCells;
    [SerializeField] private CancelZone cancelZone;
    [SerializeField] private ClickData firstClick;
    [SerializeField] private ClickData secondClick;

    private bool inventoryCellsGot;
    private bool accessCellsGot;
    public enum ClickDataType
    {
        NA,
        InventoryCell,
        QuickAccessData,
        CancelZone
    }
    public struct ClickData
    {
        public ClickDataType type;
        public QuickAccessCell quickAcceccCellReference;
        public PGS_InventoryCell inventoryCellReference;

        public ClickData(ClickDataType type,  QuickAccessCell quickAcceccCellReference, PGS_InventoryCell inventoryCellReference)
        {
            this.type = type;
            this.quickAcceccCellReference = quickAcceccCellReference;
            this.inventoryCellReference = inventoryCellReference;
        }
    }

    public PGS_Item Item
    {
        get => item;
        set
        {
            item = value;

            if(item == null)
            {
                SetItemImage(defaultImage);
                HideSelf();
                return;
            }

            SetItemImage(item.inventoryIcon);
            ShowSelf();
        }
    }
    public int Quantity
    {
        get => quantity;
        set
        {
            quantity = value;
            quantityText.text = quantity.ToString();
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForInventoryConfigurationAndGetCells());
        StartCoroutine(WaitForAccessBarConfigurationAndGetCells());
        StartCoroutine(SetActionsToAllCells());
        cancelZone.clickCallback = GetCancelZoneClick;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        FollowThePointer();
    }

    private IEnumerator WaitForInventoryConfigurationAndGetCells()
    {
        while (inventory && inventoryCells.Count == 0)
        {
            inventoryCells = inventory.GetInventoryCells();
            yield return inventoryCells;
        }

        inventoryCellsGot = true;
    }

    private IEnumerator WaitForAccessBarConfigurationAndGetCells()
    {
        while (quickAccessBar && quickAccessBarCells.Count == 0)
        {
            quickAccessBarCells = quickAccessBar.GetQuickAccessBarCells();
            yield return quickAccessBarCells;
        }

        accessCellsGot = true;
    }

    private IEnumerator SetActionsToAllCells()
    {
        while (!(inventoryCellsGot && accessCellsGot))
        {
            yield return null;
        }

        SetGetActionToInventoryCells();
        SetGetActionToAccessCells();
    }

    private void SetGetActionToInventoryCells()
    {
        for (int i = 0; i < inventoryCells.Count; i++)
        {
            inventoryCells[i].SetClickCallback(GetInventoryCell);
        }
    }
    private void SetGetActionToAccessCells()
    {
        for (int i = 0; i < quickAccessBarCells.Count; i++)
        {
            quickAccessBarCells[i].SetClickCallback(GetQuickAcceccCell);
        }
    }

    private void ClearClicksMemory()
    {
        firstClick = default;
        secondClick = default;
        Item = null;
        Quantity = -1;
    }

    public void ShowItemHolder()
    {
        gameObject.SetActive(false);
    }

    public void HideItemHolder()
    {
        gameObject.SetActive(true);
    }

    public void FollowThePointer()
    {
        imageGameObject.transform.position = C_Utility.GetMouseScreenPosition();
    }

    public void GetInventoryCell(PGS_InventoryCell cell)
    {
        if(firstClick.type == ClickDataType.NA)
        {
            if (cell.Item == null) return;
            firstClick = new ClickData(ClickDataType.InventoryCell, default, cell);
            Item = cell.Item;
            Quantity = cell.Quantity;
            return;
        }

        if(secondClick.type == ClickDataType.NA)
        {
            secondClick = new ClickData(ClickDataType.InventoryCell, default, cell);
        }

        ProcessClicksCombinations(firstClick, secondClick);
    }

    public void GetQuickAcceccCell(QuickAccessCell cell)
    {
        if (firstClick.type == ClickDataType.NA)
        {
            if (cell.Item == null) return;
            firstClick = new ClickData(ClickDataType.QuickAccessData, cell, default);
            Item = cell.Item;
            Quantity = cell.Quantity;
            return;
        }

        if (secondClick.type == ClickDataType.NA)
        {
            secondClick = new ClickData(ClickDataType.QuickAccessData, cell, default);
        }

        ProcessClicksCombinations(firstClick, secondClick);
    }

    public void GetCancelZoneClick()
    {
        if (firstClick.type == ClickDataType.NA) return;

        if(secondClick.type == ClickDataType.NA)
        {
            secondClick = new ClickData(ClickDataType.CancelZone, default, default);
        }

        ProcessClicksCombinations(firstClick, secondClick);
    }

    private void SetItemImage(Sprite sprite)
    {
        imageComponent.sprite = sprite;
    }

    private void ShowSelf()
    {
        canvas.enabled = true;
    }

    private void HideSelf()
    {
        canvas.enabled = false;
    }

    private void ProcessClicksCombinations(ClickData firstClick, ClickData secondClick)
    {
        switch (firstClick.type,secondClick.type)
        {
            case (ClickDataType.InventoryCell, ClickDataType.InventoryCell):
                ProcessInventoryInventory(firstClick.inventoryCellReference, secondClick.inventoryCellReference);
                break;
            case (ClickDataType.InventoryCell, ClickDataType.QuickAccessData):
                ProcessInventoryQAB(firstClick.inventoryCellReference, secondClick.quickAcceccCellReference);
                break;
            case (ClickDataType.InventoryCell, ClickDataType.CancelZone):
                ProcessInventoryCancelZone(firstClick.inventoryCellReference);
                break;
            case (ClickDataType.QuickAccessData, ClickDataType.InventoryCell):
                ProcessQABInventory(firstClick.quickAcceccCellReference, secondClick.inventoryCellReference);
                break;
            case (ClickDataType.QuickAccessData, ClickDataType.QuickAccessData):
                ProcessQABQAB(firstClick.quickAcceccCellReference,secondClick.quickAcceccCellReference);
                break;
            case (ClickDataType.QuickAccessData, ClickDataType.CancelZone):
                ProcessQABCancelZone(firstClick.quickAcceccCellReference);
                break;
        }

        ClearClicksMemory();
    }

    private void ProcessInventoryInventory(PGS_InventoryCell cell1, PGS_InventoryCell cell2)
    {
        if(cell1.Item != null && cell2.Item != null)
        {
            if(cell1.Item.id == cell2.Item.id)
            {
                MergeItemsInInventoryCell(cell1, cell2);
            }

            if(cell1.Item.id != cell2.Item.id)
            {
                SwapInventoryItems(cell1, cell2);
            }
        }

        if(cell1.Item != null && cell2.Item == null)
        {
            MoveItemToNewInventoryCell(cell1, cell2);
        }
    }

    private void ProcessInventoryQAB(PGS_InventoryCell cell1, QuickAccessCell cell2)
    {
        var item = cell1.Item as CustomItem;
        cell2.Item = item;
    }

    private void ProcessInventoryCancelZone(PGS_InventoryCell cell1)
    {
        Debug.Log("No implemented interactions sequention");
    }

    private void ProcessQABInventory(QuickAccessCell cell1, PGS_InventoryCell cell2)
    {
        Debug.Log("No implemented interactions sequention");
    }

    private void ProcessQABQAB(QuickAccessCell cell1, QuickAccessCell cell2)
    {
        cell2.Item = cell1.Item;
        cell1.Item = null;
    }

    private void ProcessQABCancelZone(QuickAccessCell cell1)
    {
        cell1.Item = null;
    }

    private void MergeItemsInInventoryCell(PGS_InventoryCell cell1, PGS_InventoryCell cell2)
    {
        var availableAmount = cell2.Item.maxStackAmount - cell2.Quantity;
        //MERGE WITHOUT REST
        if (availableAmount > cell1.Quantity)
        {
            cell2.Quantity += cell1.Quantity;
            cell1.Item = null;
        }
        //MERGE WITH REST
        else
        {
            cell2.Quantity += availableAmount;
            cell1.Quantity -= availableAmount;
        }
    }
    private void SwapInventoryItems(PGS_InventoryCell cell1, PGS_InventoryCell cell2)
    {
        //CREATE EMPTY INSTANCE
        var cell2ItemMemory = ScriptableObject.CreateInstance<CustomItem>();

        //CAST CLASS TYPE
        var cell2CustomItem = cell2.Item as CustomItem;

        //SAVE PROPERTIES FROM CELL2 ITEM
        cell2ItemMemory.id = cell2CustomItem.id;
        cell2ItemMemory.itemName = cell2CustomItem.itemName;
        cell2ItemMemory.description = cell2CustomItem.description;
        cell2ItemMemory.inventoryIcon = cell2CustomItem.inventoryIcon;
        cell2ItemMemory.maxStackAmount = cell2CustomItem.maxStackAmount;
        cell2ItemMemory.usableItemPrefab = cell2CustomItem.usableItemPrefab;
        var cell2QuantityMemory = cell2.Quantity;

        //SWAP ITEMS
        cell2.SetItem(cell1.Item, cell1.Quantity);
        cell1.SetItem(cell2ItemMemory, cell2QuantityMemory);
    }

    private void MoveItemToNewInventoryCell(PGS_InventoryCell cell1, PGS_InventoryCell cell2)
    {
        cell2.SetItem(cell1.Item, cell1.Quantity);
        cell1.RemoveItem();
    }

    public int RequestItemAmountInInventory(PGS_Item item)
    {
        return inventory.SummarizeItemQuantityInAllCells(item);
    }

    public void ItemAddedToInventoryEvent(PGS_Item item, int quantity, Tuple<bool, int> result)
    {
        Debug.Log($"Got informed that in inventory was added {item.itemName} with {result.Item1} result and in rest of {result.Item2} in amount of {quantity}");
    }
}
