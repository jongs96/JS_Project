using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    public bool[] SlotCheck = new bool[28];
    public Transform myParents;
    //public int order
    // Start is called before the first frame update
    void Start()
    {
        myParents = transform.parent;
        for(int i = 0; i < SlotCheck.Length; ++i)//initialize slotcheck
        {
            SlotCheck[i] = true;
        }
        for(int i = 0; i < transform.childCount; ++i)
        {
            Slots.Add(transform.GetChild(i).gameObject);
        }
    }

    public int GetInsertableSlotNumber(GameObject item)//ºó½½·Ô, ÈÄ¥ ¼ö ÀÖ´ÂÅÛ ÆÇº°
    {
        string itemType = item.GetComponent<DropItem>().iteminfo.type.ToString();
        switch(itemType)
        {
            case "Equip":
                for (int i = 0; i < SlotCheck.Length; ++i)
                {
                    if (SlotCheck[i])
                    {
                        return i;
                    }
                }
                return -1;
            case "Consume":
                for (int i = 0; i < SlotCheck.Length; ++i)
                {
                    if (Slots[i].GetComponentInChildren<Item>().iteminfo.ItemName == item.GetComponent<DropItem>().iteminfo.ItemName
                        && Slots[i].GetComponentInChildren<Item>().ItemCount < Slots[i].GetComponentInChildren<Item>().iteminfo.MaxCount)
                    {
                        ++Slots[i].GetComponentInChildren<Item>().ItemCount;
                        return -1;
                    }
                    if (SlotCheck[i])
                    {
                        return i;
                    }
                }
                return -1;
            default:
                return -1;
        }
    }
    
    public void ChangeChildLocation()
    {
        Button[] buttons = UIManager.Inst.Inventory.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; ++i)
        {
            if (buttons[i].name == gameObject.name)
            {
                buttons[i].transform.SetAsLastSibling();
                break;
            }
        }
        Transform screen = myParents.Find("__Screen__");
        screen.SetAsLastSibling();
        transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
