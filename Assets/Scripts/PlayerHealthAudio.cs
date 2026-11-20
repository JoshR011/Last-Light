/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-19
 * Date Last Updated: 2026-11-20
 * Summary: Plays audio for damage, healing, low health, and player death.
 */

using UnityEngine;

[RequireComponent(typeof(Life))]
public class PlayerHealthAudio : MonoBehaviour
{
    [Header("Damage and Healing")]
    [SerializeField] private AudioSource feedbackSound;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip damageClip;

    [Header("Low Health")]
    [SerializeField] private AudioSource heartbeatSound;

    [Header("Death")]
    [SerializeField] private AudioSource deathSoundPrefab;

    private const float LowHealthThreshold = 20f;
    private Life playerLife;

    private void Awake()
    {
        playerLife = GetComponent<Life>();
    }

    private void OnEnable()
    {
        playerLife.Damaged += PlayDamage;
    }

    private void OnDisable()
    {
        playerLife.Damaged -= PlayDamage;
        if (heartbeatSound != null)
        {
            heartbeatSound.Stop();
        }
    }

    private void Update()
    {
        if (heartbeatSound == null)
        {
            return;
        }

        // Let the heartbeat loop play through without restarting it every frame.
        bool lowHealth = playerLife.currentHealth > 0f && playerLife.currentHealth <= LowHealthThreshold;
        if (lowHealth && !heartbeatSound.isPlaying)
        {
            heartbeatSound.Play();
        }
        else if (!lowHealth && heartbeatSound.isPlaying)
        {
            heartbeatSound.Stop();
        }
    }

    public void PlayHeal()
    {
        PlayFeedback(healClip);
    }

    private void PlayDamage()
    {
        // The death response plays its own cue for a fatal hit.
        if (playerLife.currentHealth > 0f)
        {
            PlayFeedback(damageClip);
        }
    }

    private void PlayFeedback(AudioClip clip)
    {
        if (isActiveAndEnabled && feedbackSound != null && clip != null)
        {
            feedbackSound.clip = clip;
            feedbackSound.Play();
        }
    }

    public void PlayDeath()
    {
        if (!isActiveAndEnabled || deathSoundPrefab == null || deathSoundPrefab.clip == null)
        {
            return;
        }

        AudioListener playerListener = GetComponentInChildren<AudioListener>();
        AudioSource cue = Instantiate(deathSoundPrefab,
            playerListener != null ? playerListener.transform.position : transform.position,
            Quaternion.identity);

        // Death removes the player camera and its listener. Keep the cue audible until it finishes.
        if (playerListener != null && playerListener.isActiveAndEnabled)
        {
            playerListener.enabled = false;
            cue.gameObject.AddComponent<AudioListener>();
        }

        cue.Play();
        Destroy(cue.gameObject, cue.clip.length);
    }
}
