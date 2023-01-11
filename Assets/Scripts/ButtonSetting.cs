using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    public Button NewGame;
    public Button LoadGame;
    public Button ExitYes;
    // Start is called before the first frame update
    void Start()
    {
        NewGame.onClick.RemoveAllListeners();
        NewGame.onClick.AddListener(() => SceneMgr.Inst.MoveSceneToLoad(2));
        NewGame.onClick.AddListener(() => SceneMgr.Inst.IsNewGame(true));
        LoadGame.onClick.RemoveAllListeners();
        LoadGame.onClick.AddListener(() => SceneMgr.Inst.MoveSceneToLoad(2));
        LoadGame.onClick.AddListener(() => SceneMgr.Inst.IsNewGame(false));
        ExitYes.onClick.RemoveAllListeners();
        ExitYes.onClick.AddListener(() => SceneMgr.Inst.ExitGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
