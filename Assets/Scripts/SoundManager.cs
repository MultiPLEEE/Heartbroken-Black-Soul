using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    private AudioSource _musicSource;
    private AudioSource _seSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.spatialBlend = 0f;

        _seSource = gameObject.AddComponent<AudioSource>();
        _seSource.spatialBlend = 0f;
    }
    
    public void SetMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
    
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        _seSource.PlayOneShot(clip, volume);
    }
}