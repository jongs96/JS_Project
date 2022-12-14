using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "ItemInfo", menuName = "ScriptableObject/ItemData", order = 1)]
public class ItemInfo : ScriptableObject
{
    public enum ItemType
    { Consume, Equip }
    public ItemType type;
    public string ItemName;
    //ConsumeItem recovery amount
    public int Value;
    public float Droprate = 100.0f;
    //Number of Inventory item
    public int MaxCount = 20;
    //Item original object
    public GameObject Resource = null;
}