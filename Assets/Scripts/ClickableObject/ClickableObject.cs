using UnityEngine;

public abstract class ClickableObject : MonoBehaviour, IOnClick
{
    public abstract void OnClick();
}
