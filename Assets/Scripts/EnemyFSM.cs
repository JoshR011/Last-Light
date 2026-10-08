using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Switches between waiting and chasing based on the sight sensor and stopping distance.
public class EnemyFSM : MonoBehaviour
{
    // These states describe the movement behaviors available to the enemy.
    public enum EnemyState
    {
        Stop,
        MoveToPlayer
    }

    // Set the visibility sensor and distance at which the enemy should stop approaching.
    public Sight sightSensor;
    public float stopDistance = 1.5f;
    public EnemyState currentState = EnemyState.Stop;

    // Cache the navigation component used by both movement states.
    private UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        // The textbook puts this script on the AI child and the agent on Enemy.
        agent = GetComponentInParent<UnityEngine.AI.NavMeshAgent>();

        // Use the local sight sensor when one has not been assigned in the Inspector.
        if (sightSensor == null)
            sightSensor = GetComponent<Sight>();

        // Disable this behavior if required components are missing, avoiding invalid updates.
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

        // Keep navigation's stopping distance in sync and read the currently visible target.
        agent.stoppingDistance = stopDistance;
        Collider player = sightSensor.detectedObject;

        // Wait when no player is visible or the player is close enough; otherwise chase.
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
        // Pause navigation without clearing the agent's existing destination.
        agent.isStopped = true;
    }

    private void MoveToPlayer(Transform player)
    {
        // Resume navigation and update the destination as the player moves.
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }
}
