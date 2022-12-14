using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventFuc : MonoBehaviour
{
    public UnityEvent EndDeath = null;
    public float AttackRange = 0.2f;
    public UnityEvent<float> Attack = null;
    public UnityEvent Skill = null;
    public UnityEvent ComboCheckStart = null;
    public UnityEvent ComboCheckEnd = null;

    public void OnAttack()
    {
        Attack?.Invoke(AttackRange);
    }
    public void OnSkill()
    {
        Skill?.Invoke();
    }
    public void OnComboCheckStart()
    {
        ComboCheckStart?.Invoke();
    }
    public void OnComboCheckEnd()
    {
        ComboCheckEnd?.Invoke();
    }
    public void AfterDeath()
    {
        EndDeath?.Invoke();
    }
}
