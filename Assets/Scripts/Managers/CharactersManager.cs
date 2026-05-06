using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CharactersManager : Singleton<CharactersManager>
{
    [SerializeField] private int charactersAmount;
    private bool isOptionsGenerated;
    public static List<CharacterOptionData> characterOptionsData = new();
    public static List<GameObject> characterOptionsGameObjects = new();
    public static CharacterOptionData activeCharacterData;
    public static Character activeCharacter;

    private void Start()
    {
        GenerateCharacterOptions();
        SpawnCharacterOptionsGameObjects();
        StartCoroutine(GetActiveCharacter());
    }
    private void GenerateCharacterOptions()
    {
        if (isOptionsGenerated) return;
        for (int i = 0; i < charactersAmount; i++)
        {
            var characterOptionData = new CharacterOptionData(GenerateSpriteColor(), new Vector3(i - 4 + 0.5f * i, 0, 0));
            characterOptionsData.Add(characterOptionData);
        }
        isOptionsGenerated = true;
    }

    public void SpawnCharacterOptionsGameObjects()
    {

        for (int i = 0; i < characterOptionsData.Count; i++)
        {
            var go = Instantiate(Resources.Load("Prefabs/CharacterOption") as GameObject);
            go.GetComponent<SpriteRenderer>().color = characterOptionsData[i].color;
            go.transform.position = characterOptionsData[i].position;
            characterOptionsGameObjects.Clear();
            characterOptionsGameObjects.Add(go);
            go.name = "CharacterOption";
        }
    }

    public static void SwitchCharacter(CharacterOption co)
    {
        var newCharacterSR = co.GetComponent<SpriteRenderer>();
        var activeCharacterSR = activeCharacter.GetComponent<SpriteRenderer>();

        var activeCharacterColor = activeCharacterSR.color;
        var newCharacterColor = newCharacterSR.color;

        newCharacterSR.color = activeCharacterColor;
        activeCharacterSR.color = newCharacterColor;
    }

    private Color32 GenerateSpriteColor()
    {
        return new Color32(
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)255f);
    }

    private IEnumerator GetActiveCharacter()
    {
        GameObject go = null;
        while(go == null)
        {
            go = GameObject.Find("Character");
            yield return null;
        }

        CharacterOptionData data = new CharacterOptionData(go.GetComponent<SpriteRenderer>().color, transform.position);
        activeCharacterData = data;
        activeCharacter = go.GetComponent<Character>();
    }
}
