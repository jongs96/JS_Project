using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst = null;
    public GameObject PauseMenu;
    public GameObject Menu;
    public GameObject Equip;
    public GameObject Inventory;
    public Transform HotKeySlot;
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
        SetAbilityWindow();
    }
    public void SetAbilityWindow()// text°ª Ãâ·Â.
    {
        Ability[] abil = Equip.GetComponentsInChildren<Ability>();
        abil[0].statValue.text = DataManager.Inst.PlayerStatData.Level.ToString();
        abil[1].statValue.text = DataManager.Inst.PlayerStatData.CurExp.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxExp.ToString();
        abil[2].statValue.text = DataManager.Inst.PlayerStatData.CurHP.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxHP.ToString();
        abil[3].statValue.text = DataManager.Inst.PlayerStatData.CurEnergy.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxEnergy.ToString();
        abil[4].statValue.text = DataManager.Inst.PlayerStatData.AttackPower.ToString();
        abil[5].statValue.text = DataManager.Inst.PlayerStatData.DefensePower.ToString();
        abil[6].statValue.text = DataManager.Inst.PlayerStatData.CriticalRate.ToString();
        abil[7].statValue.text = DataManager.Inst.PlayerStatData.Point.ToString();
        abil[8].statValue.text = DataManager.Inst.PlayerStatData.Strength.ToString();
        abil[9].statValue.text = DataManager.Inst.PlayerStatData.Agility.ToString();
        abil[10].statValue.text = DataManager.Inst.PlayerStatData.Evation.ToString();        
    }
    public void SetAbility(string value)
    {
        Ability[] abil = Equip.GetComponentsInChildren<Ability>();
        switch(value)
        { 
        }
        abil[0].statValue.text = DataManager.Inst.PlayerStatData.Level.ToString();
        abil[1].statValue.text = DataManager.Inst.PlayerStatData.CurExp.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxExp.ToString();
        abil[2].statValue.text = DataManager.Inst.PlayerStatData.CurHP.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxHP.ToString();
        abil[3].statValue.text = DataManager.Inst.PlayerStatData.CurEnergy.ToString()
            + "/" + DataManager.Inst.PlayerStatData.MaxEnergy.ToString();
        abil[4].statValue.text = DataManager.Inst.PlayerStatData.AttackPower.ToString();
        abil[5].statValue.text = DataManager.Inst.PlayerStatData.DefensePower.ToString();
        abil[6].statValue.text = DataManager.Inst.PlayerStatData.CriticalRate.ToString();
        abil[7].statValue.text = DataManager.Inst.PlayerStatData.Point.ToString();
        abil[8].statValue.text = DataManager.Inst.PlayerStatData.Strength.ToString();
        abil[9].statValue.text = DataManager.Inst.PlayerStatData.Agility.ToString();
        abil[10].statValue.text = DataManager.Inst.PlayerStatData.Evation.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
