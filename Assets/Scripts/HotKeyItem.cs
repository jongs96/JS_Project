using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HotKeyItem : MonoBehaviour
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/" + iteminfo.ItemName);
        SetCountText();
        DataManager.Inst.setSlotCount = SetCountText;
    }

    // Update is called once per frame
    void Update()
    {
    }
    void SetCountText()
    {
        Count_text.GetComponent<TMPro.TMP_Text>().text = DataManager.Inst.ItemTotalCount[iteminfo.ItemName].ToString();
    }
}
