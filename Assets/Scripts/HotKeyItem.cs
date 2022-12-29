using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HotKeyItem : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;
    public HotkeySlot myParent;
    GameObject draggingObj;
    Vector2 dragoffset = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprite/{iteminfo.ItemName}");
        SetCountText();
        DataManager.Inst.setSlotCount = SetCountText;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�� ����
        //Debug.Log("�巡�� ����");
        dragoffset = (Vector2)transform.position - eventData.position;
        draggingObj = eventData.pointerDrag;
        draggingObj.transform.SetParent(transform.parent.parent);
        draggingObj.GetComponent<Image>().raycastTarget = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //�巡�� ��
        //Debug.Log("�巡�� ��: " + eventData.position);
        draggingObj.transform.position = eventData.position + dragoffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�巡�� ��
        //Debug.Log("�巡�� ��");
        transform.SetParent(myParent.transform);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        draggingObj.GetComponent<Image>().raycastTarget = true;
    }
    public void SetParent(HotkeySlot newSlot)
    {
        myParent = newSlot;
        transform.SetParent(myParent.transform);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    public void SetCountText()//ui�� count reset
    {
        Count_text.GetComponent<TMPro.TMP_Text>().text = DataManager.Inst.ItemTotalCount[iteminfo.ItemName].ToString();
    }
}
