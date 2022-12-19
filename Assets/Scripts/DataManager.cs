using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst = null;
    //public Dictionary<string, InvenItemData> ItemData = new Dictionary<string, InvenItemData>();
    public Stat StatData;
    public int SceneNum;
    public Vector3 myPostion = Vector3.zero;
    public InventoryManager Inven_Equip;
    public InventoryManager Inven_Consume;
    /*public struct InvenItemData//슬롯의 아이템정보 = item class
    {
        ItemInfo ItemSO;//아이템 정보
        int ItemCount;//갯수
        int slotPos;//슬롯위치 inventorymanager의 slots index
    }*/
    public Item inputItem;
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
                    Inven_Equip = invenM[i];
                    break;
                default:
                    break;
            }
        }
    }
    public void InputItemData(GameObject item)//데이타받고 슬롯 확인/저장.
    {
        //Inven_Equip.GetInsertableSlotNumber()
        //Inven_Equip.SlotCheck[num] = false;//슬롯 확인 빈슬롯 or 동일아이템
        //곂치는 아이템 없는경우(소비 다른템, 장비)
        //GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), Inven_Equip.Slots[num].transform) as GameObject;
        //obj.GetComponent<Item>().iteminfo = item.GetComponent<DropItem>().iteminfo;
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
