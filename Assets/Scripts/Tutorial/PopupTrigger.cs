using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    [SerializeField] private TutorialUI tutorialUI;
    [TextArea(3,10)]
    [SerializeField] private string message;

    private void Awake()
    {
        tutorialUI = FindFirstObjectByType<TutorialUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Character")
        {
            if (tutorialUI == null) return;
            tutorialUI.SpawnPopup(message);
            Destroy(gameObject);
        }
    }
}
