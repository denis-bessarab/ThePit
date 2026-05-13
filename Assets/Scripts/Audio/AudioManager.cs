using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private EventReference startMusicEvent;
    [SerializeField] private static Bus masterBus;
    [SerializeField] private bool musicStarted;
    [Range(0,1)]
    [SerializeField] public float masterVolume = 1;
    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        PlayMusic();
    }
    public void PlayMusic()
    {
        //if (musicStarted) return;
        //var instance = RuntimeManager.CreateInstance(startMusicEvent);
        //instance.start();
        //musicStarted = true;
    }

    public void ChangeMasterVolume(float volume)
    {
        masterVolume = volume;
        masterBus.setVolume(volume);
    }
}
