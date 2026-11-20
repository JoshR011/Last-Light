/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-20
 * Date Last Updated: 2026-11-20
 * Summary: Spawns zombies on the NavMesh near the player and caps the number it maintains.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningManager : MonoBehaviour
{
    private const int SpawnAttempts = 16;
    private const float NavMeshSampleDistance = 2f;
    private const float MinimumSpawnDistance = 1.5f;

    [Header("Zombie Spawning")]
    [SerializeField] private NavMeshAgent zombiePrefab;
    [SerializeField, Min(2f), Tooltip("Maximum horizontal distance from the player in world units.")]
    private float spawnRadius = 10f;
    [SerializeField, Min(0.1f)] private float spawnInterval = 5f;
    [SerializeField, Min(1)] private int maxAlive = 6;

    private readonly List<NavMeshAgent> spawnedZombies = new List<NavMeshAgent>();
    private Transform player;
    private int characterLayers;
    private float nextSpawnTime;

    private void Awake()
    {
        characterLayers = LayerMask.GetMask("Enemy", "Player");
    }

    private void OnEnable()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time < nextSpawnTime || zombiePrefab == null)
        {
            return;
        }

        nextSpawnTime = Time.time + spawnInterval;
        // Remove destroyed zombies so their spawn slots can be reused.
        spawnedZombies.RemoveAll(zombie => zombie == null);
        if (spawnedZombies.Count >= maxAlive)
        {
            return;
        }

        if (player == null || !player.gameObject.activeInHierarchy)
        {
            player = GameObject.FindWithTag("Player")?.transform;
        }

        if (player != null && player.gameObject.scene == gameObject.scene)
        {
            TrySpawnZombie(player.position);
        }
    }

    private void TrySpawnZombie(Vector3 playerPosition)
    {
        var filter = new NavMeshQueryFilter
        {
            agentTypeID = zombiePrefab.agentTypeID,
            areaMask = zombiePrefab.areaMask
        };
        float maxDistanceSquared = spawnRadius * spawnRadius;
        float minDistanceSquared = MinimumSpawnDistance * MinimumSpawnDistance;

        for (int attempt = 0; attempt < SpawnAttempts; attempt++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 candidate = playerPosition + new Vector3(offset.x, 0f, offset.y);
            if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, NavMeshSampleDistance, filter))
            {
                continue;
            }

            Vector3 toPlayer = playerPosition - hit.position;
            // Use horizontal aim so the zombie stays upright when the player is uphill or downhill.
            toPlayer.y = 0f;
            float distanceSquared = toPlayer.sqrMagnitude;
            // Sampling can move a point across either distance limit.
            if (distanceSquared < minDistanceSquared || distanceSquared > maxDistanceSquared)
            {
                continue;
            }

            float zombieRadius = zombiePrefab.radius;
            Vector3 overlapCenter = hit.position + Vector3.up * zombieRadius;
            if (Physics.CheckSphere(overlapCenter, zombieRadius, characterLayers, QueryTriggerInteraction.Ignore))
            {
                continue;
            }

            Quaternion rotation = Quaternion.LookRotation(toPlayer, Vector3.up);
            // Spawn under this scene root so door travel suspends these zombies with the exterior.
            NavMeshAgent zombie = Instantiate(zombiePrefab, hit.position, rotation, transform);
            spawnedZombies.Add(zombie);
            return;
        }
    }
}
