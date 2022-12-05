using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject MobManager = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(MobManager != null)
        {

        }
        else
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                GetComponent<Animator>().SetTrigger("Open");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GetComponent<Animator>().SetTrigger("Close");
        }
    }
}
