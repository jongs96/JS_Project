using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stat
{
    [SerializeField] int level;
    public int Level
    {
        get => level;
        set => level = Mathf.Clamp(value, 0, 30);
    }
    [SerializeField] float maxExp;
    [SerializeField] float curExp;
    public float MaxExp
    {
        get => maxExp;
        set => maxExp = value;
    }
    public float CurExp
    {
        get => curExp;
        set => curExp = value;
    }
    [SerializeField] float maxHp;
    [SerializeField] float curHp;

    public float MaxHP
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
    }
    [SerializeField] int strength;
    public int Strength
    {
        get => strength;
        set => strength = value;
    }

    [SerializeField] float maxEnergy;
    [SerializeField] float curEnergy;
    public float TotalEnergy
    {
        get => maxEnergy;
    }
    public float CurEnergy
    {
        get => curEnergy;
        set => curEnergy = Mathf.Clamp(value, 0.0f, maxEnergy);
    }

    [SerializeField] float defensePower;
    public float DefensePower
    {
        get => defensePower;
    }
    [SerializeField] int agility;
    public int Agility
    {
        get => agility;
        set => agility = value;
    }

    [SerializeField] float criticalPercent;
    public float CriticalPercent
    {
        get => criticalPercent;
    }
    [SerializeField] int evation;
    public int Evation
    {
        get => evation;
        set => evation = value;
    }

    public Stat(int level, float curExp, float maxExp, float curHp, int strength, int agility, int evation)
    {
        this.level = level;
        this.curExp = curExp;
        this.maxExp = maxExp;
        this.curHp = curHp;
        maxHp = 400 + level * 100;
        this.strength = strength;
        attackPower = strength * 20;
        this.agility = agility;
        defensePower = agility * 20;
        this.evation = evation;
        criticalPercent = evation * 1.0f;
        curEnergy = maxEnergy = 100 + level * 10;
    }
}
