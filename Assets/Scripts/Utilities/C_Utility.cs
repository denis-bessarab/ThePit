using UnityEngine;
using UnityEngine.InputSystem;

public static class C_Utility
{
    public static Vector3 GetDirectionToPointer(Vector3 pos, Vector3 pointerPos)
    {
        return (pointerPos - pos).normalized;
    }

    public static Vector3 GetMousePosition()
    {
        var mousePosX = Mouse.current.position.x.ReadValue();
        var mousePosY = Mouse.current.position.y.ReadValue();
        var mouseScreenPos = new Vector3 (mousePosX, mousePosY, 0);
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos = new Vector3 (mouseWorldPos.x, mouseWorldPos.y, 0);
        return mouseWorldPos;
    }
}
