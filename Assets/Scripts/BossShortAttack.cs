using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShortAttack : MonoBehaviour
{
    public Boss boss;
    private void OnTriggerEnter(Collider other)
    {
        IBattle ib = other.gameObject.GetComponent<IBattle>();
        if (ib != null)
        {
            ib?.OnDamage(boss.bossStat.AttackPower, boss.transform);
        }
    }
}
