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
    public Transform hpBars;
    public Transform InfoText;
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
        abil[1].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurExp)}/{ DataManager.Inst.PlayerStatData.MaxExp}";
        abil[2].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurHP)}/{DataManager.Inst.PlayerStatData.MaxHP}";
        abil[3].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurEnergy)}/{ DataManager.Inst.PlayerStatData.MaxEnergy}";
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
            case "Level":
                abil[0].statValue.text = DataManager.Inst.PlayerStatData.Level.ToString();
                break;
            case "Exp":
                abil[1].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurExp)}/{ DataManager.Inst.PlayerStatData.MaxExp}";
                break;
            case "HP":
                abil[2].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurHP)}/{DataManager.Inst.PlayerStatData.MaxHP}";
                break;
            case "Energy":
                abil[3].statValue.text = $"{Mathf.Floor(DataManager.Inst.PlayerStatData.CurEnergy)}/{ DataManager.Inst.PlayerStatData.MaxEnergy}";
                break;
            case "AttackPower":
                abil[4].statValue.text = DataManager.Inst.PlayerStatData.AttackPower.ToString();
                break;
            case "DefensePower":
                abil[5].statValue.text = DataManager.Inst.PlayerStatData.DefensePower.ToString();
                break;
            case "CriticalRate":
                abil[6].statValue.text = DataManager.Inst.PlayerStatData.CriticalRate.ToString();
                break;
            case "Point":
                abil[7].statValue.text = DataManager.Inst.PlayerStatData.Point.ToString();
                break;
            case "Strength":
                abil[8].statValue.text = DataManager.Inst.PlayerStatData.Strength.ToString();
                break;
            case "Agility":
                abil[9].statValue.text = DataManager.Inst.PlayerStatData.Agility.ToString();
                break;
            case "Evation":
                abil[10].statValue.text = DataManager.Inst.PlayerStatData.Evation.ToString();
                break;
            default:
                break;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
