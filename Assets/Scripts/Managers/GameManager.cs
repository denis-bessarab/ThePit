using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PGS_PrototypePauseMenu pauseMenu;
    private Coroutine findPauseMenuCorotine;

    private void Start()
    {
        if (pauseMenu != null || findPauseMenuCorotine != null ) return;
        findPauseMenuCorotine = StartCoroutine(FindPauseMenu());
    }

    private IEnumerator FindPauseMenu()
    {
        PGS_PrototypePauseMenu pauseMenu = null;

        while (pauseMenu == null)
        {
            pauseMenu = FindFirstObjectByType<PGS_PrototypePauseMenu>();
            yield return null;
        }

        this.pauseMenu = pauseMenu;
    }

    public void PauseUnpauseGame()
    {
        pauseMenu.Settings();
    }

    public static void FinishGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
