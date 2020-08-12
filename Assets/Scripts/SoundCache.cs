using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCache : MonoBehaviour
{
    public static Dictionary<string, AudioClip> cache = new Dictionary<string, AudioClip>();
    private static SoundCache instance = null;

    public AudioSource audioSource = null;

    public static SoundCache GetInstance()
    {
        return SoundCache.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SoundCache.instance != null)
        {
            throw new System.Exception("SoundCache is singleton");
        }

        SoundCache.instance = this;
        SoundCache.cache.Add("tirin1", Resources.Load<AudioClip>("Sound/tirin1"));
    }

    public AudioClip GetCache(string name)
    {
        return SoundCache.cache[name];
    }
}
