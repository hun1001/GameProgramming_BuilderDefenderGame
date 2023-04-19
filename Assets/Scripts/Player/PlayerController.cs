using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    protected Vector2 m_MoveDir;
    [SerializeField]
    protected Rigidbody2D m_rigidbody;

    [SerializeField]
    protected BoxCollider2D m_BoxCollider;

    [SerializeField]
    protected Animator m_Anim;

    [SerializeField]
    protected AttackCollision m_AttackCollision;

    [SerializeField]
    protected HealthSystem m_HealthSystem;

    [Header("[Setting]")]
    public float MoveSpeed = 6;

    protected void Flip(bool bLeft)
    {
        transform.localScale = new Vector3(bLeft ? 1 : -1, 1, 1);
    }

    public abstract void Attack();
}
