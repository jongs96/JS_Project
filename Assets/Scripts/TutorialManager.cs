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
            //�����¿� �̵�
            case 0:
                
                break;
            //�޸��� - �޸����� 3.0�� ä���
            case 1:
                break;
            //�ɱ� check
            case 2:
                break;
            //����ϱ� check
            case 3:
                break;
            //����
            case 4:
                break;
            //G���� �̵�
            case 5:
                break;
            //��� ����, ������ ����
            case 6:
                break;
            //����� ��� ������ ���
            case 7:
                break;
            //���� óġ ���� Ŭ����
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
