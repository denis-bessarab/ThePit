using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public static PGS_PrototypePauseMenu pauseMenu;
    private Coroutine findPauseMenuCorotine;

    private void Start()
    {
        if (pauseMenu != null || findPauseMenuCorotine != null ) return;
        findPauseMenuCorotine = StartCoroutine(FindPauseMenu());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            var ia = Resources.Load("InputSystem_Actions") as InputActionAsset;
            for(int i = 0; i < ia.actionMaps.Count; i++)
            {
                Debug.Log($"{ia.actionMaps[i].name} {ia.actionMaps[i].enabled}");
            }
        }
    }

    public static IEnumerator FindPauseMenu()
    {
        PGS_PrototypePauseMenu pauseMenu = null;

        while (pauseMenu == null)
        {
            pauseMenu = FindFirstObjectByType<PGS_PrototypePauseMenu>();
            yield return null;
        }

        GameManager.pauseMenu = pauseMenu;
    }

    public void PauseUnpauseGame()
    {
        if (pauseMenu == null) return;
        pauseMenu.Settings();
    }

    public static void FinishGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
