using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField]
    private Image _fadeImage;
    public static SceneTransitionManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(_fadeImage);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; //���[�h������ɏ���ɃV�[���ڍs����Ȃ��悤��false�ɂ��Ƃ�
        float fadeDuration = 3.0f;
        float timer = 0;
        while (timer < fadeDuration) //�t�F�[�h�A�E�g������
        {
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            if (_fadeImage != null) { _fadeImage.color = new Color(0, 0, 0, alpha); }
            else { Debug.Log("�C���[�Wnull"); }
            timer += Time.deltaTime;
            yield return null;
        }
        while (asyncLoad.progress < 0.9f) //�t�F�[�h�A�E�g�I����Ƀ��[�h�����܂ő҂�
        {
            Debug.Log($"���[�h��...{asyncLoad.progress}");
            yield return null;
        }
        if (_fadeImage != null) { _fadeImage.enabled = false; }
        else { Debug.Log("�C���[�Wnull"); }
        asyncLoad.allowSceneActivation = true; //�V�[���ڍs
    }
}
