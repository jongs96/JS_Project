using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotion : MonoBehaviour
{
    Vector3 moveDelta = Vector3.zero;
    Coroutine coRot = null;
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
    public void RotCharactor(Transform transform, Transform target)
    {
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, target));
    }
    public static IEnumerator Rotating(Transform transform, Transform cam)
    {
        Vector3 rot;
        Vector3 camRot;
        do
        {
            rot = transform.rotation.eulerAngles;//quaternion값인 rotation을 euler값으로 바꿔 계산.
            camRot = cam.rotation.eulerAngles;
            rot.y = Mathf.Lerp(rot.y, camRot.y, Time.deltaTime * 10.0f);
            transform.rotation = Quaternion.Euler(rot);//euler로 계산한 값을 quaternion으로 바꿔 대입

            yield return null;
        }
        while (!Mathf.Approximately(rot.y, camRot.y));
    }
}
