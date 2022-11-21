using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MyAction();
public delegate void MyAction<T>(T t);

public class MobMovement : CharacterProperty
{
    Coroutine coMove = null;
    Coroutine coRot = null;

    protected void MoveToPos(Vector3 target, float movSpeed, float rotSpeed, MyAction done = null)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove = StartCoroutine(MovingToPos(transform, target, movSpeed, done));
        if (coRot!= null) StopCoroutine(coRot);
        coRot = StartCoroutine(Rotating(transform, target, rotSpeed));
    }

    public static IEnumerator Rotating(Transform transform, Vector3 target, float rotSpeed)
    {
        Vector3 dir = target - transform.position;
        if (dir.magnitude <= Mathf.Epsilon) yield break;
        dir.Normalize();

        float d = Vector3.Dot(dir, transform.forward);
        float r = Mathf.Acos(d);
        float angle = r * Mathf.Rad2Deg;

        if (angle > Mathf.Epsilon)
        {
            float rotDir = Vector3.Dot(dir, transform.right) >= 0.0f ? 1.0f : -1.0f;
            while (angle > Mathf.Epsilon)
            {
                float delta = rotSpeed * Time.deltaTime;
                if(delta > angle)
                {
                    delta = angle;
                }
                angle -= delta;
                transform.Rotate(Vector3.up * rotDir * delta, Space.World);
                yield return null;
            }
        }
    }
    public IEnumerator MovingToPos(Transform transform, Vector3 target, float movSpeed, MyAction done = null)
    {
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();

        myAnim.SetBool("IsMoving", true);

        while (dist > Mathf.Epsilon)
        {
            if(!myAnim.GetBool("IsAttacking"))
            {
                float delta = movSpeed * Time.deltaTime;
                if(delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                break;
            }
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }

}
