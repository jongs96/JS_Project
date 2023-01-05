using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EqItemInfo", menuName = "ScriptableObject/EqItemData", order = 2)]
public class EqItemInfo : ScriptableObject
{
    public Vector3 EquipPostion = Vector3.zero;
    public Vector3 EquipRotation = Vector3.zero;
    public float Range = 0.0f;
}
