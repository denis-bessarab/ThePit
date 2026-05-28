using UnityEngine;

public class ActionMapSwitcher : MonoBehaviour
{
    [SerializeField] private string actionMap;
    [SerializeField] private string actionMapReturn;
    [SerializeField] private bool isActive;
    

    public void OnMouseEnter()
    {
        Debug.Log($"Switching map to {actionMap}");
        if (actionMap == string.Empty) { Debug.LogWarning("actionMap is not provided"); return; } 
        var ic = FindAnyObjectByType<C_InputController>();
        if (ic == null) return;
        isActive = true;
        ic.SetActions(actionMap);
    }

    public void OnMouseExit()
    {
        Debug.Log($"Switching map to {actionMapReturn}");
        if (actionMapReturn == string.Empty) { Debug.LogWarning("actionMapReturn is not provided"); return; }
        var ic = FindAnyObjectByType<C_InputController>();
        if (ic == null) return;
        if (!isActive) return;
        ic.SetActions(actionMapReturn);
    }

    public void OnClick()
    {
        Debug.Log("Click!");
        //Debug.Log($"Switching map to {actionMapReturn}");
        //if (actionMapReturn == string.Empty) { Debug.LogWarning("actionMapReturn is not provided"); return; }
        //var ic = FindAnyObjectByType<C_InputController>();
        //if (ic == null) return;
        //if (!isActive) return;
        //ic.SetActions(actionMapReturn);
    }
}
