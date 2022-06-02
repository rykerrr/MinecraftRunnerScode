using System;
using System.Collections.Generic;
using System.Linq;
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

    private List<Sound> sounds;

    protected void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            sounds = Resources.LoadAll<AudioSource>("Audio/Sources").Select(x => new Sound()
            {
                clip = x.clip,
                looping = x.loop,
                name = x.name,
                pitch = x.pitch,
                source = null,
                volume = x.volume
            }).ToList();
            
            Debug.Log(sounds.Count);
            sounds.ForEach(Debug.Log);
        }

        foreach (Sound s in sounds)
        {
            s.source = new GameObject(s.name).AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
            
            s.source.transform.SetParent(transform);
        }

    }

    public void PlayAudio(string soundName)
    {
        Debug.Log("being asked 2 play dis");
        
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
