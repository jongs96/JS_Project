using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    public Transform objEquipPos = null;
    public void OnDrop(PointerEventData eventData)
    {
        Item it = eventData.pointerDrag.GetComponent<Item>();
        if(it !=null && name == it.iteminfo.equiptype.ToString())//장착가능 슬롯 판별.
        {
            ChangeImg("Sprite/Eq_Slot");
            EquipItem eqitem = transform.GetComponentInChildren<EquipItem>();
            if (eqitem == null)//빈슬롯에 장착.
            {
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                DataManager.Inst.UseSlotItem(it);//사용 스텟증감처리
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
            }
            else//아이템 교체.
            {
                ItemInfo eqitemInfo = eqitem.iteminfo;//이전템정보
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                DataManager.Inst.UseSlotItem(it);//사용 스텟증감처리
                Destroy(eqitem.gameObject);
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
                DataManager.Inst.InputItemData(eqitemInfo);//교체되는 아이템 dictionary에 전달
                GameObject beforeObj = Instantiate(Resources.Load("Item/SlotItem"), DataManager.Inst.EquipSlots[it.mySlotNum]) as GameObject;
                beforeObj.GetComponent<Item>().iteminfo = eqitemInfo;
                beforeObj.GetComponent<Item>().ItemCount = it.ItemCount;
                beforeObj.GetComponent<Item>().mySlotNum = it.mySlotNum;
            }
            EquipmentObj();
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
    public void Equipment(string equip, bool Increase)
    {
        switch(equip)
        {
            case "Weapon":
                if(Increase) DataManager.Inst.PlayerStatData.AttackPower += itemInfo.Value;
                else DataManager.Inst.PlayerStatData.AttackPower -= itemInfo.Value;
                UIManager.Inst.SetAbility("AttackPower");
                break;
            case "Shield":
                if(Increase) DataManager.Inst.PlayerStatData.DefensePower += itemInfo.Value;
                else DataManager.Inst.PlayerStatData.DefensePower -= itemInfo.Value;
                UIManager.Inst.SetAbility("DefensePower");
                break;
            default:
                break;
        }
    }
    public void EquipmentObj()
    {
        if (objEquipPos.childCount != 0)//장착된 장비가 있는지 확인.
        {
            Destroy(objEquipPos.GetChild(0).gameObject);
        }
        GameObject obj = Instantiate(itemInfo.Resource, objEquipPos);
        EqItemInfo objtransInfo = Resources.Load<EqItemInfo>($"Item/Data/{itemInfo.ItemName}");
        obj.transform.localPosition = objtransInfo.EquipPostion;
        obj.transform.localEulerAngles = objtransInfo.EquipRotation;
        if (name == "Weapon")
        {
            Player.Inst.GetComponentInChildren<AnimEventFuc>().AttackRange = objtransInfo.Range;
        }
        Player.Inst.AttackPos = obj.transform.GetChild(0);
    }
    public void ChangeImg(string path)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
