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
    public List<SaveItem> InventoryValue;
}

[Serializable]
public struct ChildData
{
    public List<string> itemSlotChild;
    public string WeaponSlotChild;
    public string ShieldSlotChild;
}