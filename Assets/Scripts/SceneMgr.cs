using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{
    static SceneMgr _inst = null;
    public static SceneMgr Inst
    {
        get
        {
            if (_inst == null)
            {
                _inst = FindObjectOfType<SceneMgr>();
                if (_inst == null)
                {
                    _inst = (new GameObject("SceneLoader")).AddComponent<SceneMgr>();
                }
            }
            return _inst;
        }
    }
    public bool isNewGame;
    public Slider LoadingBar;

    private void Awake()
    {
        if(_inst != null)
        {
            Destroy(gameObject);
            Destroy(LoadingBar);
        }
        _inst = this;
        DontDestroyOnLoad(gameObject);
    }
    public void QuickMoveScene(int SceneNum)
    {
        SceneManager.LoadScene(SceneNum);
    }
    public void MoveSceneToLoad(int SceneNum)
    {
        StartCoroutine(Loading(SceneNum));
    }
    IEnumerator Loading(int SceneNum)
    {
        yield return SceneManager.LoadSceneAsync("Loading");
        LoadingBar.gameObject.SetActive(true);
        LoadingBar.value = 0.0f;
        StartCoroutine(LoadingScene(SceneNum));
    }
    IEnumerator LoadingScene(int SceneNum)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(SceneNum);
        ao.allowSceneActivation = false;
        while(!ao.isDone)
        {
            LoadingBar.value = ao.progress / 0.9f;
            if(ao.progress >= 0.9f)
            {
                LoadingBar.gameObject.SetActive(false);
                ao.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    
    public void IsNewGame(bool check)
    {
        isNewGame = check;
    }
}
