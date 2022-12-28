using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerClickHandler,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;
    public int mySlotNum;
    GameObject draggingObj;
    public bool IsDestroy = false;
    int _itemCount = 0;

    [SerializeField]
    public int ItemCount
    {
        get
        {
            return _itemCount;
        }
        set
        {
            _itemCount = value;            
            Count_text.GetComponent<TMPro.TMP_Text>().text = _itemCount.ToString();
            if (_itemCount >= 2)
            {                
                Count_text.gameObject.SetActive(true);
            }
            else
            {
                Count_text.gameObject.SetActive(false);
            }
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //마우스 클릭 했을때 소비 아이템 사용 & 장비장착
        if(eventData.clickCount == 2)
        {
            switch(iteminfo.equiptype)
            {
                case ItemInfo.EquipType.Weapon:
                    EquipItem eqwpItem = DataManager.Inst.WeaponSlot.GetComponentInChildren<EquipItem>();
                    if (eqwpItem != null)//이미 장착한 무기 존재
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                case ItemInfo.EquipType.Shield:
                    EquipItem eqsdItem = DataManager.Inst.ShieldSlot.GetComponentInChildren<EquipItem>();
                    if (eqsdItem != null)//이미 장착한 방패 존재
                    {
                        
                    }
                    else
                    {
                        
                    }
                    break;
                default:
                    break;
            }
            DataManager.Inst.UseSlotItem(iteminfo);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 시작
        //Debug.Log("드래그 시작");
        draggingObj = Instantiate(Resources.Load("Item/SlotItem"), UIManager.Inst.HotKeySlot.parent) as GameObject;
        draggingObj.GetComponent<Item>().iteminfo = iteminfo;
        Color color = draggingObj.GetComponent<Image>().color;
        color.a /= 2;
        draggingObj.GetComponent<Image>().color = color;
        draggingObj.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //드래그 중
        //Debug.Log("드래그 중: " + eventData.position);
        draggingObj.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그 끝
        if(IsDestroy)//장비를 드래그드랍한 경우.
        {
            Destroy(gameObject);
        }
        Destroy(draggingObj, 0.01f);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/" + iteminfo.ItemName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
