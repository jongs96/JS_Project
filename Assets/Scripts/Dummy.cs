using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : CharacterProperty, IBattle
{
    public bool IsLive { get { return true; } }
    public Transform myHeadPos;
    public Transform HeadPos { get => myHeadPos; }
    public void OnDamage(float dmg, Transform target)
    {
        StartCoroutine(TakeHit());
    }
    IEnumerator TakeHit()
    {
        Renderer render = myRenderer;
        render.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(0.4f);
        render.material.SetColor("_Color", Color.white);
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
