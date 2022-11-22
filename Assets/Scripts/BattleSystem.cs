using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    Transform HeadPos { get; }
    Transform transform { get; }
    void OnDamage(float dmg, Transform target);
    bool IsLive { get; }
}

public class BattleSystem : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}