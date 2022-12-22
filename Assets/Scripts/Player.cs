using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : CharacterProperty, IBattle
{
    public enum STATE
    {
        Create, Playing, Pause, Death
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
    public Slider Hpbar = null;
    public Slider Energybar = null;
    
    UnityAction goPos = null;
    Vector3 desireDir = Vector3.zero;
    Vector3 curDir = Vector3.zero;
    public LayerMask Enemy;
    public Stat myStat;
    public bool IsPlaying = true;
    bool IsComboable = false;
    bool canGo = false;
    int ClickCount = 0;        

    public void OnDamage(float dmg, Transform target)
    {
        myStat.CurHP -= dmg;        
        if (Mathf.Approximately(myStat.CurHP, 0.0f))
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
                Pause(false);
                //Fix the cursor inside the window
                //Cursor.lockState = CursorLockMode.Locked;
                break;
            case STATE.Pause:
                Pause(true);
                Cursor.lockState = CursorLockMode.Confined;
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
                Hpbar.value = myStat.CurHP / myStat.MaxHP;
                Energybar.value = myStat.CurEnergy / myStat.MaxEnergy;
                Recovery();
                CheckGround();
                break;
            case STATE.Pause:
                break;
            case STATE.Death:
                break;
        }
    }

    void Recovery()
    {
        myStat.CurEnergy += Time.deltaTime * 1f;
        UIManager.Inst.SetAbility("Energy");
    }


    // Start is called before the first frame update
    void Start()
    {
        StatInitialize();
        ChangeState(STATE.Playing);
    }
    
    // Update is called once per frame
    void Update()
    {        
        StateProcess();
        
        desireDir.x = Input.GetAxis("Horizontal");
        desireDir.y = Input.GetAxis("Vertical");
        desireDir.z = Input.GetAxis("Sprint");

        curDir.x = Mathf.Lerp(curDir.x, desireDir.x, Time.deltaTime * 10.0f);
        curDir.y = Mathf.Lerp(curDir.y, desireDir.y, Time.deltaTime * 10.0f);
        curDir.z = Mathf.Lerp(curDir.z, desireDir.z, Time.deltaTime * 10.0f);

        myAnim.SetFloat("Horizontal", curDir.x);
        myAnim.SetFloat("Vertical", curDir.y);
        myAnim.SetFloat("JudgementValue", curDir.z);
        
        if (Mathf.Abs(myAnim.GetFloat("Vertical")) > 0.01 || Mathf.Abs(myAnim.GetFloat("Horizontal")) > 0.01)
        {
            myAnim.SetBool("IsMoving", true);
            if (myCam.rotation.y != mainBody.rotation.y)
            {
                mainBody.GetComponent<RootMotion>().RotCharactor(mainBody, myCam, 360.0f);
            }
        }
        else
        {
            myAnim.SetBool("IsMoving", false);
        }

        //jump
        if (!myAnim.GetBool("IsAir") && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Jump");
        }
        else if(myAnim.GetBool("IsAir") && Input.GetKeyDown(KeyCode.Space))
        {
            myAnim.SetTrigger("Roll");
        }
        //Sit
        if(!myAnim.GetBool("IsAir") && Input.GetKey(KeyCode.LeftControl))
        {
            myAnim.SetBool("Sitting", true);
        }
        else
        {
            myAnim.SetBool("Sitting", false);
        }
        //Defence
        if (!myAnim.GetBool("IsAir") && Input.GetMouseButton(1))
        {
            myAnim.SetBool("Defending", true);
        }
        else
        {
            myAnim.SetBool("Defending", false);
        }
        //Attack
        if(!myAnim.GetBool("IsAir") && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(!myAnim.GetBool("IsAttacking")) myAnim.SetTrigger("Attack");
            if(IsComboable)
            {
                ClickCount++;
            }
        }
        //JumpAttack
        if (!myAnim.GetBool("IsAir") && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.E) && myStat.CurEnergy > 30.0f)
        {
            myAnim.SetTrigger("Skill");
            myStat.CurEnergy -= 30.0f;
            UIManager.Inst.SetAbility("Energy");
        }
        //Move Portal
        if(Input.GetKeyDown(KeyCode.G) && canGo)
        {
            goPos();
        }
        //Open Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(STATE.Pause);
            UIManager.Inst.PauseMenu.SetActive(true);
            UIManager.Inst.Menu.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!UIManager.Inst.Inventory.activeSelf)
                UIManager.Inst.Inventory.SetActive(true);
            else UIManager.Inst.Inventory.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (!UIManager.Inst.Equip.activeSelf)
                UIManager.Inst.Equip.SetActive(true);
            else UIManager.Inst.Equip.SetActive(false);
        }
    }
    public void JumpUp()
    {
        myAnim.SetBool("IsAir", true);
        myRigid.AddForce((Vector3.up + transform.GetChild(0).forward * myAnim.GetFloat("Vertical") + transform.GetChild(0).right * myAnim.GetFloat("Horizontal")) * 200.0f);
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

    void Pause(bool p)
    {
        if(p)
        {
            IsPlaying = false;
            Time.timeScale = 0.0f;
        }
        else
        {
            IsPlaying = true;
            Time.timeScale = 1.0f;
        }
    }

    public void ResumeGame()
    {
        ChangeState(STATE.Playing);
    }
    void CheckGround()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = -transform.up;
        Debug.DrawRay(transform.position, -transform.up * 0.1f, Color.white);
        if (Physics.Raycast(ray, 0.1f, 1 << LayerMask.NameToLayer("Ground")))
        {
            myAnim.SetBool("IsAir", false);
        }
        else
        {
            myAnim.SetBool("IsAir", true);
        }
    }
    void StatInitialize()
    {
        //if (SceneMgr.Inst.isNewGame)//새 게임
        //{
        myStat = new Stat(1, 0, 40, 500.0f, 0, 5, 5, 5);
        //}
        //else//로드 게임
        //{

        //}
        DataManager.Inst.PlayerStatData = myStat;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Portal"))
        {
            canGo = true;
            goPos = collision.gameObject.GetComponent<Portal>().Teleportation;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Portal"))
        {
            canGo = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //pick up item
        if(other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            PickupItem(other.gameObject);
        }
    }
    
    void PickupItem(GameObject item)
    {
        DataManager.Inst.InputItemData(item.GetComponent<DropItem>().iteminfo);
        Destroy(item);
    }
}