using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private HealthSystem healthSystem = null;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDied += (_, _) =>
        {
            Destroy(gameObject);
        };
    }

    public void Spawning()
    {
        Enemy.Create(this.transform.position + UtilsClass.GetRandomDir() * UnityEngine.Random.Range(0f, 10f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerAttackCollision"))
        {
            healthSystem.Damage(20);
        }
    }
}
