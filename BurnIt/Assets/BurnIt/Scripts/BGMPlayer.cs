using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour {
    public Transform target;
    public AudioClip BGMClip;
	// Use this for initialization
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = target.position;
        }
	}

    AudioSource[] _bgmSources;
    void Awake()
    {
        _bgmSources = GetComponents<AudioSource>();
        if(_bgmSources != null)
        {
            foreach(var source in _bgmSources)
            {
                source.loop = true;
            }
        }
    }

    
    public void Play()
    {
        if(_bgmSources != null)
        {
            foreach(var source in _bgmSources)
            {
                source.Play();
            }
        }
    }

    public void Stop()
    {
        if (_bgmSources != null)
        {
            foreach (var source in _bgmSources)
            {
                source.Stop();
            }
        }
    }
}
