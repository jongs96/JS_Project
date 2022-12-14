using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System;

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
public class DataManager : MonoBehaviour
{
    public static DataManager Inst = null;

    //저장해야하는 데이터들
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
    
    private void Awake()
    {
        if (Inst != null) Destroy(gameObject);
        Inst = this;
    }

    void AppointedInventory()//타입별 inventory 저장.equipSlot 저장
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
                    if (!ItemData.ContainsKey($"{item.type}.{i}"))
                    {
                        SaveItem inputItem = new SaveItem(item, i);
                        ++inputItem.ItemCount;
                        ItemData[$"{item.type}.{i}"] = inputItem;
                        SetItemToInventoryChildren(EquipSlots[i], $"{item.type}.{i}");//인벤토리 자식으로 설정(생성).
                        break;
                    }
                }
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (!ItemData.ContainsKey($"{item.type}.{i}"))//빈슬롯에 새 아이템 추가. 단축키list 존재확인 후 add
                    {
                        SaveItem inputItem = new SaveItem(item, i);
                        ++inputItem.ItemCount;
                        ItemData[$"{item.type}.{i}"] = inputItem;
                        SetItemToInventoryChildren(ConsumeSlots[i], $"{item.type}.{i}");
                        SetItemTotalCount(item.ItemName, true);
                        setSlotCount?.Invoke();
                        HotKeyListAddRemv(ConsumeSlots[i].GetComponentInChildren<Item>(), true);
                        break;
                    }
                    else if(ItemData[$"{item.type}.{i}"].itemInfo.ItemName == item.ItemName//같은이름의 아이템이 있는경우
                        && ItemData[$"{item.type}.{i}"].ItemCount < item.MaxCount)//슬롯에 겹쳐지는 최대 갯수보다 작을 때
                    {
                        SaveItem inputItem = ItemData[$"{item.type}.{i}"];
                        ++inputItem.ItemCount;
                        ItemData[$"{item.type}.{i}"] = inputItem;
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
    void OutPutItemData(Item item)//아이템 사용 데이터 제거.
    {
        switch (item.iteminfo.type.ToString())
        {
            case "Equip":
                ItemData.Remove($"{item.iteminfo.type}.{item.mySlotNum}");//데이터 삭제.
                break;
            case "Consume":
                for (int i = 0; i < 21; ++i)
                {
                    if (ItemData.ContainsKey($"{item.iteminfo.type}.{i}"))
                    {
                        if (ItemData[$"{item.iteminfo.type}.{i}"].itemInfo == item.iteminfo)
                        {
                            SaveItem outputItem = ItemData[$"{item.iteminfo.type}.{i}"];
                            --outputItem.ItemCount;
                            if (outputItem.ItemCount == 0)
                            {
                                ItemData.Remove($"{item.iteminfo.type}.{i}");
                                HotKeyListAddRemv(item, false);
                                Destroy(ConsumeSlots[i].GetChild(0).gameObject);
                            }
                            else
                            {
                                ItemData[$"{item.iteminfo.type}.{i}"] = outputItem;
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
    void SetItemToInventoryChildren(Transform parent, string key)//아이템 습득시 inven에 obj생성 및 설정
    {
        GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), parent) as GameObject;
        obj.GetComponent<Item>().iteminfo = ItemData[key].itemInfo;
        obj.GetComponent<Item>().ItemCount = ItemData[key].ItemCount;
        obj.GetComponent<Item>().mySlotNum = ItemData[key].Slotnum;
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
    public void UseSlotItem(Item item)
    {
        switch (item.iteminfo.type)//스텟 증가감소만 관리.
        {
            case ItemInfo.ItemType.Equip:
                switch (item.iteminfo.equiptype)
                {
                    case ItemInfo.EquipType.Weapon:
                        EquipItem eqwpItem = WeaponSlot.GetComponentInChildren<EquipItem>();
                        if (eqwpItem != null)//이미 장착한 무기 존재
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
                        if (eqshItem != null)//이미 장착한 방패 존재
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
        foreach (HotkeySlot hks in HkSlots)//list에서 제거.
        {
            if (hks.itemInfo == item.iteminfo)
            {
                if (!adrm) hks.connectIT.Remove(item);
                else hks.connectIT.Add(item);
                return;
            }
        }
    }
    void SavePlayerData()
    {
        PlayerData player = new PlayerData();
        player.Position = Player.Inst.transform.position;
        player.playerStat = PlayerStatData;
        string data = JsonUtility.ToJson(player, true);
        FileManager.Inst.SaveText(Application.dataPath + @"\PlayerData.json", data);

        InventoryData inventoryData = new InventoryData();
        inventoryData.InventoryKey = new List<string>();
        inventoryData.InventoryItemInfo = new List<ItemInfo>();
        inventoryData.InventorySlotnum = new List<int>();
        inventoryData.InventoryItemCount = new List<int>();

        foreach (string Key in ItemData.Keys)
        {
            inventoryData.InventoryKey.Add(Key);
        }
        foreach (SaveItem Value in ItemData.Values)
        {
            inventoryData.InventoryItemInfo.Add(Value.itemInfo);
            inventoryData.InventorySlotnum.Add(Value.Slotnum);
            inventoryData.InventoryItemCount.Add(Value.ItemCount);
        }
        data = JsonUtility.ToJson(inventoryData, true);
        FileManager.Inst.SaveText(Application.dataPath + @"\InventoryData.json", data);
        
        ChildData childData = new ChildData();
        if (ShieldSlot.transform.GetComponentInChildren<EquipItem>() != null)
        {
            childData.ShieldSlotChild = ShieldSlot.transform.GetComponentInChildren<EquipItem>().iteminfo;
        }
        if (WeaponSlot.transform.GetComponentInChildren<EquipItem>() != null)
        {
            childData.WeaponSlotChild = WeaponSlot.transform.GetComponentInChildren<EquipItem>().iteminfo;
        }
        HotkeySlot[] HkSlots = UIManager.Inst.HotKeySlot.GetComponentsInChildren<HotkeySlot>();
        childData.itemSlotChild = new ItemInfo[HkSlots.Length];
        for(int i = 0; i < HkSlots.Length; ++i)
        {
            childData.itemSlotChild[i] = HkSlots[i].itemInfo;
        }
        data = JsonUtility.ToJson(childData, true);
        FileManager.Inst.SaveText(Application.dataPath + @"\ChildData.json", data);
    }
    void LoadData()
    {//ItemData
        string data = FileManager.Inst.LoadText(Application.dataPath + @"\InventoryData.Json");
        InventoryData invenData = JsonUtility.FromJson<InventoryData>(data);
        for (int i = 0; i < invenData.InventoryKey.Count; ++i)
        {
            SaveItem saveItem = new SaveItem();
            saveItem.ItemCount = invenData.InventoryItemCount[i];
            saveItem.Slotnum = invenData.InventorySlotnum[i];
            saveItem.itemInfo = invenData.InventoryItemInfo[i];
            ItemData.Add(invenData.InventoryKey[i], saveItem);
            string[]typenum = invenData.InventoryKey[i].Split(".");
            switch(typenum[0])
            {
                case "Equip":
                    SetItemToInventoryChildren(EquipSlots[int.Parse(typenum[1])], $"Equip.{int.Parse(typenum[1])}");
                    break;
                case "Consume":
                    SetItemToInventoryChildren(ConsumeSlots[int.Parse(typenum[1])], $"Consume.{int.Parse(typenum[1])}");
                    if (!ItemTotalCount.ContainsKey(saveItem.itemInfo.ItemName))
                    {
                        ItemTotalCount[saveItem.itemInfo.ItemName] = saveItem.ItemCount;
                    }
                    else
                    {
                        ItemTotalCount[saveItem.itemInfo.ItemName] += saveItem.ItemCount;
                    }
                    break;
            }
        }
        //hotkeyslot, equipslot
        data = FileManager.Inst.LoadText(Application.dataPath + @"\ChildData.Json");
        ChildData childData = JsonUtility.FromJson<ChildData>(data);
        if (childData.WeaponSlotChild != null)
        {
            WeaponSlot.GetComponent<EquipSlot>().itemInfo = childData.WeaponSlotChild;
            GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), WeaponSlot.transform) as GameObject;
            WeaponSlot.GetComponent<EquipSlot>().ChangeImg("Sprite/Eq_Slot");
            obj.GetComponent<EquipItem>().iteminfo = childData.WeaponSlotChild;
            obj.GetComponent<EquipItem>().SetParent(WeaponSlot.transform);
            WeaponSlot.GetComponent<EquipSlot>().EquipmentObj();
        }
        if (childData.ShieldSlotChild != null)
        {
            ShieldSlot.GetComponent<EquipSlot>().itemInfo = childData.ShieldSlotChild;
            GameObject obj = Instantiate(Resources.Load("Item/EquipItem"), ShieldSlot.transform) as GameObject;
            ShieldSlot.GetComponent<EquipSlot>().ChangeImg("Sprite/Eq_Slot");
            obj.GetComponent<EquipItem>().iteminfo = childData.ShieldSlotChild;
            obj.GetComponent<EquipItem>().SetParent(ShieldSlot.transform);
            ShieldSlot.GetComponent<EquipSlot>().EquipmentObj();
        }
        HotkeySlot[] HkSlots = UIManager.Inst.HotKeySlot.GetComponentsInChildren<HotkeySlot>();
        for (int i = 0; i < HkSlots.Length; ++i)
        {
            if(childData.itemSlotChild[i] != null)
            {
                GameObject obj = Instantiate(Resources.Load("Item/HotKeyItem"), HkSlots[i].transform) as GameObject;
                obj.GetComponent<HotKeyItem>().iteminfo = childData.itemSlotChild[i];
                obj.GetComponent<HotKeyItem>().SetParent(HkSlots[i]);
                HkSlots[i].GetComponent<HotkeySlot>().itemInfo = obj.GetComponent<HotKeyItem>().iteminfo;
                HkSlots[i].GetComponent<HotkeySlot>().ConnectItem();
            }
        }
    }
    public void ExitGame()
    {
        SavePlayerData();
        SceneMgr.Inst.MoveSceneToLoad(1);

        Time.timeScale = 1.0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        AppointedInventory();
        GetInventorySlots(EquipSlots, Inven_Equip);
        GetInventorySlots(ConsumeSlots, Inven_Consume);
        if(!SceneMgr.Inst.isNewGame)
        {
            LoadData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
