using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemInfo iteminfo = null;
    public Transform Count_text = null;
    public int mySlotNum;
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
