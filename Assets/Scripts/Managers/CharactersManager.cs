using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class CharactersManager : Singleton<CharactersManager>
{
    [SerializeField] public int charactersAmount;
    [SerializeField] private Vector3 instantiatePosition;
    [SerializeField] public List<CharacterData> charactersData;
    [SerializeField] private Character character;
    [SerializeField] private List<C_MovementParametersHolder> movementParametersPresets;
    [SerializeField] private List<C_Appearance> appearancePresets;
    [SerializeField] public CharacterData activeCharacter;

    [Header("Operational Flags")]
    [SerializeField] public bool areMovementPresetsLoaded;
    [SerializeField] public bool areApperancePresetsLoaded;
    [SerializeField] public bool areCharactersGenerated;
    [SerializeField] public bool isDataReady;


    private void Start()
    {
        movementParametersPresets = LoadParametersPresets();
        appearancePresets = LoadAppearancePresets();
        GenerateCharacters();
        InstantiateCharacter();
    }

    public void InstantiateCharacter()
    {
        if (character != null) return;

        var op = SceneManager.LoadSceneAsync("CharacterScene", LoadSceneMode.Additive);
        op.completed += AfterInstantiateSetup;
        op.completed += _ => SetCharacterPosition(instantiatePosition);
        op.completed += _ => AssignCharactersData();
        op.completed += _ => isDataReady = true;
    }

    private void AssignCharactersData(int index = 0)
    {
        if(activeCharacter.id != 0)
        {
            character.dataController.CharacterData = activeCharacter;
        }
        else
        {
            character.dataController.CharacterData = charactersData[index];
            activeCharacter = charactersData[index];
        }
    }

    private void AssignCharactersData(CharacterData cd)
    {
        character.dataController.CharacterData = cd;
        activeCharacter = cd;
    }

    private void GenerateCharacters()
    {
        if (areCharactersGenerated) return;

        for (int i = 0; i < charactersAmount; i++)
        {
            var appearance = ChooseApperancePreset();
            var movementParameters = ChooseMovementParametersPreset();

            var newCharactersData = new CharacterData(i + 1, $"Character-{i + 1}", appearance, movementParameters);
            charactersData.Add(newCharactersData);
        }

        areCharactersGenerated = true;
    }

    private C_Appearance ChooseApperancePreset()
    {
        return appearancePresets[Random.Range(0,appearancePresets.Count)];
    }

    private C_MovementParametersHolder ChooseMovementParametersPreset()
    {
        return movementParametersPresets[Random.Range(0, movementParametersPresets.Count)];
    }

    public void ChangeCharacter(CharacterData cd)
    {
        activeCharacter = cd;
        AssignCharactersData(cd);
        FindAndUpdateRoster();
    }

    private void SetCharacterPosition(Vector3 pos)
    {
        if (character == null) return;
        character.gameObject.transform.position = pos;
    }

    private List<C_MovementParametersHolder> LoadParametersPresets()
    {
        if (areMovementPresetsLoaded) return movementParametersPresets;
        var presets = Resources.LoadAll<C_MovementParametersHolder>("Parameters/C_MovementParametersPresets");
        areMovementPresetsLoaded = true;
        return presets.ToList();
    }

    private List<C_Appearance> LoadAppearancePresets()
    {
        if (areApperancePresetsLoaded) return appearancePresets;
        var presets = Resources.LoadAll<C_Appearance>("Appearances/AppearancePresets");
        areApperancePresetsLoaded = true;
        return presets.ToList();
    }

    private void AfterInstantiateSetup(AsyncOperation op)
    {
        var character = GameObject.Find("Character");
        var c = character.GetComponent<Character>();
        this.character = c;
    }

    private void FindAndUpdateRoster()
    {
        var roster = GameObject.Find("Roster");
        if(roster == null) return;
        var r = roster.GetComponent<Roster>();
        r.UpdateRoster();
    }

    public void GetCommandWhenSceneChanges(string sceneName)
    {
        //Debug.Log($"I see that change to {sceneName}");

        switch(sceneName)
        {
            case "Dungeon":
                ResolveDungeonChange();
                break;
            case "Home":
                ResolveHomeChange();
                break;
        }
    }

    private void ResolveDungeonChange()
    {
        character = null;
        InstantiateCharacter();
    }

    private void ResolveHomeChange()
    {
        character = null;
        InstantiateCharacter();
    }
}
