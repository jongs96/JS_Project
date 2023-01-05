using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] int level;
    public int Level
    {
        get => level;
        set
        {
            level = Mathf.Clamp(value, 0, 30);
            CurHP = maxHp = 400 + level * 100;
            curEnergy = maxEnergy = 90 + level * 10;
            point += 5;
            MaxExp += 20;
        }
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
        set
        {
            attackPower = value;
        }
    }
    [SerializeField] int strength;
    public int Strength
    {
        get => strength;
        set
        {
            attackPower += (value - strength) * 20;
            strength = value;            
        }
    }

    [SerializeField] float maxEnergy;
    [SerializeField] float curEnergy;
    public float MaxEnergy
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
        set => defensePower = value;
    }
    [SerializeField] int agility;
    public int Agility
    {
        get => agility;
        set
        {
            defensePower += (value - agility) * 20;
            agility = value;
        }
    }

    [SerializeField] float criticalRate;
    public float CriticalRate
    {
        get => criticalRate;
        set => criticalRate = value;
    }
    [SerializeField] int evation;
    public int Evation
    {
        get => evation;
        set
        {
            criticalRate += (value - evation) * 1;
            evation = value;
        }
    }

    [SerializeField] int point;
    public int Point
    {
        get => point;
        set => point = Mathf.Clamp(value, 0, 150);
    }

    public Stat(int level, float curExp, float maxExp, float curHp, int point, int strength, int agility, int evation)
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
        criticalRate = evation * 1.0f;
        curEnergy = maxEnergy = 90 + level * 10;
        this.point = point;
    }
}
