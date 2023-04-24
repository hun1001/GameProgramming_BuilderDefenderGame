using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum State
    {
        Spawning,
        Invincible,
        Common
    }

    private State state = State.Common;
    private float stateTimer = 0f;

    private void SetState(State state)
    {
        switch (state)
        {
            case State.Spawning:
                stateTimer = 3f;
                break;
            case State.Invincible:
                stateTimer = 3f;
                break;
            case State.Common:
                stateTimer = 3f;
                break;
        }
        this.state = state;
    }

    private void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    private void Spawning()
    {
    }

    private void Invincible()
    {
    }

    private void Common()
    {
    }
}
