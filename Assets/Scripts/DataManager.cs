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
    /*public struct InvenItemData//������ ���������� = item class
    {
        ItemInfo ItemSO;//������ ����
        int ItemCount;//����
        int slotPos;//������ġ inventorymanager�� slots index
    }*/
    public Item inputItem;
    private void Awake()
    {
        if (Inst != null) Destroy(gameObject);
        Inst = this;
    }

    void AppointedInventory()//Ÿ�Ժ� ���� ����.
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
    public void InputItemData(GameObject item)//����Ÿ�ް� ���� Ȯ��/����.
    {
        //Inven_Equip.GetInsertableSlotNumber()
        //Inven_Equip.SlotCheck[num] = false;//���� Ȯ�� �󽽷� or ���Ͼ�����
        //��ġ�� ������ ���°��(�Һ� �ٸ���, ���)
        //GameObject obj = Instantiate(Resources.Load("Item/SlotItem"), Inven_Equip.Slots[num].transform) as GameObject;
        //obj.GetComponent<Item>().iteminfo = item.GetComponent<DropItem>().iteminfo;
        SaveItemData();//����
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
