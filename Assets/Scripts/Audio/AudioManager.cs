using FMODUnity;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private EventReference startMusicEvent;
    private void Start()
    {
        PlayMusic();
    }
    public void PlayMusic()
    {
        RuntimeManager.PlayOneShot(startMusicEvent);
    }
}
