using UnityEngine;
using UnityEngine.UI;

public class UsableItemUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject container;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject breakPoint;
    [SerializeField] private RectTransform breakPointTransform;

    public void ShowUI()
    {
        container.SetActive(true);
    }

    public void HideUI()
    {
        container.SetActive(false);
    }

    public void SetupUsableItemUI(float minValue, float maxValue, bool breakPoint, float breakPointPercent)
    {
        ResetUsableItemUI();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
        this.breakPoint.SetActive(breakPoint);
        var pos = new Vector3(rectTransform.rect.width * breakPointPercent, 0f, 0f);
        breakPointTransform.anchoredPosition = pos;
    }

    public void SetupUsableItemUI(float minValue, float maxValue)
    {
        ResetUsableItemUI();
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    public void UpdateValue(float value)
    {
        slider.value = value;
    }

    private void ResetUsableItemUI()
    {
        slider.minValue = 0;
        slider.maxValue = 1;
        breakPoint.SetActive(false);
        var pos = new Vector3(0f, 0f, 0f);
        breakPointTransform.localPosition = pos;
    }
}
