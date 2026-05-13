using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roster : MonoBehaviour
{
    [SerializeField] private List<Vector3> spawnPoints;
    [SerializeField] private List<GameObject> rosterCharacters;

    private void Awake()
    {
        StartCoroutine(GetDataFromManager());
    }

    private IEnumerator GetDataFromManager()
    {
        while (CharactersManager.Instance == null) yield return null;

        while (!CharactersManager.Instance.isDataReady)
        {
            yield return null;
        }

        ConstructRoster();
    }

    private void ConstructRoster()
    {
        GenerateSpawnPoints();
        GenerateRosterCharacters();
    }

    private void GenerateSpawnPoints()
    {
        var amount = CharactersManager.Instance.charactersAmount;

        for (int i = 0; i < amount; i++)
        {
            var newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            newPos.x += 2 * i;
            spawnPoints.Add(newPos);
        }
    }

    private void GenerateRosterCharacters()
    {
        var rosterCharacterPrefab = Resources.Load<GameObject>("Prefabs/RosterCharacter");
        var amount = CharactersManager.Instance.charactersAmount;
        var availableCharacters = CharactersManager.Instance.charactersData;
        var activeCharacter = CharactersManager.Instance.activeCharacter;



        for (int i = 0; i < amount; i++)
        {

            if (availableCharacters[i].id == activeCharacter.id) continue;


            var rosterCharacter = Instantiate(rosterCharacterPrefab, transform);
            rosterCharacters.Add(rosterCharacter);
            rosterCharacter.name = "RosterCharacter";
            rosterCharacter.transform.position = spawnPoints[i];
            var rc = rosterCharacter.GetComponent<RosterCharacter>();
            rc.CharacterData = availableCharacters[i];
        }
    }

    public void CleanRoster()
    {
        for(int i = rosterCharacters.Count - 1; i >= 0; i--)
        {
            Destroy(rosterCharacters[i]);
        }

        rosterCharacters.Clear();
    }


    public void UpdateRoster()
    {
        CleanRoster();
        GenerateRosterCharacters();
    }
}
