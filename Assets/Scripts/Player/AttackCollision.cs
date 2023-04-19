using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnAttack(Transform t, bool isFlip)
    {
        gameObject.SetActive(true);
        transform.position = t.position;
        transform.localScale = new Vector3(isFlip ? -1 : 1, 1, 1);
        StartCoroutine(OnAttackEnd());
    }

    private IEnumerator OnAttackEnd()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
    }
}
