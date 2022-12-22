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

    void AppointedInventory()//Ÿ�Ժ� inventory ����.
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
    void GetInventorySlots(List<Transform> Label, Inventory type)//Ÿ�Ժ� ���� ����.
    {        
        for (int i = 0; i < type.transform.childCount; ++i)
        {
            Label.Add(type.transform.GetChild(i));
        }
    }
    public void InputItemData(ItemInfo item)//����Ÿ�ް� ���� Ȯ��/����.
    {   //������ ����� ȣ��, ����Ÿ ���� �Ǵ� �Լ�.
        //���� Ȯ�� �󽽷� or ���Ͼ�����
        //��ġ�� ������ ���°��(�Һ� �ٸ���, ���)
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
                        SetItemToInventoryChildren(EquipSlots[i], item.type.ToString() + $"{i}");//�κ��丮 �ڽ����� ����(����).
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
                    else if(ItemData[item.type.ToString() + $"{i}"].itemInfo.ItemName == item.ItemName//�����̸��� �������� �ִ°��
                        && ItemData[item.type.ToString() + $"{i}"].ItemCount < item.MaxCount)//���Կ� �������� �ִ� �������� ���� ��
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
    public void OutPutItemData(ItemInfo item)//������ ���
    {
        switch (item.type.ToString())
        {
            case "Equip":
                for (int i = 0; i < 21; ++i)
                {
                    if (ItemData.ContainsKey(item.type.ToString() + $"{i}"))
                    {
                        ItemData.Remove(item.type.ToString() + $"{i}");//������ ����.
                        Destroy(EquipSlots[i].GetChild(0).gameObject);//����� ��� ����
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
    void SetItemToInventoryChildren(Transform parent, string key)//������ ����� inven�� obj���� �� ����
    {
        GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), parent) as GameObject;
        obj.GetComponent<Item>().iteminfo = ItemData[key].itemInfo;
        obj.GetComponent<Item>().ItemCount = ItemData[key].ItemCount;
    }
    void SetItemTotalCount(string itemName, bool type)//increase : true, decrease : false
    {//�Һ�������� �� ���� ����
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
