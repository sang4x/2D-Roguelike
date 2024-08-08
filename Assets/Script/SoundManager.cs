using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager intance = null;
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;
     void Awake()
    {
        if (intance==null)
        {
            intance = this;
        }
        else if (intance!=null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlayeSingle(AudioClip clip)
    {
        efxSource.clip = clip;
        efxSource.Play();
    }
    public void RamdomizeSfx(params AudioClip [] clips)
    {
        int radomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        efxSource.pitch = randomPitch;
        efxSource.clip = clips[radomIndex];
        efxSource.Play();
    }
}
