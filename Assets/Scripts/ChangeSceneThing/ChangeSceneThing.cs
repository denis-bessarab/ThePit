using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneThing : MonoBehaviour
{

    [SerializeField] private string sceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
