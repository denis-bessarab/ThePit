using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]   
public class RosterCharacter : ClickableObject
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Data")]
    [SerializeField] private CharacterData characterData;


    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public CharacterData CharacterData
    {
        get => characterData;
        set
        {
            characterData = value;
            SetAppearance(characterData);
        }
    }

    private void SetAppearance(CharacterData cd)
    {
        spriteRenderer.sprite = cd.appearance.sprite;
        spriteRenderer.color = cd.appearance.color;
    }

    public override void OnClick()
    {
        CharactersManager.Instance.ChangeCharacter(characterData);
    }
}
