using System;
using UnityEngine;
using UnityEngine.Audio;

#pragma warning disable 0649
public class SoundManager : MonoBehaviour
{
    #region Singleton
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    instance = new GameObject("SoundMngr", typeof(SoundManager)).GetComponent<SoundManager>();

                    if (instance == null)
                    {
                        throw new Exception("Soundmngr can't be spawned in...");
                    }

                }

            }

            return instance;
        }
    }
    #endregion

    [SerializeField] private Sound[] sounds;

    protected void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
        }

    }

    public void PlayAudio(string soundName)
    {
        Sound s = null;

        foreach(Sound sound in sounds)
        {
            if(sound.name == soundName)
            {
                s = sound;
            }
        }

        s?.source.Play();
    }

    public void StopAudio(string soundName)
    {
        Sound s = null;

        foreach (Sound sound in sounds)
        {
            if (sound.name == soundName)
            {
                s = sound;
            }
        }

        s?.source.Stop();
    }
}
#pragma warning restore 0649
