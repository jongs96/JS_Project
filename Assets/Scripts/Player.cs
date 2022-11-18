using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    Vector2 desireDir = Vector2.zero;
    Vector2 curDir = Vector2.zero;
    public Transform myCam = null;
    public Transform mainBody = null;
    bool IsAir = false;
    // Start is called before the first frame update
    enum STATE
    {
        Create, Normal, Battle, Death
    }
    
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
    }
    public void JumpUp()
    {
        IsAir = true;
        myRigid.AddForce(Vector3.up * 200.0f);
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
