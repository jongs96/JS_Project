using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLongAttack : MonoBehaviour
{
    Vector3 StartAtPos = new Vector3(-1.5f, 0.0f, 7.5f);
    Vector3 EndAtPos = new Vector3(1.5f, 0.0f, 7.5f);
    public MeshFilter myFilter;
    public float Angle;
    public float Distance = 14.0f;
    public int DetailCount = 100;
    public bool AttackStart = false;
    Vector3[] Dirs = null;
    Vector3 leftDir = Vector3.zero;
    Vector3 rightDir = Vector3.zero;
    public Boss boss;

    void Start()
    {
        Angle = Vector3.Angle(StartAtPos, EndAtPos);        
        Vector3[] vb = new Vector3[DetailCount + 1];
        int[] ib = new int[(DetailCount - 1) * 3];        
        Dirs = new Vector3[DetailCount];
        Vector3 dir = -transform.forward * Distance;
        Dirs[0] = Quaternion.AngleAxis(-Angle / 2.0f, Vector3.up) * dir;

        float angleGap = Angle / (DetailCount - 1);

        vb[0] = Vector3.zero;
        for (int i = 1; i < vb.Length; ++i)
        {
            vb[i] = vb[0] + Dirs[i - 1];
            if (i < DetailCount) Dirs[i] = Quaternion.AngleAxis(angleGap, Vector3.up) * Dirs[i - 1];

            if (i >= 2)
            {
                ib[(i - 2) * 3] = 0;
                ib[(i - 2) * 3 + 1] = i - 1;
                ib[(i - 2) * 3 + 2] = i;
            }
        }

        Mesh _mesh = new Mesh();
        _mesh.vertices = vb;
        _mesh.triangles = ib;
        myFilter.mesh = _mesh;

        rightDir = Vector3.Cross(vb[1] - transform.position, Vector3.up).normalized;
        leftDir = Vector3.Cross(Vector3.up, vb[DetailCount] - transform.position).normalized;
    }
    private void FixedUpdate()
    {
        Vector3[] vb = myFilter.mesh.vertices;
        for (int i = 0; i < Dirs.Length; ++i)
        {
            vb[i + 1] = vb[0] + Dirs[i].normalized * Distance;
        }
        myFilter.mesh.vertices = vb;
        if(AttackStart)
        {
            bool OnRange = false;
            Vector3 dir = (boss.myTarget.position - vb[1]).normalized;
            Vector3 dir2 = (boss.myTarget.position - vb[DetailCount]).normalized;
            if (Vector3.Dot(dir, rightDir) > 0.0f && Vector3.Dot(dir2, leftDir) > 0.0f) OnRange = true;
            if (OnRange)
            {
                IBattle ib = boss.myTarget.GetComponent<IBattle>();
                ib?.OnDamage(boss.bossStat.AttackPower, boss.transform);
                AttackStart = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IBattle ib = other.gameObject.GetComponent<IBattle>();
        if (ib != null)
        {
            ib?.OnDamage(boss.bossStat.AttackPower, boss.transform);
        }
    }
}
