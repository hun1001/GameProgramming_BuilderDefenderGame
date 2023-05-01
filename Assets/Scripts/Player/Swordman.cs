using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : PlayerController
{
    private void Awake()
    {
        m_HealthSystem.OnDied += (_, _) =>
        {
            m_Anim.Play("Die");
            StartCoroutine(DieCoroutine());
        };
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
        GameOverUI.Instance.Show();
    }

    private void Update()
    {
        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -115, 115);
        pos.y = Mathf.Clamp(pos.y, -65, 65);
        transform.position = pos;
    }

    public void checkInput()
    {
        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Sit") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            return;
        }

        m_MoveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_Anim.Play("Attack");
            }
            else
            {
                if (m_MoveDir.magnitude <= 0)
                {
                    m_Anim.Play("Idle");
                }
                else
                {
                    m_Anim.Play("Run");
                }
            }
        }

        transform.transform.Translate(new Vector3(m_MoveDir.x * MoveSpeed * Time.deltaTime, m_MoveDir.y * MoveSpeed * Time.deltaTime, 0));

        if (Input.GetKey(KeyCode.D))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;
        }

        if (UtilsClass.GetMouseWorldPosition().x < transform.position.x)
        {
            Flip(true);
        }
        else
        {
            Flip(false);
        }
    }

    public override void Attack()
    {
        m_AttackCollision.gameObject.SetActive(true);
        m_AttackCollision.OnAttack(transform, transform.localScale.x == -1);
    }
}
