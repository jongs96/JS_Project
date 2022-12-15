using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst = null;
    public GameObject PauseMenu;
    public GameObject Menu;
    public GameObject Equip;
    public GameObject Inventory;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Inst != null)
            Destroy(gameObject);
        Inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
