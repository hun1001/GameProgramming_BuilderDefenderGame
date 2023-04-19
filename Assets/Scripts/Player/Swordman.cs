using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : PlayerController
{
    private void Update()
    {
        checkInput();

        if (m_rigidbody.velocity.magnitude > 30)
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x - 0.1f, m_rigidbody.velocity.y - 0.1f);
        }
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

        // 기타 이동 인풋.
        if (Input.GetKey(KeyCode.D))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.A))
                Flip(false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                return;

            if (!Input.GetKey(KeyCode.D))
                Flip(true);
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
    }
}
