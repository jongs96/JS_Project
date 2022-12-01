using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject EscMenu = null;
    Vector2 desireDir = Vector2.zero;
    Vector2 curDir = Vector2.zero;
    public LayerMask Enemy;
    public Stat PlayerStat;
    public bool IsPlaying = true;
    bool IsComboable = false;
    bool Standby = true;
    int ClickCount = 0;


    public void OnDamage(float dmg, Transform target)
    {
        PlayerStat.CurHP -= dmg;        
        if (Mathf.Approximately(PlayerStat.CurHP, 0.0f))
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
                Cursor.lockState = CursorLockMode.Locked;
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
                Hpbar.value = PlayerStat.CurHP / PlayerStat.TotalHP;
                Energybar.value = PlayerStat.CurEnergy / PlayerStat.TotalEnergy;
                CheckGround();
                break;
            case STATE.Pause:
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
        if (PlayerStat.CurEnergy > 1.0) Standby = true;
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
        if(!myAnim.GetBool("IsAir") && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.Space))
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
        if (!myAnim.GetBool("IsAir") && !myAnim.GetBool("IsAttacking") && Input.GetKeyDown(KeyCode.E) && PlayerStat.CurEnergy > 30.0f)
        {
            myAnim.SetTrigger("Skill");
            PlayerStat.CurEnergy -= 30.0f;
        }
        //Open Menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeState(STATE.Pause);
            EscMenu.SetActive(true);
        }
    }
    public void JumpUp()
    {        
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
        //Debug.DrawRay(transform.position, -transform.up * 1.0f, Color.red);
        if (Physics.Raycast(ray, 1.0f, 1 << LayerMask.NameToLayer("Ground")))
        {
            myAnim.SetBool("IsAir", false);
        }
        else
        {
            myAnim.SetBool("IsAir", true);
        }
    }
}
