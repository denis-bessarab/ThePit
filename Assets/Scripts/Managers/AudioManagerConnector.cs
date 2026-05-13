using UnityEngine;
using UnityEngine.UI;

public class AudioManagerConnector : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        StartCoroutine(GameManager.FindPauseMenu());
    }
    private void LateUpdate()
    {
        if (AudioManager.Instance == null) return;

        if(AudioManager.Instance.masterVolume != slider.value)
        {
            slider.value = AudioManager.Instance.masterVolume;
        }
    }

    public void ChangeMasterValue(float value)
    {
        AudioManager.Instance.ChangeMasterVolume(value);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
