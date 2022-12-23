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
        if (eventData.pointerDrag.GetComponent<Item>())
        {
            if (GetComponentInChildren<HotKeyItem>() == null)
            {
                itemInfo = eventData.pointerDrag.GetComponent<Item>().iteminfo;
                GameObject obj = Instantiate(Resources.Load("Item/HotKeyItem"), transform) as GameObject;
                obj.GetComponent<HotKeyItem>().iteminfo = itemInfo;
                obj.GetComponent<HotKeyItem>().SetParent(this);
            }
        }
        else
        {
            HotKeyItem child = GetComponentInChildren<HotKeyItem>();
            HotKeyItem OndropIem = eventData.pointerDrag.GetComponent<HotKeyItem>();
            child?.SetParent(OndropIem.myParent);
            child.transform.parent.GetComponent<HotkeySlot>().itemInfo = child.iteminfo;
            OndropIem?.SetParent(this);
            itemInfo = OndropIem.iteminfo;
            eventData.pointerDrag.transform.SetParent(transform);
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
            DataManager.Inst.UseSlotItem(itemInfo);
            transform.GetComponentInChildren<HotKeyItem>().SetCountText();
        }
    }
    
}
