using System;

[Serializable]
public struct CharacterData
{
    public int id;
    public string name;
    public C_Appearance appearance;
    public C_MovementParametersHolder movementParameters;

    public CharacterData(int id, string name, C_Appearance a, C_MovementParametersHolder p )
    {
        this.id = id;
        this.name = name;
        appearance = a;
        movementParameters = p;
    }
}
