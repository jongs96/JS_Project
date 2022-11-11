using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    Vector3 moveDelta = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.parent.Translate(moveDelta, Space.World);
        moveDelta = Vector3.zero;
    }
    private void OnAnimatorMove()
    {
        moveDelta += GetComponent<Animator>().deltaPosition;
        transform.parent.Rotate(GetComponent<Animator>().deltaRotation.eulerAngles, Space.World);
    }
}
