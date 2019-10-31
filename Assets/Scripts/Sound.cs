using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

#pragma warning disable 0649
[System.Serializable]
class Sound
{
    public string name;

    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    [Range(0, 1)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool looping;
}
#pragma warning restore 0649

