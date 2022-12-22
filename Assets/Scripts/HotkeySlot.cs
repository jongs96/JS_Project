using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotkeySlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    int slotNum;
    public void OnDrop(PointerEventData eventData)
    {
        itemInfo = eventData.pointerDrag.GetComponent<Item>().iteminfo;
        if (GetComponentInChildren<HotKeyItem>() == null)
        {
            GameObject obj = Instantiate(Resources.Load("Item/HotKeyItem"), transform) as GameObject;
            obj.GetComponent<HotKeyItem>().iteminfo = itemInfo;
        }
    }    
    // Start is called before the first frame update
    void Start()
    {
        slotNum = int.Parse(name[name.Length - 1].ToString());        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown((KeyCode)(slotNum+48)) && itemInfo != null)
        {
            UseSlotItem();
        }
    }
    void UseSlotItem()
    {
        DataManager.Inst.OutPutItemData(itemInfo);        
    }
}
