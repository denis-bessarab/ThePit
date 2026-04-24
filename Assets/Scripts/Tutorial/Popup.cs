using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    public void SetText(string message)
    {
        tmp.text = message;
    }

    public void ClosePopup()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }
}
