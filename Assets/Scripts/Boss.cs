using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MobMovement, IBattle
{
    Transform myTarget = null;
    public MobStat bossStat =  new MobStat(200.0f, 2000.0f, 100.0f, 2.5f, 360.0f, 2.0f);
    public List<ItemInfo> myItems = new List<ItemInfo>();
    public Transform ItemParents;

    public enum STATE
    {
        Create, Normal, Battle, Death
    }
    public STATE myState = STATE.Create;
    public bool IsLive
    {
        get
        {
            if (Mathf.Approximately(bossStat.CurHP, 0.0f))
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
        bossStat.CurHP -= dmg;
        if (Mathf.Approximately(bossStat.CurHP, 0.0f))
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
                myAnim.SetBool("IsMoving", false);
                break;
            case STATE.Battle:
                StopAllCoroutines();
                FollowTarget(myTarget, bossStat.MoveSpeed, bossStat.RotSpeed, 20.0f, OnAttack);
                break;
            case STATE.Death:
                //if (myHpBar != null) Destroy(myHpBar.gameObject);
                DropItem();
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
                if (!myAnim.GetBool("IsAttacking")) bossStat.curAttackDelay += Time.deltaTime;
                //Lost Target
                if (OutRange)
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
    void OnAttack()
    {
        float dist = (myTarget.position - transform.position).magnitude;
        if(dist < 3.0f)//short attack
        {
            myAnim.SetInteger("RandNum", Random.Range(0, 2));
        }
        else if(dist >= 3.0f && dist < 10.0f)//middle attack
        {
            myAnim.SetInteger("RandNum", Random.Range(2, 4));
        }
        else if(dist >= 10.0f&& dist<15.0f)//long attack
        {
            myAnim.SetInteger("RandNum", 4);
        }

        if (!myAnim.GetBool("IsAttacking"))
        {
            myAnim.SetTrigger("Attack");
        }
    }

    void Initialize()
    {
        ChangeState(STATE.Normal);
    }
    public void AttackTarget()//실제 데미지
    {
        //Collider[] list = Physics.OverlapSphere(AttackPos.position, 0.5f, Target);
        //foreach (Collider col in list)
        //{
        //    IBattle ib = col.GetComponent<IBattle>();
        //    ib?.OnDamage(bossStat.AttackPower, transform);
        //}
    }
    public void DropItem()
    {
        foreach (ItemInfo item in myItems)
        {
            int rate = Random.Range(1, 101);
            if (rate < item.Droprate)
            {
                GameObject obj = Instantiate(Resources.Load("Item/DropItem"), transform.position, Quaternion.identity, ItemParents) as GameObject;
                obj.GetComponent<DropItem>().iteminfo = item;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
}
