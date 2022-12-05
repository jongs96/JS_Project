using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialWindow;
    public Player player;
    TMPro.TMP_Text tuto_script;

    // Start is called before the first frame update
    void Start()
    {
        tuto_script = TutorialWindow.GetComponentInChildren<TMPro.TMP_Text>();
        StartCoroutine(Tutorial(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Tutorial(int step)
    {
        switch(step)
        {
            //상하좌우 이동
            case 0:
                
                break;
            //달리기 - 달린상태 3.0초 채우기
            case 1:
                break;
            //앉기 check
            case 2:
                break;
            //방어하기 check
            case 3:
                break;
            //공격
            case 4:
                break;
            //G눌러 이동
            case 5:
                break;
            //잡몹 제거, 아이템 습득
            case 6:
                break;
            //단축기 등록 아이템 사용
            case 7:
                break;
            //보스 처치 게임 클리어
            case 8:
                break;
            default:
                break;
        }

        yield return null;
    }
    void SetTutoText(string text)
    {
        tuto_script.text = text;
    }
}
