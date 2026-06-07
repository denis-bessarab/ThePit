using UnityEngine;
using TMPro;
public class CustomIntegerInputField : TMP_InputField, IInputSubscription
{
    protected override void Reset()
    {
        base.Reset();
        contentType = ContentType.IntegerNumber;
    }

    public void SubscribeToInputManager()
    {
        var inputManager = InputManager.Instance;
        if (inputManager == null) return;
        inputManager.TurnOffInputMap("QAB");
    }

    public void UnsubscribeFromInputManager()
    {
        var inputManager = InputManager.Instance;
        if (inputManager == null) return;
        inputManager.TurnOnInputMap("QAB");
    }
}
