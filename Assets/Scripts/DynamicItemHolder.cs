using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class DynamicItemHolder : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject imageGameObject;
    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Sprite defaultImage;
    //[SerializeField] private PGS_InventoryCell itemTookCellRef;
    [SerializeField] private PGS_Item item;
    [SerializeField] private int quantity;
    [SerializeField] private PGS_Inventory inventory;
    [SerializeField] private QuickAccessBar quickAccessBar;
    [SerializeField] private List<PGS_InventoryCell> inventoryCells;
    [SerializeField] private List<QuickAccessCell> quickAccessBarCells;
    [SerializeField] private PGS_InventoryCell cellReference;

    private bool inventoryCellsGot;
    private bool accessCellsGot;
    private bool initialStateSet;

    public PGS_Item Item
    {
        get => item;
        set
        {
            item = value;

            if(item == null)
            {
                ResetItemImage();
                SetGetActionToInventoryCells();
                SetGetActionToAccessCells();
                HideSelf();
            }
            else
            {
                SetItemImage(item.inventoryIcon);
                SetSetActionToInventoryCells();
                SetSetActionToAccessCells();
                ShowSelf();
            }
        }
    }

    public PGS_InventoryCell CellReference
    {
        get => cellReference;
        set
        {
            cellReference = value;

            if(cellReference == null)
            {
                ResetItemImage();
                SetGetActionToAccessCells();
                SetGetActionToInventoryCells();
                HideSelf();
            }
            else
            {
                SetItemImage(cellReference.Item.inventoryIcon);
                SetSetActionToAccessCells();
                SetSetActionToInventoryCells();
                ShowSelf();
            }
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
    }

    private void Update()
    {
        if(!initialStateSet && inventoryCellsGot && accessCellsGot)
        {
            SetGetActionToInventoryCells();
            SetGetActionToAccessCells();
            initialStateSet = true;
        }

        if (!gameObject.activeSelf) return;
        FollowThePointer();
    }

    private void SetGetActionToInventoryCells()
    {
        for (int i = 0; i < inventoryCells.Count; i++)
        {
            inventoryCells[i].SetClickCallback(GetItemFromCell);
        }
    }

    private void SetSetActionToInventoryCells()
    {
        for (int i = 0; i < inventoryCells.Count; i++)
        {
            inventoryCells[i].SetClickCallback(SetItemToCell);
        }
    }

    private void SetGetActionToAccessCells()
    {
        for (int i = 0; i < quickAccessBarCells.Count; i++)
        {
            quickAccessBarCells[i].SetClickCallback(GetQuickAccessCell);
        }
    }

    private void SetSetActionToAccessCells()
    {
        for (int i = 0; i < quickAccessBarCells.Count; i++)
        {
            quickAccessBarCells[i].SetClickCallback(SetItemToQuickAccessCell);
        }
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

    public void GetItemFromCell(PGS_InventoryCell cell)
    {
        if(cell.Item == null) return;
        Item = cell.Item;
        Quantity = cell.Quantity;
        CellReference = cell;
    }

    public void SetItemToCell(PGS_InventoryCell cell)
    {
        if (cell == null) return;
        if (CellReference == null) return;
        if(cell.Item == null)
        {
            cell.Item = Item;
            cell.Quantity = Quantity;
            CellReference.Item = null;
        }
        else
        {
            CellReference.Item = cell.Item;
            CellReference.Quantity = cell.Quantity;
            cell.Item = Item;
            cell.Quantity = Quantity;
        }
        Item = null;
        Quantity = -1;
        CellReference = null;
    }

    private void SetItemImage(Sprite sprite)
    {
        imageComponent.sprite = sprite;
    }

    private void ResetItemImage()
    {
        imageComponent.sprite = defaultImage;
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

    private void ShowSelf()
    {
        canvas.enabled = true;
    }

    private void HideSelf()
    {
        canvas.enabled = false;
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

    public void GetQuickAccessCell(QuickAccessCell cell)
    {
        CellReference = cell.CellReference;
    }

    public void SetItemToQuickAccessCell(QuickAccessCell cell)
    {
        cell.CellReference = CellReference;
        CellReference = null;
    }
}
