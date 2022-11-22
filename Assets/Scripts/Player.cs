using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class Player : CharacterProperty, IBattle
{
    enum STATE
    {
        Create, Normal, Battle, Death
    }
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
    bool IsAir = false;
    bool IsComboable = false;
    int ClickCount = 0;


    public void OnDamage(float dmg, Transform target)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //walk and run
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
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
        if(!IsAir && Input.GetKeyDown(KeyCode.Space))
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
    }
    public void JumpUp()
    {
        IsAir = true;
        myRigid.AddForce(Vector3.up * 200.0f);
    }

    //attack function for animationevent
    public void AttackTarget()
    {
        Collider[] list = Physics.OverlapSphere(AttackPos.position, 0.55f, Enemy);
        foreach(Collider col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            ib?.OnDamage(100.0f, transform);
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
