using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickAccessCell : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private PGS_InventoryCell cellReference;
    [SerializeField] private Color32 defaultColor;
    [SerializeField] private Color32 activeColor;

    [Header("Components")]
    [SerializeField] public QuickAccessBar quickAccessBar;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI quantity;

    public PGS_InventoryCell CellReference
    {
        get => cellReference;
        set
        {
            cellReference = value;
            SyncWithInventoryCell();
        }
    }

    public void ActivateCell()
    {
        backgroundImage.color = activeColor;
    }

    public void DeactivateCell()
    {
        backgroundImage.color = defaultColor;
    }

    private void Reset()
    {
        SetupCell();
    }

    private void SetupCell()
    {
        backgroundImage = GetComponent<Image>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
        quantity = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void SetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }

    private void SetQuantity(int quantity)
    {
        this.quantity.text = quantity.ToString();
    }

    public void SyncWithInventoryCell()
    {
        SetImage(cellReference.Item.inventoryIcon);
        SetQuantity(cellReference.Quantity);
    }
}
