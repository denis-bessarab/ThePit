using UnityEngine;

public class MenuInputResolver : MonoBehaviour
{
    public void ResolveMenuInput(MenuInputManager mim, GameManager gm)
    {
        if(mim.pause.WasPressedThisFrame() && gm != null) gm.PauseUnpauseGame();
    }
}
