using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotkeySlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    public List<Item> connectIT = new List<Item>();
    int slotNum;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Item>() != null)
        {
            if (GetComponentInChildren<HotKeyItem>() == null)//����Ű�� ����.
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
            HotKeyItem OndropItem = eventData.pointerDrag.GetComponent<HotKeyItem>();
            child?.SetParent(OndropItem.myParent);
            child.transform.parent.GetComponent<HotkeySlot>().itemInfo = child.iteminfo;
            child.GetComponentInParent<HotkeySlot>().ConnectItem();
            OndropItem?.SetParent(this);
            itemInfo = OndropItem.iteminfo;
            ConnectItem();
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
            DataManager.Inst.UseSlotItem(connectIT[0]);//
            transform.GetComponentInChildren<HotKeyItem>().SetCountText();
        }
    }
    public void ConnectItem()//����Ʈ�� �ش� �����۵��� �߰�.
    {
        connectIT.Clear();
        Item[] items = DataManager.Inst.Inven_Consume.GetComponentsInChildren<Item>();
        for (int i = 0; i < items.Length; ++i)
        {
            if (items[i].iteminfo.ItemName == itemInfo.ItemName)
            {
                connectIT.Add(items[i]);
            }
        }
    }
}
