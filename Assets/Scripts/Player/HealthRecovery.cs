using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    private HealthSystem healthSystem = null;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        StartCoroutine(RecoverHealth());
    }

    private IEnumerator RecoverHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            healthSystem.Heal(1);
        }
    }
}
