using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController = null;

    public void OnAttack()
    {
        playerController.Attack();
    }
}
