using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour
{
    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
                if (_rigid == null) _rigid = GetComponentInChildren<Rigidbody>();
            }
            return _rigid;
        }
    }

    Animator _anim = null;
    protected Animator myAnim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
                if (_anim == null) _anim = GetComponentInChildren<Animator>();
            }
            return _anim;
        }
    }

    Renderer _render = null;
    protected Renderer myRenderer
    {
        get
        {
            if (_render == null)
            {
                _render = GetComponent<Renderer>();
                if (_render == null) _render = GetComponentInChildren<Renderer>();
            }
            return _render;
        }
    }

    Collider _collider = null;
    protected Collider myCollider
    {
        get
        {
            if (_collider == null)
            {
                _collider = GetComponent<Collider>();
                if (_collider == null) _collider = GetComponentInChildren<Collider>();
            }
            return _collider;
        }
    }
}
