using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{    
    public Transform myParents;
        
    // Start is called before the first frame update
    void Start()
    {
        myParents = transform.parent;
    }
    
    public void ChangeChildLocation()
    {
        Button[] buttons = UIManager.Inst.Inventory.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; ++i)
        {
            if (buttons[i].name == gameObject.name)
            {
                buttons[i].transform.SetAsLastSibling();
                break;
            }
        }
        Transform screen = myParents.Find("__Screen__");
        screen.SetAsLastSibling();
        transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
