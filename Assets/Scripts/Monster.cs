using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MobMovement, IBattle
{
    Transform myTarget = null;
    Vector3 StartPos = Vector3.zero;
    public Stat mobStat;
    public Transform AttackPos;
    public LayerMask Target;
    public enum STATE
    {
        Create, Normal, Battle, Death
    }
    public STATE myState = STATE.Create;
    public bool IsLive
    {
        get
        {
            if(Mathf.Approximately(mobStat.CurHP, 0.0f))
            {
                return false;
            }
            return true;
        }
    }
    public Transform myHeadPos;
    public Transform HeadPos { get => myHeadPos; }
    public void OnDamage(float dmg, Transform target)
    {
        myTarget = target;
        mobStat.CurHP -= dmg;
        if(Mathf.Approximately(mobStat.CurHP, 0.0f))
        {
            ChangeState(STATE.Death);
        }
        else
        {
            ChangeState(STATE.Battle);
            myAnim.SetTrigger("Damage");
        }
    }

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Normal:
                StopAllCoroutines();
                mobStat.MoveSpeed = 0.8f;
                mobStat.RotSpeed = 180.0f;
                myAnim.SetBool("IsMoving", false);
                myAnim.SetBool("Battle", false);
                StartCoroutine(GoingToRndPos());
                break;
            case STATE.Battle:
                StopAllCoroutines();
                mobStat.MoveSpeed = 2.0f;
                mobStat.RotSpeed = 360.0f;
                myAnim.SetBool("Battle", true);
                FollowTarget(myTarget, mobStat.MoveSpeed, mobStat.RotSpeed, OnAttack);
                break;
            case STATE.Death:
                //if (myHpBar != null) Destroy(myHpBar.gameObject);
                StopAllCoroutines();
                myAnim.SetTrigger("Death");
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Normal:
                break;
            case STATE.Battle:
                if (!myAnim.GetBool("IsAttacking")) mobStat.curAttackDelay += Time.deltaTime;
                //Lost Target
                if(OutRange)
                {
                    myTarget = null;
                    ChangeState(STATE.Normal);
                    OutRange = false;
                }
                break;
            case STATE.Death:
                break;
        }
    }

    IEnumerator GoingToRndPos()
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        Vector3 rndPos = transform.position;
        rndPos.x = Random.Range(-5.0f, 5.0f);
        rndPos.y = Random.Range(-5.0f, 5.0f);
        MoveToPos(rndPos, mobStat.MoveSpeed, mobStat.RotSpeed, () => StartCoroutine(GoingToRndPos()));
    }
    void OnAttack()
    {
        if (!myAnim.GetBool("IsAttacking"))
        {
            myAnim.SetTrigger("Attack");
        }
    }

    //AnimationEvent Attack function
    public void AttackTarget()
    {
        Collider[] list = Physics.OverlapSphere(AttackPos.position, 0.5f, Target);
        foreach (Collider col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            ib?.OnDamage(mobStat.AttackPower, transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        mobStat = new Stat(300.0f, 50.0f, 0.8f, 180.0f, 2.0f, default);
        ChangeState(STATE.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
}