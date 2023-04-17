using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private AudioSource music;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.source.clip.name == "BreslinsMainMenu")
                music = s.source;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectOfType<AudioManager>() != this)
            Destroy(gameObject);

        if (!music.isPlaying)
            Play("MenuMusic");
    }

    void Update()
    {
        music.volume = UI.musicSlider.value;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level != 0)
            UI.musicSlider.value = 0;
        else
            UI.musicSlider.value = 0.5f;
    }
}
