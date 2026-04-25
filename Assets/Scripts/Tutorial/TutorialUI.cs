using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject popupPrefab; 
    public void SpawnPopup(string message)
    {
        Time.timeScale = 0f;
        var popup = Instantiate(popupPrefab,transform);
        var popusScript = popup.GetComponent<Popup>();
        popusScript.SetText(message);
    }

    public void SkipTutorial()
    {
        SceneManager.LoadScene("PrototypeLVL1");
    }
}
