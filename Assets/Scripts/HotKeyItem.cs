using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotKeyItem : MonoBehaviour
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;
    public int count;

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
