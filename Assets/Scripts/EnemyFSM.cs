/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-10-19
 * Summary: Switches an enemy between waiting and chasing a visible player.
 */

using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Stop,
        MoveToPlayer
    }

    [Header("Player Detection")]
    public Sight sightSensor;

    [Header("Navigation")]
    public float stopDistance = 1.5f;

    [Header("Current Behavior")]
    public EnemyState currentState = EnemyState.Stop;

    private NavMeshAgent agent;

    private void Awake()
    {
        // The AI child uses the navigation agent on its parent Enemy object.
        agent = GetComponentInParent<NavMeshAgent>();

        // Allow an Inspector override while making the usual local sensor work without wiring.
        if (sightSensor == null)
        {
            sightSensor = GetComponent<Sight>();
        }

        // Report setup errors once instead of failing again on every frame.
        if (agent == null || sightSensor == null)
        {
            Debug.LogError("EnemyFSM needs a NavMeshAgent and a Sight component.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        // Navigation calls fail until the agent is on a baked NavMesh.
        if (!agent.isOnNavMesh)
        {
            currentState = EnemyState.Stop;
            return;
        }

        // Match navigation's stopping threshold to the state transition to avoid conflicting distances.
        agent.stoppingDistance = stopDistance;
        Collider player = sightSensor.detectedTarget;

        // Decide first so losing sight of the player stops pursuit in this same update.
        UpdateState(player);
        ExecuteCurrentState(player);
    }

    private void UpdateState(Collider player)
    {
        // A missing target must be checked before its transform can be used for the distance test.
        if (player == null || Vector3.Distance(
                agent.transform.position, player.transform.position) <= stopDistance)
        {
            currentState = EnemyState.Stop;
        }
        else
        {
            currentState = EnemyState.MoveToPlayer;
        }
    }

    private void ExecuteCurrentState(Collider player)
    {
        switch (currentState)
        {
            case EnemyState.Stop:
                StopMoving();
                break;

            case EnemyState.MoveToPlayer:
                ChasePlayer(player.transform);
                break;
        }
    }

    private void StopMoving()
    {
        // Pausing the agent keeps its existing path available if pursuit resumes.
        agent.isStopped = true;
    }

    private void ChasePlayer(Transform player)
    {
        // A previous Stop state may have paused the agent even though it still has a path.
        agent.isStopped = false;
        // Refresh the destination because the player can move after a path has been calculated.
        agent.SetDestination(player.position);
    }
}
