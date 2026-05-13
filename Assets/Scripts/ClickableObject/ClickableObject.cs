using UnityEngine;
using UnityEngine.InputSystem;

public abstract class ClickableObject : MonoBehaviour, IOnClick
{
    public abstract void OnClick();
}
