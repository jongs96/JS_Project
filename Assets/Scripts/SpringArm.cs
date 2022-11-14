using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    public Transform myCam;
    public LayerMask CrashMask = default;
    public float rotSpeed = 5.0f;
    public float zoomSpeed = 3.0f;
    public Vector2 RotateRange = new Vector2(-70, 80);
    Vector2 curRot = Vector2.zero;
    Vector2 desireRot = Vector2.zero;
    public float SmoothRotSpeed = 3.0f;
    public Vector2 ZoomRange = new Vector2(1.5f, 10.0f);
    public float SmoothDistSpeed = 3.0f;
    float desireDist = 0.0f;
    float curCamDist = 0.0f;
    float OffsetDist = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Fix the cursor inside the window
        Cursor.lockState = CursorLockMode.Locked;

        desireRot.x = curRot.x = transform.localRotation.eulerAngles.x;
        desireDist = curCamDist = -myCam.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        //camera x-axis rotation
        desireRot.x += -Input.GetAxisRaw("Mouse Y") * rotSpeed;
        desireRot.x = Mathf.Clamp(desireRot.x, RotateRange.x, RotateRange.y);

        //y-axis rotation camera only
        if (Input.GetMouseButton(1))
        {
            desireRot.y += Input.GetAxisRaw("Mouse X") * rotSpeed;
        }
        //y-axis rotation with charactor
        else
        {
            transform.parent.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * rotSpeed);
        }
        curRot = Vector2.Lerp(curRot, desireRot, Time.deltaTime * SmoothRotSpeed);

        Quaternion x = Quaternion.Euler(new Vector3(curRot.x, 0, 0));
        Quaternion y = Quaternion.Euler(new Vector3(0, curRot.y, 0));
        transform.localRotation = y * x;
        
        //if (!Input.GetMouseButton(1)) transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x, 0, 0));

        //camera z-axis move
        if (Input.GetAxisRaw("Mouse ScrollWheel") > Mathf.Epsilon || Input.GetAxisRaw("Mouse ScrollWheel") < -Mathf.Epsilon)
        {
            desireDist += Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
            desireDist = Mathf.Clamp(desireDist, ZoomRange.x, ZoomRange.y);
        }
        curCamDist = Mathf.Lerp(curCamDist, desireDist, Time.deltaTime * SmoothDistSpeed);

        //avoid camera crash
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = -transform.forward;
        float checkDist = Mathf.Min(curCamDist, desireDist);
        if (Physics.Raycast(ray, out RaycastHit hit, checkDist + OffsetDist + 0.1f, CrashMask))
        {
            curCamDist = Vector3.Distance(transform.position, hit.point + myCam.forward * OffsetDist);
        }

        myCam.transform.localPosition = new Vector3(0, 0, -curCamDist);
    }
}