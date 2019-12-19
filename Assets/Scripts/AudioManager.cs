using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instanse;
    void Awake()
    {
        if (instanse == null)
            instanse = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound s in sounds)
        {
            s.sourse = gameObject.AddComponent<AudioSource>();
            s.sourse.clip = s.clip;
            s.sourse.volume = s.volume;
            s.sourse.pitch = s.pitch;
            s.sourse.loop = s.loop;
        }
    }
    public void Play(string name)
    {
        if (PlayerPrefs.GetInt("Sound", 0) != 0)
            return;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Wrong name of the sound");
            return;
        }
        s.sourse.Play();
        

    }
}