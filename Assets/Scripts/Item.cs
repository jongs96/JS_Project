using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;
    public int ItemCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/" + iteminfo.ItemName);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Countingnumber()
    {
        Count_text.GetComponent<TMPro.TextMeshPro>().text = ItemCount.ToString();
        if(ItemCount >= 2) Count_text.gameObject.SetActive(true);
    }
}
