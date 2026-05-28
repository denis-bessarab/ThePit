using UnityEngine;
using UnityEngine.InputSystem;

public class ClickableObjectsManager : Singleton<ClickableObjectsManager>
{
    void Update()
    {
        var mousePos = C_Utility.GetMouseWorldPosition();

        var rc = Physics2D.Raycast(mousePos, Vector2.zero);

        if (rc && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if(rc.collider.TryGetComponent<ClickableObject>(out var clickable))
            {
                clickable.OnClick();
            }
        }
    }
}
