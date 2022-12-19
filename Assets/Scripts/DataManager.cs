using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst = null;    
    public Dictionary<string, SaveItem> ItemData = new Dictionary<string, SaveItem>();
    public Stat StatData;
    public int SceneNum;
    public Vector3 myPostion = Vector3.zero;
    public InventoryManager Inven_Equip;
    public InventoryManager Inven_Consume;
    public struct SaveItem
    {
        public ItemInfo itemInfo;
        public int Slotnum;
        public int ItemCount;
    }
    private void Awake()
    {
        if (Inst != null) Destroy(gameObject);
        Inst = this;
    }

    void AppointedInventory()//타입별 슬롯 저장.
    {
        InventoryManager[] invenM = UIManager.Inst.Inventory.GetComponentsInChildren<InventoryManager>();
        for (int i = 0; i < invenM.Length; ++i)
        {
            switch (invenM[i].name)
            {
                case "Equip":
                    Inven_Equip = invenM[i];
                    break;
                case "Consume":
                    Inven_Consume = invenM[i];
                    break;
                default:
                    break;
            }
        }
    }
    public void InputItemData(ItemInfo item)//데이타받고 슬롯 확인/저장.
    {
        //Inven_Equip.GetInsertableSlotNumber()
        //Inven_Equip.SlotCheck[num] = false;//슬롯 확인 빈슬롯 or 동일아이템
        //곂치는 아이템 없는경우(소비 다른템, 장비)
        //GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), Inven_Equip.Slots[num].transform) as GameObject;
        //obj.GetComponent<Item>().iteminfo = item.GetComponent<DropItem>().iteminfo;
        switch(item.type.ToString())
        {
            case "Equip":
                for (int i = 0; i < 21; ++i)
                {
                    if (!ItemData.ContainsKey(item.type.ToString() + $"{i}"))
                    {
                        SaveItem inputItem = new SaveItem();
                        inputItem.itemInfo = item;
                        ++inputItem.ItemCount;
                        ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        break;
                    }
                }
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {                    
                    if (!ItemData.ContainsKey(item.type.ToString() + $"{i}"))//비어있는경우
                    {
                        SaveItem inputItem = new SaveItem();
                        inputItem.itemInfo = item;
                        ++inputItem.ItemCount;
                        ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        break;
                    }
                    else
                    {
                        if (ItemData[item.type.ToString() + $"{i}"].itemInfo.ItemName == item.ItemName
                        && ItemData[item.type.ToString() + $"{i}"].ItemCount < 20)//같은이름의 아이템이 있는경우
                        {
                            SaveItem inputItem = ItemData[item.type.ToString() + $"{i}"];
                            ++inputItem.ItemCount;
                            ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        }
                    }
                }
                break;
            default:
                break;
        }                
        SaveItemData();//저장
    }
    void SaveItemData()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        AppointedInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
