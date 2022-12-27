using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipItem : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo iteminfo = null;
    public Transform myParent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount >= 2)
        {
            Destroy(gameObject);
        }
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
    public void SetParent(Transform newSlot)
    {
        myParent = newSlot;
        transform.SetParent(myParent.transform);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
