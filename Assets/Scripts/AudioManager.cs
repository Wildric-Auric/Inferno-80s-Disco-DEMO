using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private void Start()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.pitch = s.pitch;
            s.audioSource.volume = s.volume;
            s.audioSource.spatialBlend = s.spacial;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.awake;
        }
    }
    public string PlaySoundByIndex(int index)
    {
        Sound target = sounds[index];
        if (target != null)
        {
            target.audioSource.Play();
            return target.Name;
        }
        return "";
    }

    public void PlaySound(string name)
    { 
        Sound target = Array.Find(sounds, sound => sound.Name == name);
        if (target != null)
        {
            target.audioSource.Play();
        }
    }
    public void PlaySoundWithpitch(string name, float bitch, float vol)
    {
        
        Sound target = Array.Find(sounds, sound => sound.Name == name);
        if (target != null)
        {
            
            target.audioSource.pitch = bitch;
            target.audioSource.volume = vol;
            target.audioSource.Play();
            
        }
    }

    public void StopSound(string name)
    {
        Sound target = Array.Find(sounds, sound => sound.Name == name);
        if (target != null)
        {
            target.audioSource.Stop();
        }
    }

    public bool IsPlaying(string name)
    {
        Sound target = Array.Find(sounds, sound => sound.Name == name);
        if (target != null)
        {
            return target.audioSource.isPlaying;
        }
        return false;
    }
    public AudioSource GetAudioSource(string name)
    {
        Sound target = Array.Find(sounds, sound => sound.Name == name);
        if (target != null)
        {
            return target.audioSource;
        }
        return null;
    }
}
