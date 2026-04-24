using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform characterTransform;
    private Coroutine findCharacterCoroutine;

    private void Awake()
    {
        if (findCharacterCoroutine != null) return;
        findCharacterCoroutine = StartCoroutine(FindCharacter());
    }

    void LateUpdate()
    {
        if (characterTransform == null) return;
        FollowObject();
    }

    private void FollowObject()
    {
        transform.position = new Vector3(characterTransform.position.x, characterTransform.position.y, -10);
    }

    private IEnumerator FindCharacter()
    {
        GameObject character = null;

        while (character == null)
        {
            character = GameObject.Find("Character");
            yield return null;
        }

        characterTransform = character.transform;
    }
}
