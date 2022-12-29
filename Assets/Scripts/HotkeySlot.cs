using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotkeySlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    public List<Item> connectIT = new List<Item>();
    Item it = null;
    int slotNum;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Item>() != null)
        {
            if (GetComponentInChildren<HotKeyItem>() == null)//단축키가 빈경우.
            {
                itemInfo = eventData.pointerDrag.GetComponent<Item>().iteminfo;
                GameObject obj = Instantiate(Resources.Load("Item/HotKeyItem"), transform) as GameObject;
                obj.GetComponent<HotKeyItem>().iteminfo = itemInfo;
                obj.GetComponent<HotKeyItem>().SetParent(this);
                ConnectItem();
            }
        }
        else if(GetComponentInChildren<HotKeyItem>())
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
            DataManager.Inst.UseSlotItem(it);
            transform.GetComponentInChildren<HotKeyItem>().SetCountText();
        }
    }
    void ConnectItem()//리스트에 해당 아이템들을 추가.
    {
        for (int i = 0; i < 21; ++i)
        {
            if (DataManager.Inst.ItemData[$"{itemInfo.type}{i}"].itemInfo == itemInfo)
            {
                break;
            }
        }
    }
}
