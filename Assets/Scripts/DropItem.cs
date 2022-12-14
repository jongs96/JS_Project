using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Transform DropPos =null;
    public GameObject myChild = null;
    public ItemInfo iteminfo = null;
    // Start is called before the first frame update
    void Start()
    {
        myChild = Instantiate(iteminfo.Resource, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
