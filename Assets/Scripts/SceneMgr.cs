using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr Inst = null;
    public Slider LoadingBar;

    private void Awake()
    {
        if(Inst != null)
        {
            Destroy(gameObject);
            Destroy(LoadingBar);
        }
        Inst = this;
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
}
