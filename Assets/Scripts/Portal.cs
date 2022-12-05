using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public GameObject bg;
    public Transform TargetPos;
    Transform player;
    Coroutine co = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Teleportation()
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(SwitchPosition());
    }
    IEnumerator SwitchPosition()
    {
        bg.SetActive(true);
        Color color = bg.GetComponent<Image>().color;
        while (bg.GetComponent<Image>().color.a < 1.0f)
        {
            float delta = Time.deltaTime;
            color.a += delta;
            bg.GetComponent<Image>().color = color;
            yield return null;
        }
        player.position = TargetPos.position;
        while (bg.GetComponent<Image>().color.a > 0.5f)
        {
            float delta = Time.deltaTime;
            color.a -= delta;
            bg.GetComponent<Image>().color = color;
            yield return null;
        }
        bg.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = collision.transform;
        }
    }
}
