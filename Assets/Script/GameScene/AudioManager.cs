using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]
    AudioSource _bgmSource;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AudioSource _seSource;
    [SerializeField]
    private AudioClip[] _seClips;
    private int _firstRun = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        CountDownSound();
        PlayBgm();
    }
    private void CountDownSound()
    {
        switch ((int)GameManager.Instance.CountdownTime)
        {
            case 5:
                PlaySe(0);
                break;
            case 4:
                //PlaySe(0);
                break;
            case 3:
                break;
            case 2:
                break;
            case 1:
                break;
            case 0:
                break;
        }
    }
    private void PlayBgm()
    {
        if (!GameManager.Instance.IsGameStart || _firstRun > 0) return;
        _bgmSource.clip = _audioClip;
        _bgmSource.Play();
        _bgmSource.loop = true;
        _firstRun = 1;
    }
    public void PlaySe(int index)
    {
        _seSource.loop = false;
        _seSource.clip = _seClips[index];
        _seSource.Play();
    }
}
