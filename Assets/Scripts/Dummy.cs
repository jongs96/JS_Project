using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : CharacterProperty, IBattle
{
    public List<ItemInfo> myItems = new List<ItemInfo>();
    public Transform ItemParents;
    bool Dropping = true;
    public bool IsLive { get { return true; } }
    public Transform myHeadPos;
    public Transform HeadPos { get => myHeadPos; }
    public void OnDamage(float dmg, Transform target)
    {
        StartCoroutine(TakeHit());
        if (Dropping)
        {
            DropItem();
            Dropping = false;
        }
    }
    IEnumerator TakeHit()
    {
        Renderer render = myRenderer;
        render.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.4f);
        render.material.SetColor("_Color", Color.white);
    }
    public void DropItem()
    {
        foreach (ItemInfo item in myItems)
        {
            int rate = Random.Range(0, 100);
            Vector3 droppos = transform.position + new Vector3(0, 0, 0.5f);
            if (rate < item.Droprate)
            {
                GameObject obj = Instantiate(Resources.Load("Item/DropItem"), droppos, Quaternion.identity, ItemParents) as GameObject;
                obj.GetComponent<DropItem>().iteminfo = item;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
