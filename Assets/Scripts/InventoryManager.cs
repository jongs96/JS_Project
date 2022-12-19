using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    public bool[] SlotCheck = new bool[21];
    public Transform myParents;
    //public int order
    private void Awake()
    {
        myParents = transform.parent;
        for (int i = 0; i < SlotCheck.Length; ++i)//initialize slotcheck
        {
            SlotCheck[i] = true;
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            Slots.Add(transform.GetChild(i).gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetInsertableSlotNumber(GameObject item)//빈슬롯, 곂칠 수 있는템 판별
    {        
        switch(item.GetComponent<DropItem>().iteminfo.type)
        {
            case ItemInfo.ItemType.Equip:
                for (int i = 0; i < SlotCheck.Length; ++i)
                {
                    if (SlotCheck[i])
                    {
                        return i;
                    }
                }
                return -1;
            case ItemInfo.ItemType.Consume:
                for (int i = 0; i < SlotCheck.Length; ++i)
                {
                    Item initem = Slots[i].GetComponentInChildren<Item>();
                    if (initem != null)
                    {
                        if (initem.iteminfo.ItemName == item.GetComponent<DropItem>().iteminfo.ItemName
                        && initem.ItemCount < Slots[i].GetComponentInChildren<Item>().iteminfo.MaxCount)
                        {
                            ++Slots[i].GetComponentInChildren<Item>().ItemCount;
                            return -1;
                        }
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
    private void OnEnable()
    {
        //모든 아이템 Destory 후 생성. or 비교하고 다른거만 파괴 후 생성.
        for(int i = 0; i < Slots.Count; ++i)
        {
            GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), Slots[i].transform) as GameObject;
            obj.GetComponent<Item>().iteminfo = DataManager.Inst.ItemData[gameObject.name + $"{i}"].itemInfo;
            obj.GetComponent<Item>().ItemCount= DataManager.Inst.ItemData[gameObject.name + $"{i}"].ItemCount;
        }        
    }    
    // Update is called once per frame
    void Update()
    {
        
    }
}
