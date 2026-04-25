using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private EventReference startMusicEvent;
    [SerializeField] private bool musicStarted;
    private void Start()
    {
        PlayMusic();
    }
    public void PlayMusic()
    {
        if (musicStarted) return;
        var instance = RuntimeManager.CreateInstance(startMusicEvent);
        instance.start();
        musicStarted = true;
    }
}
