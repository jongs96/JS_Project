using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst = null;

    //�����ؾ��ϴ� �����͵�
    public Dictionary<string, SaveItem> ItemData = new Dictionary<string, SaveItem>();
    public Dictionary<string, int> ItemTotalCount = new Dictionary<string, int>();
    public Stat PlayerStatData;
    public Vector3 myPostion = Vector3.zero;
    
    public int SceneNum;
    public Inventory Inven_Equip;
    public Inventory Inven_Consume;
    public EquipSlot WeaponSlot;
    public EquipSlot ShieldSlot;
    public UnityAction setSlotCount;
    public List<Transform> EquipSlots = new List<Transform>();
    public List<Transform> ConsumeSlots = new List<Transform>();
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

    void AppointedInventory()//Ÿ�Ժ� inventory ����.equipSlot ����
    {
        Inventory[] invenM = UIManager.Inst.Inventory.GetComponentsInChildren<Inventory>();
        EquipSlot[] eqSlots = UIManager.Inst.Equip.GetComponentsInChildren<EquipSlot>();
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
        for (int i = 0; i < eqSlots.Length; ++i)
        {
            switch (eqSlots[i].name)
            {
                case "Weapon":
                    WeaponSlot = eqSlots[i];
                    break;
                case "Shield":
                    ShieldSlot = eqSlots[i];
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
                        ItemData[$"{item.type}{i}"] = inputItem;
                        SetItemToInventoryChildren(EquipSlots[i], $"{item.type}{i}");//�κ��丮 �ڽ����� ����(����).
                        break;
                    }
                }
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (!ItemData.ContainsKey($"{item.type}{i}"))//�󽽷Կ� �� ������ �߰�. ����Űlist ����Ȯ�� �� add
                    {
                        SaveItem inputItem = new SaveItem(item, i);
                        ++inputItem.ItemCount;
                        ItemData[$"{item.type}{i}"] = inputItem;
                        SetItemToInventoryChildren(ConsumeSlots[i], $"{item.type}{i}");
                        SetItemTotalCount(item.ItemName, true);
                        setSlotCount?.Invoke();
                        HotKeyListAddRemv(ConsumeSlots[i].GetComponentInChildren<Item>(), true);
                        break;
                    }
                    else if(ItemData[$"{item.type}{i}"].itemInfo.ItemName == item.ItemName//�����̸��� �������� �ִ°��
                        && ItemData[$"{item.type}{i}"].ItemCount < item.MaxCount)//���Կ� �������� �ִ� �������� ���� ��
                    {
                        SaveItem inputItem = ItemData[$"{item.type}{i}"];
                        ++inputItem.ItemCount;
                        ItemData[$"{item.type}{i}"] = inputItem;
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
    void OutPutItemData(Item item)//������ ��� ������ ����.
    {
        switch (item.iteminfo.type.ToString())
        {
            case "Equip":
                ItemData.Remove($"{item.iteminfo.type}{item.mySlotNum}");//������ ����.
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (ItemData.ContainsKey($"{item.iteminfo.type}{i}"))
                    {
                        if (ItemData[$"{item.iteminfo.type}{i}"].itemInfo == item.iteminfo)
                        {
                            SaveItem outputItem = ItemData[$"{item.iteminfo.type}{i}"];
                            --outputItem.ItemCount;
                            if (outputItem.ItemCount == 0)
                            {
                                ItemData.Remove($"{item.iteminfo.type}{i}");
                                HotKeyListAddRemv(item, false);
                                Destroy(ConsumeSlots[i].GetChild(0).gameObject);
                            }
                            else
                            {
                                ItemData[$"{item.iteminfo.type}{i}"] = outputItem;
                                --ConsumeSlots[i].GetComponentInChildren<Item>().ItemCount;
                            }
                            SetItemTotalCount(item.iteminfo.ItemName, false);
                            //setSlotCount?.Invoke();
                            break;
                        }
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
        obj.GetComponent<Item>().mySlotNum = ItemData[key].Slotnum;
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
    public void UseSlotItem(Item item)
    {
        switch (item.iteminfo.type)//���� �������Ҹ� ����.
        {
            case ItemInfo.ItemType.Equip:
                switch (item.iteminfo.equiptype)
                {
                    case ItemInfo.EquipType.Weapon:
                        EquipItem eqwpItem = WeaponSlot.GetComponentInChildren<EquipItem>();
                        if (eqwpItem != null)//�̹� ������ ���� ����
                        {
                            PlayerStatData.AttackPower -= eqwpItem.iteminfo.Value;
                            PlayerStatData.AttackPower += item.iteminfo.Value;
                            UIManager.Inst.SetAbility("AttackPower");
                        }
                        else
                        {
                            PlayerStatData.AttackPower += item.iteminfo.Value;
                            UIManager.Inst.SetAbility("AttackPower");
                        }
                        break;
                    case ItemInfo.EquipType.Shield:
                        EquipItem eqshItem = ShieldSlot.GetComponentInChildren<EquipItem>();
                        if (eqshItem != null)//�̹� ������ ���� ����
                        {
                            PlayerStatData.DefensePower -= eqshItem.iteminfo.Value;
                            PlayerStatData.DefensePower += item.iteminfo.Value;
                            UIManager.Inst.SetAbility("DefensePower");
                        }
                        else
                        {
                            PlayerStatData.DefensePower += item.iteminfo.Value;
                            UIManager.Inst.SetAbility("DefensePower");
                        }
                        break;
                    default:
                        break;
                }
                break;
            case ItemInfo.ItemType.Consume:
                switch (item.iteminfo.ItemName)
                {
                    case "Hp_Potion":
                        PlayerStatData.CurHP += item.iteminfo.Value;
                        UIManager.Inst.SetAbility("HP");
                        break;
                    case "Energy_Potion":
                        PlayerStatData.CurEnergy += item.iteminfo.Value;
                        UIManager.Inst.SetAbility("Energy");
                        break;
                }
                break;
            default:
                break;
        }
        OutPutItemData(item);
    }
    void HotKeyListAddRemv(Item item, bool adrm)
    {
        HotkeySlot[] HkSlots = UIManager.Inst.HotKeySlot.GetComponentsInChildren<HotkeySlot>();
        foreach (HotkeySlot hks in HkSlots)//list���� ����.
        {
            if (hks.itemInfo == item.iteminfo)
            {
                if (!adrm) hks.connectIT.Remove(item);
                else hks.connectIT.Add(item);
                return;
            }
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
        
    }
}
