using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PlayerData
{
    public Vector3 Position;
    public Stat playerStat;
}

[Serializable]
public struct InventoryData
{
    public List<string> InventoryKey;
    public List<ItemInfo> InventoryItemInfo;
    public List<int> InventorySlotnum;
    public List<int> InventoryItemCount;
}

[Serializable]
public struct ChildData
{
    public ItemInfo[] itemSlotChild;
    public ItemInfo WeaponSlotChild;
    public ItemInfo ShieldSlotChild;
}