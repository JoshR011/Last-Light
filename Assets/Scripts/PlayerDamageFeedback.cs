/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Switches between saved damage and healing Volume Profiles, then fades the active effect.
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Life))]
public class PlayerDamageFeedback : MonoBehaviour
{
    [Header("Feedback Profiles")]
    public VolumeProfile damageProfile;
    public VolumeProfile healProfile;

    [Header("Flash Timing")]
    [Min(0.01f)]
    public float fadeDuration = 0.7f;

    private Life playerLife;
    private Volume feedbackVolume;
    private float flashTimeRemaining;

    private void Awake()
    {
        playerLife = GetComponent<Life>();
        if (damageProfile == null || healProfile == null)
        {
            Debug.LogWarning("Player feedback needs damage and healing Volume Profiles.", this);
            enabled = false;
            return;
        }

        Camera playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogWarning("Player damage feedback needs a child camera.", this);
            enabled = false;
            return;
        }
        CreateFeedbackVolume(playerCamera);
    }

    private void OnEnable()
    {
        if (feedbackVolume == null) return;
        // Subscribe only while active so disabling this feedback cannot leave stale hit listeners.
        playerLife.Damaged += ShowDamageFlash;
    }

    private void CreateFeedbackVolume(Camera playerCamera)
    {
        UniversalAdditionalCameraData cameraData = playerCamera.GetUniversalAdditionalCameraData();
        cameraData.renderPostProcessing = true;
        cameraData.volumeLayerMask |= 1 << gameObject.layer;

        GameObject volumeObject = new GameObject("Player Feedback Volume");
        volumeObject.transform.SetParent(transform, false);
        volumeObject.layer = gameObject.layer;
        feedbackVolume = volumeObject.AddComponent<Volume>();
        feedbackVolume.isGlobal = true;
        feedbackVolume.priority = 100f;
        feedbackVolume.weight = 0f;
        feedbackVolume.sharedProfile = damageProfile;
        feedbackVolume.enabled = false;
    }

    private void ShowDamageFlash()
    {
        ShowFlash(damageProfile);
    }

    public void ShowHealFlash()
    {
        if (!isActiveAndEnabled) return;
        ShowFlash(healProfile);
    }

    private void ShowFlash(VolumeProfile profile)
    {
        if (feedbackVolume == null || profile == null) return;
        // Swap saved profiles without editing or cloning their shared settings.
        flashTimeRemaining = Mathf.Max(0.01f, fadeDuration);
        feedbackVolume.sharedProfile = profile;
        feedbackVolume.weight = 1f;
        feedbackVolume.enabled = true;
    }

    private void Update()
    {
        if (flashTimeRemaining <= 0f) return;
        // Use real elapsed time so pausing or slowing gameplay cannot leave a flash stuck on screen.
        flashTimeRemaining = Mathf.Max(0f, flashTimeRemaining - Time.unscaledDeltaTime);
        // Fade the volume's blend weight; the saved profile retains its authored intensity.
        feedbackVolume.weight = Mathf.SmoothStep(0f, 1f, flashTimeRemaining / Mathf.Max(0.01f, fadeDuration));
        if (flashTimeRemaining <= 0f) feedbackVolume.enabled = false;
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
    }

    private void OnDestroy()
    {
        // Only the volume is temporary; the shared profile assets belong to the project.
        if (feedbackVolume != null) Destroy(feedbackVolume.gameObject);
    }
}
