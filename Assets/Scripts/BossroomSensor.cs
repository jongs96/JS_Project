using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossroomSensor : MonoBehaviour
{
    public LayerMask myEnemy = default;
    public Boss boss;
    public event MyAction FindTarget = null;


    // Start is called before the first frame update
    void Start()
    {
        myEnemy = LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MakeHpBar()
    {
        //GameObject obj = Instantiate(Resources.Load("UI/BossHpBar")) as GameObject;
        //myHpBar = obj.GetComponent<HpBar>();
        //myHpBar.myTarget = myHeadPos;
        //obj.transform.SetParent(UIManager.Inst.hpBars);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((myEnemy & 1 << other.gameObject.layer) != 0)
        {
            boss.myTarget = other.transform;
            FindTarget?.Invoke();
        }
    }
}
