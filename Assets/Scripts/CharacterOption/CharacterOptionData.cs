using UnityEngine;

public struct CharacterOptionData
{
    public Color32 color;
    public Vector3 position;

    public CharacterOptionData(Color32 color, Vector3 position)
    {
        this.color = color;
        this.position = position;
    }
}
