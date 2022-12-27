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
        Item it = eventData.pointerDrag.GetComponent<Item>();
        if(it !=null)
        {
            itemInfo = it.iteminfo;
            GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), transform) as GameObject;
            obj.GetComponent<Image>().sprite = Resources.Load("Sprite/" + itemInfo.ItemName) as Sprite;
            DataManager.Inst.PlayerStatData.AttackPower += itemInfo.Value;
            UIManager.Inst.SetAbility("AttackPower");
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
}
