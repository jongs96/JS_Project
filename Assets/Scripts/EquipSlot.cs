using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    public void OnDrop(PointerEventData eventData)
    {
        Item it = eventData.pointerDrag.GetComponent<Item>();
        if(it !=null && name == it.iteminfo.equiptype.ToString())//¿Â¬¯∞°¥… ΩΩ∑‘ ∆«∫∞.
        {
            ChangeImg("Sprite/Eq_Slot");
            if (transform.GetComponentInChildren<EquipItem>() == null)//∫ÛΩΩ∑‘ø° ¿Â¬¯.
            {
                it.IsDestroy = true;
                itemInfo = it.iteminfo;
                DataManager.Inst.UseSlotItem(it);//ªÁøÎ Ω∫≈›¡ı∞®√≥∏Æ
                GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
                obj.GetComponent<EquipItem>().iteminfo = itemInfo;
                obj.GetComponent<EquipItem>().SetParent(transform);
            }
            else//æ∆¿Ã≈€ ±≥√º.
            {

            }
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
    public void ChangeImg(string path)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
}
