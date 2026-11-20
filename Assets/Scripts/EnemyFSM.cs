/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Chases a visible player and deals timed melee damage while within reach.
 */

using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    private static readonly int MoveSpeedParameter = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int IdleActionTrigger = Animator.StringToHash("IdleAction");

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
    [SerializeField, Min(0f)] private float attackWindup = 0.3f;

    [Header("Zombie Audio")]
    public AudioSource voiceSource;
    public AudioSource attackSource;
    public AudioClip[] idleClips;
    public AudioClip[] chaseClips;
    public AudioClip[] attackClips;

    [Header("Current Behavior")]
    public EnemyState currentState = EnemyState.Stop;

    private NavMeshAgent agent;
    private Animator animator;
    private Life enemyLife;
    private Life pendingAttackTarget;
    private float attackImpactTime;
    private float nextAttackTime;
    private EnemyState previousVoiceState = EnemyState.Stop;
    private float nextVoiceTime;
    private float nextAttackSoundTime;

    private void Awake()
    {
        // The AI child uses the navigation agent on its parent Enemy object.
        agent = GetComponentInParent<NavMeshAgent>();
        enemyLife = GetComponentInParent<Life>();
        animator = GetComponentInParent<Animator>();
        if (animator != null && animator.runtimeAnimatorController == null)
        {
            animator = null;
        }

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

    private void OnEnable()
    {
        nextVoiceTime = Time.time + Random.Range(6f, 16f);
        nextAttackSoundTime = Time.time;
    }

    private void OnDisable()
    {
        pendingAttackTarget = null;
        if (voiceSource != null)
        {
            voiceSource.Stop();
        }
        if (attackSource != null)
        {
            attackSource.Stop();
        }
        previousVoiceState = EnemyState.Stop;
    }

    private void Update()
    {
        // Navigation calls fail until the agent is on a baked NavMesh.
        if (!agent.isOnNavMesh)
        {
            currentState = EnemyState.Stop;
            pendingAttackTarget = null;
            animator?.SetFloat(MoveSpeedParameter, 0f);
            UpdateZombieVoice();
            return;
        }

        // Match navigation's stopping threshold to the state transition to avoid conflicting distances.
        agent.stoppingDistance = Mathf.Max(0.1f, stopDistance);
        Collider player = sightSensor.detectedTarget;
        Life playerLife = player != null ? player.GetComponentInParent<Life>() : null;

        // Decide first so losing sight of the player stops pursuit in this same update.
        UpdateState(playerLife);
        if (currentState != EnemyState.Attack)
        {
            pendingAttackTarget = null;
        }
        UpdateZombieVoice();
        ExecuteCurrentState(playerLife);
        animator?.SetFloat(MoveSpeedParameter, currentState == EnemyState.MoveToPlayer ? agent.velocity.magnitude : 0f);
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
                player.GetComponent<AdaptiveMusic>()?.RecordCombat();
                AttackPlayer(player);
                break;
        }
    }

    private void UpdateZombieVoice()
    {
        if (voiceSource == null)
        {
            return;
        }
        if (enemyLife != null && enemyLife.currentHealth <= 0f)
        {
            voiceSource.Stop();
            return;
        }

        bool chasing = currentState == EnemyState.MoveToPlayer;
        if (currentState != previousVoiceState)
        {
            previousVoiceState = currentState;
            nextVoiceTime = Time.time + (chasing ? Random.Range(0.45f, 1.5f) : Random.Range(10f, 20f));
        }

        if (currentState == EnemyState.Attack || Time.time < nextVoiceTime || voiceSource.isPlaying ||
            (attackSource != null && attackSource.isPlaying))
        {
            return;
        }

        float volume = chasing ? Random.Range(0.18f, 0.28f) : Random.Range(0.075f, 0.115f);
        float duration = PlayRandomVocal(voiceSource, chasing ? chaseClips : idleClips, volume);
        if (!chasing && duration > 0f)
        {
            animator?.SetTrigger(IdleActionTrigger);
        }
        float silence = chasing ? Random.Range(2.5f, 5f) : Random.Range(10f, 20f);
        nextVoiceTime = Time.time + duration + silence;
    }

    private static float PlayRandomVocal(AudioSource source, AudioClip[] clips, float volume)
    {
        if (clips == null || clips.Length == 0)
        {
            return 0f;
        }

        int choice = Random.Range(0, clips.Length);
        if (clips.Length > 1 && clips[choice] == source.clip)
        {
            choice = (choice + Random.Range(1, clips.Length)) % clips.Length;
        }

        AudioClip clip = clips[choice];
        if (clip == null)
        {
            return 0f;
        }

        source.clip = clip;
        source.volume = volume;
        source.pitch = Random.Range(0.94f, 1.06f);
        source.Play();
        return clip.length / source.pitch;
    }

    private void AttackPlayer(Life player)
    {
        if (pendingAttackTarget != null)
        {
            if (Time.time < attackImpactTime) return;
            Life target = pendingAttackTarget;
            pendingAttackTarget = null;
            if (target == player && HasClearAttackPath())
            {
                ApplyAttack(player);
            }
            return;
        }

        // Keep the cooldown when a target leaves reach so moving back in cannot cause rapid extra hits.
        if (Time.time < nextAttackTime || !HasClearAttackPath()) return;
        float cooldown = Mathf.Max(0.01f, attackInterval);
        nextAttackTime = Time.time + cooldown;
        if (animator == null || !animator.isActiveAndEnabled)
        {
            ApplyAttack(player);
            return;
        }

        animator.SetTrigger(AttackTrigger);
        if (attackWindup <= 0f)
        {
            ApplyAttack(player);
            return;
        }
        pendingAttackTarget = player;
        attackImpactTime = Time.time + Mathf.Min(attackWindup, cooldown);
    }

    private bool HasClearAttackPath()
    {
        // Sight may have run before a door closed or a target moved behind cover this frame.
        return !Physics.Linecast(sightSensor.transform.position, sightSensor.detectedTarget.bounds.center,
            sightSensor.obstacleLayers);
    }

    private void ApplyAttack(Life player)
    {
        float healthBeforeHit = player.currentHealth;
        player.TakeDamage(attackDamage);
        if (attackSource != null && player.currentHealth < healthBeforeHit &&
            Time.time >= nextAttackSoundTime && !attackSource.isPlaying)
        {
            if (voiceSource != null)
            {
                voiceSource.Stop();
            }
            float duration = PlayRandomVocal(attackSource, attackClips, Random.Range(0.24f, 0.34f));
            nextAttackSoundTime = Time.time + duration + Random.Range(1.1f, 2.4f);
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
