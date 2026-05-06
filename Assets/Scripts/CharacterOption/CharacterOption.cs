using UnityEngine;

public class CharacterOption : ClickableObject
{
    public override void OnClick()
    {
        CharactersManager.SwitchCharacter(this);
    }
}
