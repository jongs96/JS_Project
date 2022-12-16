using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> Slots = new List<GameObject>();
    public bool[] SlotCheck = new bool[28];
    //public int order
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < SlotCheck.Length; ++i)
        {
            SlotCheck[i] = true;
        }
        for(int i = 0; i < transform.childCount; ++i)
        {
            Slots.Add(transform.GetChild(i).gameObject);
        }
    }

    public int GetInsertableSlotNumber()//Search for blanks from the front
    {
        for(int i = 0; i < SlotCheck.Length; ++i)
        {
            if(SlotCheck[i])
            {
                return i;
            }
        }
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
