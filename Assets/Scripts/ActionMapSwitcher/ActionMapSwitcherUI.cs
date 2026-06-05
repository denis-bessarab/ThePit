using UnityEngine;
using UnityEngine.EventSystems;

public class ActionMapSwitcherUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string actionMap;
    [SerializeField] private string actionMapReturn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log($"Switching map to {actionMap}");
        //var ic = FindAnyObjectByType<C_InputController>();
        //if (ic == null) return;
        //ic.SetActions(actionMap);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"Switching map to {actionMapReturn}");
        //var ic = FindAnyObjectByType<C_InputController>();
        //if (ic == null) return;
        //ic.SetActions(actionMapReturn);
    }

}
