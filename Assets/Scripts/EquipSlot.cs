using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour,IDropHandler,IPointerClickHandler
{
    public ItemInfo itemInfo;
    public void OnDrop(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Eq_Slot");
        Item it = eventData.pointerDrag.GetComponent<Item>();
        if(it !=null && name == it.iteminfo.equiptype.ToString())//ÀåÂø°¡´É ½½·Ô ÆÇº°.
        {
            if (transform.GetComponentInChildren<EquipItem>() == null)//ºó½½·Ô¿¡ ÀåÂø.
            {
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
                Equipment(itemInfo.equiptype.ToString());

            }
            else//¾ÆÀÌÅÛ ±³Ã¼.
            {

            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount >=2)
        {

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Equipment(string equip)
    {
        switch(equip)
        {
            case "Weapon":
                DataManager.Inst.PlayerStatData.AttackPower += itemInfo.Value;
                UIManager.Inst.SetAbility("AttackPower");
                break;
            case "Shield":
                DataManager.Inst.PlayerStatData.DefensePower += itemInfo.Value;
                UIManager.Inst.SetAbility("DefensePower");
                break;
            default:
                break;
        }
    }
}
