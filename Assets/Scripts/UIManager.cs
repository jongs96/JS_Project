using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst = null;
    public GameObject PauseMenu;
    public GameObject Menu;
    public GameObject Equip;
    public GameObject Inventory;
    public Inventory[] invenLabels;//type

    public ScrollRect scrRect;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Inst != null)
            Destroy(gameObject);
        Inst = this;
    }
    void Start()
    {
        invenLabels = Inventory.transform.GetComponentsInChildren<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
