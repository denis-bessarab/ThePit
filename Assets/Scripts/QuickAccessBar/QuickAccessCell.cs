using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickAccessCell : MonoBehaviour, IPointerClickHandler
{
    [Header("Parameters")]
    [SerializeField] private PGS_InventoryCell cellReference;
    [SerializeField] private Color32 defaultColor;
    [SerializeField] private Color32 activeColor;
    [SerializeField] private Action<QuickAccessCell> onClickCallback;
    [SerializeField] private Sprite defaultSprite;

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

            if(cellReference == null)
            {
                SetImage(defaultSprite);
            }
            else
            {
                SyncWithInventoryCell();
            }
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
        SetImage(CellReference.Item.inventoryIcon);
        SetQuantity(CellReference.Quantity);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke(this);
    }

    public void SetClickCallback(Action<QuickAccessCell> callback)
    {
        onClickCallback = callback;
    }
}
