using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEventFuc : MonoBehaviour
{
    public UnityEvent Attack = null;
    public UnityEvent ComboCheckStart = null;
    public UnityEvent ComboCheckEnd = null;

    public void OnAttack()
    {
        Attack?.Invoke();
    }
    public void OnComboCheckStart()
    {
        ComboCheckStart?.Invoke();
    }
    public void OnComboCheckEnd()
    {
        ComboCheckEnd?.Invoke();
    }
}
