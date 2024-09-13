using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClips;
    [SerializeField]
    private AudioSource _seSource;
    [SerializeField]
    private AudioClip[] _seClips;
    private void Awake()
    {
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
    private void Update()
    {
        CountDownSound();
    }


    private void CountDownSound()
    {
        //Debug.Log((int)GameManager.Instance.CountdownTime);
        switch ((int)GameManager.Instance.CountdownTime)
        {
            case 7:

                break;
            case 6:

                break;
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
               // Debug.Log("Ç∑ÇΩÅ[Ç∆ÅIÅI");
                break;
        }
    }

    //public void PlayClip(int index)
    //{
    //    _audioSource.clip = _audioClips[index];
    //    _audioSource.Play();
    //}
    public void PlaySe(int index)
    {
        _seSource.clip = _seClips[index];
        _seSource.Play();
    }
}
