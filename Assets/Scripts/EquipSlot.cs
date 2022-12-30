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
        if(it !=null && name == it.iteminfo.equiptype.ToString())//�������� ���� �Ǻ�.
        {
            ChangeImg("Sprite/Eq_Slot");
            EquipItem eqitem = transform.GetComponentInChildren<EquipItem>();
            if (eqitem == null)//�󽽷Կ� ����.
            {
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                DataManager.Inst.UseSlotItem(it);//��� ��������ó��
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
            }
            else//������ ��ü.
            {
                ItemInfo eqitemInfo = eqitem.iteminfo;//����������
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                DataManager.Inst.UseSlotItem(it);//��� ��������ó��
                Destroy(eqitem.gameObject);
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
                DataManager.Inst.InputItemData(eqitemInfo);//��ü�Ǵ� ������ dictionary�� ����
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
        ChangeImg("Sprite/Eq_Slot");
        itemInfo = GetComponentInChildren<EquipItem>().iteminfo;        
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
        if (GetComponentInChildren<EquipItem>())//������ ��� �ִ��� Ȯ��.
        {

            Destroy(objEquipPos.GetChild(0).gameObject);
        }
        GameObject obj = Instantiate(itemInfo.Resource, objEquipPos);
        EqItemInfo objtransInfo = Resources.Load<EqItemInfo>($"Item/Data/{itemInfo.ItemName}");
        obj.transform.localPosition = objtransInfo.EquipPostion;
        obj.transform.localEulerAngles = objtransInfo.EquipRotation;
        Player.Inst.AttackPos = objEquipPos.GetChild(0).GetChild(0);
    }
    public void ChangeImg(string path)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
