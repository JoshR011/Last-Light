using UnityEngine;

[RequireComponent(typeof(Life))]
public class PlayerHealthAudio : MonoBehaviour
{
    [SerializeField] private AudioSource feedbackSound;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioSource heartbeatSound;
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
