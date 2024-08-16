using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClips;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayClip(int index)
    {
        _audioSource.clip = _audioClips[index];
        _audioSource.Play();
    }
}
