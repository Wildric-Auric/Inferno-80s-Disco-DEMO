using UnityEngine.Audio;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Sound", menuName ="Sound")]
[Serializable]
public class Sound : ScriptableObject 
{
    public AudioClip audioClip;
    public string Name;
    [Range(0, 1)] public float volume;
    [Range(-3, 3)] public float pitch;
    [Range(0,1)] public float spacial;
    [HideInInspector] public AudioSource audioSource;
    public bool loop;
    public bool awake;

}
