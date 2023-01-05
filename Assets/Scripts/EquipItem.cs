using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipItem : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo iteminfo = null;
    public Transform myParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)//장착해제
        {
            string path = $"Sprite/{transform.parent.name}_Slot";
            transform.GetComponentInParent<EquipSlot>().ChangeImg(path);
            if (GetComponentInParent<EquipSlot>().objEquipPos.childCount != 0)//장착된 장비가 있는지 확인.
            {
                Destroy(GetComponentInParent<EquipSlot>().objEquipPos.GetChild(0).gameObject);
            }
            Destroy(gameObject);
            if (transform.parent.name == "Weapon")
            {
                Player.Inst.GetComponentInChildren<AnimEventFuc>().AttackRange = 0.2f;
            }
            DataManager.Inst.InputItemData(iteminfo);
            GetComponentInParent<EquipSlot>().Equipment(iteminfo.equiptype.ToString(), false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprite/{iteminfo.ItemName}");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetParent(Transform newSlot)
    {
        myParent = newSlot;
        transform.SetParent(myParent.transform);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
