using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomLoading : MonoBehaviour
{
    public TMPro.TMP_Text myTip;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Image/Loading_img{Random.Range(0, 3)}");
        myTip.text += SelectTip();
    }

    string SelectTip()
    {
        int num = Random.Range(0, 10);
        switch(num)
        {
            case 0:
                return "Esc�� ������ ������ �Ͻ����� �� �� �ֽ��ϴ�.";
            case 1:
                return "R�� ������ ���콺�� �����̸� ȭ�鸸 ȸ���մϴ�.";
            case 2:
                return "Shift�� ������ �޸� �� �ֽ��ϴ�.";
            case 3:
                return "������ �Һ��Ͽ� ü���� ȸ���� �� �ֽ��ϴ�.";
            case 4:
                return "i�� ������ �κ��丮�� Ȯ�� �� �� �ֽ��ϴ�.";
            case 5:
                return "u�� ������ ���� ������ Ȯ�� �� �� �ֽ��ϴ�.";
            case 6:
                return "�κ��丮�� �������� �����Կ� ����Ͽ� ���ϰԻ���ϼ���!";
            case 7:
                return "���͸� ��� ���� �������� ������ �ٰ����� ����˴ϴ�.";
            case 8:
                return "������ óġ�ϰ� ������ Ż���ϼ���!";
            case 9:
                return "Space Bar�� �������� ������ ���� �� �ֽ��ϴ�.";
            default:
                return " ";
        }
    }
}
