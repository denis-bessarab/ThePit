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
        if(AudioManager.masterVolume != slider.value)
        {
            slider.value = AudioManager.masterVolume;
        }
    }

    public void ChangeMasterValue(float value)
    {
        AudioManager.ChangeMasterVolume(value);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
