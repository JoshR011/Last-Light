/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Flashes a URP vignette red on damage or green on healing, then fades it away.
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Life))]
public class PlayerDamageFeedback : MonoBehaviour
{
    [Header("Damage Flash Color")]
    public Color flashColor = new Color(0.9f, 0.02f, 0.02f, 1f);
    [Header("Healing Flash Color")]
    public Color healFlashColor = new Color(0.02f, 0.9f, 0.02f, 1f);

    [Header("Shared Flash Strength")]
    [Tooltip("Peak vignette intensity for damage and healing.")]
    [Range(0f, 1f)]
    public float flashOpacity = 0.8f;

    [Header("Damage Flash Timing")]
    [Min(0.01f)]
    public float fadeDuration = 0.7f;

    [Header("Vignette Edges")]
    [Tooltip("Softness of the vignette border; higher values make the transition more gradual.")]
    [Range(0.05f, 0.45f)]
    public float edgeWidth = 0.22f;

    private Life playerLife;
    private Volume feedbackVolume;
    private VolumeProfile feedbackProfile;
    private Vignette vignette;
    private float flashTimeRemaining;

    private void Awake()
    {
        playerLife = GetComponent<Life>();
        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogWarning("Player damage feedback needs a child camera.", this);
            enabled = false;
            return;
        }
        CreateVignette(playerCamera);
    }

    private void OnEnable()
    {
        if (feedbackVolume == null) return;
        // Subscribe only while active so disabling this feedback cannot leave stale hit listeners.
        playerLife.Damaged += ShowDamageFlash;
        feedbackVolume.enabled = true;
    }

    private void CreateVignette(Camera playerCamera)
    {
        UniversalAdditionalCameraData cameraData = playerCamera.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = true;
        cameraData.volumeLayerMask |= 1 << gameObject.layer;

        // Own a runtime profile so flashes never modify shared scene assets.
        feedbackProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        feedbackProfile.name = "Player Feedback Profile";
        feedbackProfile.hideFlags = HideFlags.DontSave;
        vignette = feedbackProfile.Add<Vignette>(true);
        vignette.intensity.value = 0f;
        vignette.smoothness.value = Mathf.Clamp(edgeWidth, 0.05f, 0.45f);

        GameObject volumeObject = new GameObject("Player Feedback Volume");
        volumeObject.transform.SetParent(transform, false);
        volumeObject.layer = gameObject.layer;
        feedbackVolume = volumeObject.AddComponent<Volume>();
        feedbackVolume.isGlobal = true;
        feedbackVolume.priority = 100f;
        feedbackVolume.weight = 0f;
        feedbackVolume.sharedProfile = feedbackProfile;
    }

    private void ShowDamageFlash()
    {
        ShowFlash(flashColor);
    }

    public void ShowHealFlash()
    {
        if (!isActiveAndEnabled) return;
        ShowFlash(healFlashColor);
    }

    private void ShowFlash(Color color)
    {
        if (vignette == null) return;
        // A fresh hit replaces the color and restarts the fade on the same vignette.
        flashTimeRemaining = Mathf.Max(0.01f, fadeDuration);
        vignette.color.value = color;
        vignette.intensity.value = Mathf.Clamp01(color.a * flashOpacity);
        feedbackVolume.weight = 1f;
    }

    private void Update()
    {
        if (flashTimeRemaining <= 0f) return;
        // Use real elapsed time so pausing or slowing gameplay cannot leave a flash stuck on screen.
        flashTimeRemaining = Mathf.Max(0f, flashTimeRemaining - Time.unscaledDeltaTime);
        float fade = Mathf.SmoothStep(0f, 1f, flashTimeRemaining / Mathf.Max(0.01f, fadeDuration));
        vignette.intensity.value = Mathf.Clamp01(vignette.color.value.a * flashOpacity) * fade;
        feedbackVolume.weight = flashTimeRemaining > 0f ? 1f : 0f;
    }

    private void OnDisable()
    {
        playerLife.Damaged -= ShowDamageFlash;
        flashTimeRemaining = 0f;
        if (feedbackVolume != null)
        {
            feedbackVolume.weight = 0f;
            feedbackVolume.enabled = false;
        }
        if (vignette != null) vignette.intensity.value = 0f;
    }

    private void OnDestroy()
    {
        // Release runtime objects even if only this component is removed from the player.
        if (feedbackVolume != null) Destroy(feedbackVolume.gameObject);
        if (vignette != null) Destroy(vignette);
        if (feedbackProfile != null) Destroy(feedbackProfile);
    }
}
