using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelZone : MonoBehaviour, IPointerClickHandler
{
    public Action clickCallback;
    public void OnPointerClick(PointerEventData eventData)
    {
        clickCallback?.Invoke();
    }
}

