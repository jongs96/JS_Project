using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class Player : CharacterProperty, IBattle
{
    public enum STATE
    {
        Create, Playing, Death
    }
    public STATE myState = STATE.Create;
    public bool IsLive
    {
        get
        {
            return true;
        }
    }
    Transform myHeadPos;
    public Transform HeadPos { get => myHeadPos; }
    public Transform AttackPos = null;
    public Transform myCam = null;
    public Transform mainBody = null;
    Vector2 desireDir = Vector2.zero;
    Vector2 curDir = Vector2.zero;
    public LayerMask Enemy;
    public Stat PlayerStat;
    bool IsAir = false;
    bool IsComboable = false;
    bool Standby = true;
    int ClickCount = 0;


    public void OnDamage(float dmg, Transform target)
    {
        PlayerStat.CurHP -= dmg;
        if(Mathf.Approximately(PlayerStat.CurHP, 0.0f))
        {
            ChangeState(STATE.Death);
        }
        else
        {
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
            case STATE.Playing:                
                break;            
            case STATE.Death:
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
            case STATE.Playing:
                break;            
            case STATE.Death:
                break;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerStat = new Stat(1000.0f, 100.0f, 2.2f, 360.0f, default, 100.0f);
        ChangeState(STATE.Playing);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        if(!myAnim.GetBool("IsRunning")) PlayerStat.CurEnergy += 3 * Time.deltaTime;
        if (Mathf.Approximately(PlayerStat.CurEnergy, 0)) Standby = false;
        if (PlayerStat.CurEnergy > 5.0) Standby = true;
        //walk and run
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Standby)
            {
                myAnim.SetBool("IsWalking", false);
                myAnim.SetBool("IsRunning", true);
            }
            else
            {
                myAnim.SetBool("IsRunning", false);
                myAnim.SetBool("IsWalking", true);                
            }
            if (myCam.rotation.y != mainBody.rotation.y)
            {
                mainBody.GetComponent<RootMotion>().RotCharactor(mainBody, myCam, 360.0f);
            }
        }
        else if (Mathf.Approximately(Input.GetAxis("Horizontal"), 0) && Mathf.Approximately(Input.GetAxis("Vertical"), 0))
        {
            myAnim.SetBool("IsWalking", false);
            myAnim.SetBool("IsRunning", false);
        }

        desireDir.x = Input.GetAxis("Horizontal");
        desireDir.y = Input.GetAxis("Vertical");

        curDir.x = Mathf.Lerp(curDir.x, desireDir.x, Time.deltaTime * 10.0f);
        curDir.y = Mathf.Lerp(curDir.y, desireDir.y, Time.deltaTime * 10.0f);

        myAnim.SetFloat("x", curDir.x);
        myAnim.SetFloat("y", curDir.y);

        //jump
        if(!IsAir && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Jump");
        }
        else if(IsAir && Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Roll");
        }
        //Sit
        if(!IsAir && Input.GetKey(KeyCode.LeftControl))
        {
            myAnim.SetBool("Sitting", true);
        }
        else
        {
            myAnim.SetBool("Sitting", false);
        }
        //Defence
        if (!IsAir && Input.GetMouseButton(1))
        {
            myAnim.SetBool("Defending", true);
        }
        else
        {
            myAnim.SetBool("Defending", false);
        }
        //Attack
        if(!IsAir && Input.GetMouseButtonDown(0))
        {
            if(!myAnim.GetBool("IsAttacking")) myAnim.SetTrigger("Attack");
            if(IsComboable)//&& !EventSystem.current.IsPointerOverGameObject())
            {
                ClickCount++;
            }
        }
        //JumpAttack
        if (!IsAir && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.E) && PlayerStat.CurEnergy > 30.0f)
        {
            myAnim.SetTrigger("Skill");
            PlayerStat.CurEnergy -= 30.0f;
        }
    }
    public void JumpUp()
    {
        IsAir = true;
        myRigid.AddForce(Vector3.up * 200.0f);
    }

    //AnimationEvent Attack function
    public void AttackTarget()
    {
        Collider[] list = Physics.OverlapSphere(AttackPos.position, 0.55f, Enemy);
        foreach(Collider col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            ib?.OnDamage(100.0f, transform);
        }
    }
    public void AttackSkill()
    {
        Collider[] list = Physics.OverlapSphere(transform.position, 3.0f, Enemy);
        foreach (Collider col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            ib?.OnDamage(200.0f, transform);
        }
    }

    public void ComboCheck(bool swich)
    {
        IsComboable = swich;
        if (swich)
        {
            //ComboCheckStart
            ClickCount = 0;
        }
        else
        {
            //ComboCheckEnd
            if (ClickCount == 0)
            {
                myAnim.SetTrigger("Stop");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsAir = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            IsAir = true;
        }
    }
}
