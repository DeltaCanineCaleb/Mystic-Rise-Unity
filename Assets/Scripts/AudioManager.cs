using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] musicTracks;
    public Sound[] soundEffects;
    Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        sounds = musicTracks.Concat(soundEffects).ToArray();
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>(); 
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
    
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainGame")
            Play("Temo Village");
    }

    public void StopAll () 
    {
        foreach (Sound s in sounds) {
            s.source.Stop();
        }
    }

    public void Play (string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound '" + name + "' not found!");
            return;
        }
        s.source.Play();
    }
}
