/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Chases a visible player and deals timed melee damage while within reach.
 */

using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Stop,
        MoveToPlayer,
        Attack
    }

    [Header("Player Detection")]
    public Sight sightSensor;

    [Header("Melee Attack")]
    [Min(0.1f)]
    public float stopDistance = 1.5f;
    [Min(0f)]
    public float attackDamage = 10f;
    [Min(0.01f)]
    public float attackInterval = 1f;

    [Header("Current Behavior")]
    public EnemyState currentState = EnemyState.Stop;

    private NavMeshAgent agent;
    private Life enemyLife;
    private float nextAttackTime;

    private void Awake()
    {
        // The AI child uses the navigation agent on its parent Enemy object.
        agent = GetComponentInParent<NavMeshAgent>();
        enemyLife = GetComponentInParent<Life>();

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
        agent.stoppingDistance = Mathf.Max(0.1f, stopDistance);
        Collider player = sightSensor.detectedTarget;
        Life playerLife = player != null ? player.GetComponentInParent<Life>() : null;

        // Decide first so losing sight of the player stops pursuit in this same update.
        UpdateState(playerLife);
        ExecuteCurrentState(playerLife);
    }

    private void UpdateState(Life player)
    {
        // A dead enemy or player must stop fighting even before deferred destruction finishes.
        if ((enemyLife != null && enemyLife.currentHealth <= 0f)
            || player == null || !player.CompareTag("Player") || player.currentHealth <= 0f)
        {
            currentState = EnemyState.Stop;
        }
        // A small tolerance accounts for the agent settling just outside its requested stopping distance.
        else if (Vector3.Distance(agent.transform.position, player.transform.position) <= agent.stoppingDistance + 0.05f)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.MoveToPlayer;
        }
    }

    private void ExecuteCurrentState(Life player)
    {
        switch (currentState)
        {
            case EnemyState.Stop:
                StopMoving();
                break;

            case EnemyState.MoveToPlayer:
                ChasePlayer(player.transform);
                break;

            case EnemyState.Attack:
                StopMoving();
                AttackPlayer(player);
                break;
        }
    }

    private void AttackPlayer(Life player)
    {
        // Keep the cooldown when a target leaves reach so moving back in cannot cause rapid extra hits.
        if (Time.time < nextAttackTime) return;
        // Recheck obstacles at the moment of impact because the sight result can be from the previous update.
        if (Physics.Linecast(sightSensor.transform.position, sightSensor.detectedTarget.bounds.center,
                sightSensor.obstacleLayers)) return;
        nextAttackTime = Time.time + Mathf.Max(0.01f, attackInterval);
        // Damage is applied once per attack, rather than once per frame spent beside the player.
        player.TakeDamage(attackDamage);
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
