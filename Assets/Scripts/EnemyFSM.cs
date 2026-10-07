using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Stop,
        MoveToPlayer
    }

    public Sight sightSensor;
    public float stopDistance = 1.5f;
    public EnemyState currentState = EnemyState.Stop;

    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        // The textbook puts this script on the AI child and the agent on Enemy.
        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

        if (sightSensor == null)
            sightSensor = GetComponent<Sight>();

        if (agent == null || sightSensor == null)
        {
            Debug.LogError("EnemyFSM needs a NavMeshAgent and a Sight component.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        // Agent movement calls require the enemy to be on a baked NavMesh.
        if (!agent.isOnNavMesh)
        {
            currentState = EnemyState.Stop;
            return;
        }

        agent.stoppingDistance = stopDistance;
        Collider player = sightSensor.detectedObject;

        // Choose the state from visibility and distance to the player.
        if (player == null || Vector3.Distance(
                agent.transform.position, player.transform.position) <= stopDistance)
            currentState = EnemyState.Stop;
        else
            currentState = EnemyState.MoveToPlayer;

        // Execute the current state's behavior.
        switch (currentState)
        {
            case EnemyState.Stop:
                Stop();
                break;

            case EnemyState.MoveToPlayer:
                MoveToPlayer(player.transform);
                break;
        }
    }

    private void Stop()
    {
        agent.isStopped = true;
    }

    private void MoveToPlayer(Transform player)
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
}
