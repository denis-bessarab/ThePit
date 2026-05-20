using UnityEngine;
using UnityEngine.InputSystem;

public class ClickableObjectsManager :Singleton<ClickableObjectsManager>
{
    void Update()
    {
        var mousePos = C_Utility.GetMouseWorldPosition();

        var rc = Physics2D.Raycast(mousePos, Vector2.zero, LayerMask.NameToLayer("Character"));
        if (rc && Mouse.current.leftButton.wasPressedThisFrame && rc.collider.name == "RosterCharacter")
        {
            rc.collider.gameObject.GetComponent<RosterCharacter>().OnClick();
        }
    }
}
