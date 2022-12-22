using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst = null;    
    public Dictionary<string, SaveItem> ItemData = new Dictionary<string, SaveItem>();
    public Dictionary<string, int> ItemTotalCount = new Dictionary<string, int>();
    public Stat PlayerStatData;

    public int SceneNum;
    public Vector3 myPostion = Vector3.zero;
    public Inventory Inven_Equip;
    public Inventory Inven_Consume;
    public UnityAction setSlotCount;

    List<Transform> EquipSlots = new List<Transform>();
    List<Transform> ConsumeSlots = new List<Transform>();
    public struct SaveItem
    {
        public ItemInfo itemInfo;
        public int Slotnum;
        public int ItemCount;
        public SaveItem(ItemInfo iteminfo, int slotnum)
        {
            itemInfo = iteminfo;
            Slotnum = slotnum;
            ItemCount = 0;
        }
    }
    private void Awake()
    {
        if (Inst != null) Destroy(gameObject);
        Inst = this;
    }

    void AppointedInventory()//타입별 inventory 저장.
    {
        Inventory[] invenM = UIManager.Inst.Inventory.GetComponentsInChildren<Inventory>();
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
    void GetInventorySlots(List<Transform> Label, Inventory type)//타입별 슬롯 저장.
    {        
        for (int i = 0; i < type.transform.childCount; ++i)
        {
            Label.Add(type.transform.GetChild(i));
        }
    }
    public void InputItemData(ItemInfo item)//데이타받고 슬롯 확인/저장.
    {   //아이템 습득시 호출, 데이타 저장 되는 함수.
        //슬롯 확인 빈슬롯 or 동일아이템
        //곂치는 아이템 없는경우(소비 다른템, 장비)
        switch (item.type.ToString())
        {
            case "Equip":
                for (int i = 0; i < 21; ++i)
                {
                    if (!ItemData.ContainsKey(item.type.ToString() + $"{i}"))
                    {
                        SaveItem inputItem = new SaveItem(item, i);
                        ++inputItem.ItemCount;
                        ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        SetItemToInventoryChildren(EquipSlots[i], item.type.ToString() + $"{i}");//인벤토리 자식으로 설정(생성).
                        break;
                    }
                }
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (!ItemData.ContainsKey(item.type.ToString() + $"{i}"))//slot empty
                    {
                        SaveItem inputItem = new SaveItem(item, i);
                        ++inputItem.ItemCount;
                        ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        SetItemToInventoryChildren(ConsumeSlots[i], item.type.ToString() + $"{i}");
                        SetItemTotalCount(item.ItemName, true);
                        setSlotCount?.Invoke();
                        break;
                    }
                    else if(ItemData[item.type.ToString() + $"{i}"].itemInfo.ItemName == item.ItemName//같은이름의 아이템이 있는경우
                        && ItemData[item.type.ToString() + $"{i}"].ItemCount < item.MaxCount)//슬롯에 겹쳐지는 최대 갯수보다 작을 때
                    {
                        SaveItem inputItem = ItemData[item.type.ToString() + $"{i}"];
                        ++inputItem.ItemCount;
                        ItemData[item.type.ToString() + $"{i}"] = inputItem;
                        ++ConsumeSlots[i].GetComponentInChildren<Item>().ItemCount;
                        SetItemTotalCount(item.ItemName, true);
                        setSlotCount?.Invoke();
                        break;
                    }
                }
                break;
            default:
                break;
        }
    }
    public void OutPutItemData(ItemInfo item)//아이템 사용
    {
        switch (item.type.ToString())
        {
            case "Equip":
                for (int i = 0; i < 21; ++i)
                {
                    if (ItemData.ContainsKey(item.type.ToString() + $"{i}"))
                    {
                        ItemData.Remove(item.type.ToString() + $"{i}");//데이터 삭제.
                        Destroy(EquipSlots[i].GetChild(0).gameObject);//사용한 장비 삭제
                        break;
                    }
                }
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (ItemData.ContainsKey(item.type.ToString() + $"{i}"))
                    {
                        SaveItem outputItem = new SaveItem(item, ConsumeSlots[i].GetComponentInChildren<Item>().ItemCount);
                        --outputItem.ItemCount;
                        if (outputItem.ItemCount == 0)
                        {
                            ItemData.Remove(item.type.ToString() + $"{i}");
                            Destroy(ConsumeSlots[i].GetChild(0).gameObject);
                        }
                        else
                        {
                            ItemData[item.type.ToString() + $"{i}"] = outputItem;
                            --ConsumeSlots[i].GetComponentInChildren<Item>().ItemCount;
                        }
                        SetItemTotalCount(item.ItemName, false);
                        setSlotCount?.Invoke();
                        break;
                    }
                    else
                    {
                        
                    }
                }
                break;
            default:
                break;
        }
    }
    void SetItemToInventoryChildren(Transform parent, string key)//아이템 습득시 inven에 obj생성 및 설정
    {
        GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), parent) as GameObject;
        obj.GetComponent<Item>().iteminfo = ItemData[key].itemInfo;
        obj.GetComponent<Item>().ItemCount = ItemData[key].ItemCount;
    }
    void SetItemTotalCount(string itemName, bool type)//increase : true, decrease : false
    {//소비아이템의 총 갯수 저장
        if(type)
        {
            if (!ItemTotalCount.ContainsKey(itemName))
            {
                ItemTotalCount[itemName] = 1;
                return;
            }
            ++ItemTotalCount[itemName];
        }
        else
        {
            --ItemTotalCount[itemName];
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        AppointedInventory();
        GetInventorySlots(EquipSlots, Inven_Equip);
        GetInventorySlots(ConsumeSlots, Inven_Consume);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) ++PlayerStatData.Level;
    }
}
