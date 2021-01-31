using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager Instance { get; private set; }
    private Sound currentTheme;

    #endregion
    
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        //TODO: наверное убрать это
        // if (Instance == null)
        // {
            Instance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        //DontDestroyOnLoad(gameObject);
        
        foreach (var sound in sounds)
        {
           sound.source = gameObject.AddComponent<AudioSource>();
           sound.source.clip = sound.clip;
           sound.source.volume = sound.volume;
           sound.source.pitch = sound.pitch;
           sound.source.loop = sound.loop;
           sound.source.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            s.source.Play();
        }
    }

    public void PlayTheme(string name)
    {
        if (currentTheme != null)
        {
            currentTheme.source.Stop();
        }
        Sound theme = Array.Find(sounds, sound => sound.name == name);
        if (theme == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        theme.source.Play();
        currentTheme = theme;
    }
}
