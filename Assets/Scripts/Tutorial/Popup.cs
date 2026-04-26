using TMPro;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClosePopup();
        }
    }
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
