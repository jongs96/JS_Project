using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotkeySlot : MonoBehaviour,IDropHandler
{
    public ItemInfo itemInfo;
    int itemCount;
    public void OnDrop(PointerEventData eventData)
    {
        itemInfo = eventData.pointerDrag.GetComponent<Item>().iteminfo;
        itemCount = GetTotalNumberOfItems(itemInfo.ItemName);
        GameObject obj = Instantiate(Resources.Load("Item/HotKeyItem"), transform) as GameObject;
        obj.GetComponent<HotKeyItem>().iteminfo = itemInfo;
        obj.GetComponent<HotKeyItem>().count = itemCount;
    }
    int GetTotalNumberOfItems(string itemName)
    {
        int Count = 0, i = 0;
        
        while (DataManager.Inst.ItemData.ContainsKey("Consume" + $"{i})"))
        {
            if(DataManager.Inst.ItemData["Consume" + $"{i})"].itemInfo.ItemName == itemName)
            {
                Count += DataManager.Inst.ItemData["Consume" + $"{i})"].ItemCount;
            }
        }
        return Count;
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
