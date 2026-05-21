using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickAccessCell : MonoBehaviour, IPointerClickHandler
{
    [Header("Parameters")]
    //[SerializeField] private PGS_InventoryCell cellReference;
    [SerializeField] private Color32 defaultColor;
    [SerializeField] private Color32 activeColor;
    [SerializeField] private Action<QuickAccessCell> onClickCallback;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private CustomItem item;
    [SerializeField] private int quantity;

    [Header("Components")]
    [SerializeField] public QuickAccessBar quickAccessBar;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    public CustomItem Item
    {
        get => item;
        set
        {
            item = value;

            if(item == null)
            {
                SetImage(defaultSprite);
                Quantity = 0;
            }
            else
            {
                SetImage(item.inventoryIcon);
                Quantity = quickAccessBar.dynamicItemHolder.RequestItemAmountInInventory(item);
            }
        }
    }

    public int Quantity
    {
        get => quantity;
        set
        {
            quantity = value;
            UpdateQuantityUI(quantity);
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
        quantityText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void SetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }

    private void UpdateQuantityUI(int quantity)
    {
        if(quantity == 0) quantityText.text = string.Empty;
        else quantityText.text = quantity.ToString();
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
