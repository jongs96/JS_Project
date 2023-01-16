using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MobStat
{    
    [SerializeField] float exp;
    public float Exp
    {
        get => exp;
    }

    [SerializeField] float maxHp;
    [SerializeField] float curHp;

    public float TotalHP
    {
        get => maxHp;
    }
    public float CurHP
    {
        get => curHp;
        set => curHp = Mathf.Clamp(value, 0.0f, maxHp);
    }

    [SerializeField] float attackPower;
    public float AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    [SerializeField] float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    [SerializeField] float rotSpeed;
    public float RotSpeed
    {
        get => rotSpeed;
        set => rotSpeed = value;
    }

    [SerializeField] float attackDelay;
    public float curAttackDelay;
    
    public float AttackDelay
    {
        get => attackDelay;
    }

    public MobStat(float exp, float hp, float attackPower, float moveSpeed, float rotSpeed, float attackDelay)
    {
        this.exp = exp;
        curHp = maxHp = hp;
        this.attackPower = attackPower;
        this.moveSpeed = moveSpeed;
        this.rotSpeed = rotSpeed;
        this.attackDelay = attackDelay;
        curAttackDelay =  0.0f;
    }
}
