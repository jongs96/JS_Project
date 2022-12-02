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
                return "Esc를 누르면 게임을 일시정지 할 수 있습니다.";
            case 1:
                return "R을 누르고 마우스를 움직이면 화면만 회전합니다.";
            case 2:
                return "Shift를 누르면 달릴 수 있습니다.";
            case 3:
                return "포션을 소비하여 체력을 회복할 수 있습니다.";
            case 4:
                return "i를 누르면 인벤토리를 확인 할 수 있습니다.";
            case 5:
                return "u를 누르면 유저 정보를 확인 할 수 있습니다.";
            case 6:
                return "인벤토리의 아이템을 퀵슬롯에 등록하여 편하게사용하세요!";
            case 7:
                return "몬스터를 잡고 나온 아이템은 가까이 다가가면 습득됩니다.";
            case 8:
                return "보스를 처치하고 던전을 탈출하세요!";
            case 9:
                return "Space Bar를 연속으로 누르면 구를 수 있습니다.";
            default:
                return " ";
        }
    }
}
