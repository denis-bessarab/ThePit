using UnityEngine;

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
}
