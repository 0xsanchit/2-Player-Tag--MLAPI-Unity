using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    public static AudioManager instance;
    public AudioSource audioSource;
    public Sound s1;
    // Start is called before the first frame update
    private void Awake()
    {  /*      
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);*/
        foreach(Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

/*            s.source = audioSource;
*/            s.source.clip = s.clip;

            s.source.volume = FindObjectOfType<Values>().Volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
/*        audioSource = gameObject.AddComponent<AudioSource>();
*/    }
    private void Update()
    {
        s1.source.volume = FindObjectOfType<Values>().Volume;
    }


    public void Play(string name)
    {
        Sound s = Array.Find(Sounds, Sound => Sound.name == name);
        if(s.name!="Mouse Click")
        {
            s1 = s;

        }
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "NotFound!");
            return;
        }
        s.source.volume = FindObjectOfType<Values>().Volume;
        s.source.Play(); 
        
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(Sounds, Sound => Sound.name == name);        

        s.source.Stop();
    }

}
