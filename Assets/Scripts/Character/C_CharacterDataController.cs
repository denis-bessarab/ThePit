using UnityEngine;

public class C_CharacterDataController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterData characterData;

    [Header("Components Refereces")]
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public C_MovementParameters movementParameters;

    public CharacterData CharacterData
    {
        get => characterData;
        set
        {
            characterData = value;
            SetMovementParameters(characterData.movementParameters);
            SetAppearance(characterData.appearance);
        }
    }

    public void SetMovementParameters(C_MovementParametersHolder p)
    {
        movementParameters.ApplyParameters(p);
    }

    public void SetAppearance(C_Appearance a)
    {
        spriteRenderer.sprite = a.sprite;
        spriteRenderer.color = a.color;
    }

    public void SetName()
    {

    }

    public void SetId()
    {

    }
}
