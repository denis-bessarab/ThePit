using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneThing : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private bool commandToCharactersManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            var op = SceneManager.LoadSceneAsync(sceneName);

            if (commandToCharactersManager) op.completed += _ => CharactersManager.Instance.GetCommandWhenSceneChanges(sceneName);
        }
    }
}
