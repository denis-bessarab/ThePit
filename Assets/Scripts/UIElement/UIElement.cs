using UnityEngine;
using UnityEngine.EventSystems;
public class UIElement : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        var inputManager = InputManager.Instance;
        if (inputManager == null) return;
        inputManager.TurnOnInputMap("MouseActions");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var inputManager = InputManager.Instance;
        if (inputManager == null) return;
        inputManager.TurnOffInputMap("MouseActions");
    }
}
