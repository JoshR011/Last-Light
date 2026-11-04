/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Toggles the held flashlight through PlayerInput and shares its beam and fog settings.
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight References")]
    public Light beamLight;
    public Renderer bulbRenderer;
    public int bulbMaterialIndex = 3;

    [Header("Starting State")]
    public bool startsOn = true;

    [Header("Beam Aim")]
    [Min(1f)]
    public float beamAimDistance = 12f;

    [Header("Fog Visibility")]
    [Range(0.1f, 1f)]
    public float beamFogDensityMultiplier = 0.35f;

    [Header("Fog Lighting")]
    [Range(0f, 0.2f)]
    public float fogGlowStrength = 0.04f;

    [Header("Visible Beam Shape")]
    // Width, reach, and edge falloff come from the spotlight so the mist joins its surface footprint.
    [Tooltip("Radius of the visible beam where it leaves the flashlight lens.")]
    [Min(0.001f)]
    public float beamStartRadius = 0.015f;

    public bool IsOn { get; private set; }
    public Camera ViewCamera { get; private set; }
    // Save the scene's normal fog so the renderer can keep it dense outside this flashlight's beam.
    internal bool FogOverrideActive { get; private set; }
    internal bool SceneFogEnabled { get; private set; }
    internal FogMode SceneFogMode { get; private set; }
    internal Color SceneFogColor { get; private set; }
    internal float SceneFogDensity { get; private set; }
    internal float SceneFogStart { get; private set; }
    internal float SceneFogEnd { get; private set; }

    private MaterialPropertyBlock bulbProperties;

    private void Awake()
    {
        // The camera parent keeps the held model in place and identifies which view receives the fog effect.
        ViewCamera = GetComponentInParent<Camera>();
        // Change this flashlight's bulb glow without editing the material shared by prefab instances.
        bulbProperties = new MaterialPropertyBlock();
        UpdateBeamAim();
    }

    private void OnEnable()
    {
        // Apply the Inspector's starting state whenever this flashlight becomes active.
        SetLightOn(startsOn);
    }

    private void LateUpdate()
    {
        // Keep the light aimed at the view's center as the camera and held model move together.
        UpdateBeamAim();
    }

    private void UpdateBeamAim()
    {
        if (ViewCamera == null || beamLight == null) return;
        Vector3 target = ViewCamera.transform.position + ViewCamera.transform.forward * beamAimDistance;
        Vector3 direction = target - beamLight.transform.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            // Rotate the beam, rather than the held model, so its origin stays at the flashlight head.
            beamLight.transform.rotation = Quaternion.LookRotation(direction, ViewCamera.transform.up);
        }
    }

    // PlayerInput broadcasts this action to camera children through the new Input System.
    public void OnFlashlight(InputValue value)
    {
        // Button releases also send input callbacks, so only the press should toggle the light.
        if (value.isPressed)
        {
            SetLightOn(!IsOn);
        }
    }

    public void SetLightOn(bool enabled)
    {
        // Toggle the light component while leaving the held model visible in the player's view.
        IsOn = enabled;
        if (beamLight != null)
        {
            beamLight.enabled = enabled;
        }
        if (bulbRenderer != null)
        {
            // The bulb uses one material slot; the body and glass should keep their existing appearance.
            if (bulbProperties == null) bulbProperties = new MaterialPropertyBlock();
            bulbProperties.SetColor("_EmissionColor", enabled ? new Color(0.8f, 0.75f, 0.6f) : Color.black);
            bulbRenderer.SetPropertyBlock(bulbProperties, bulbMaterialIndex);
        }
    }

    internal void BeginFogOverride()
    {
        // Capture once per camera render so repeated setup calls cannot replace the original settings.
        if (FogOverrideActive) return;
        SceneFogEnabled = RenderSettings.fog;
        SceneFogMode = RenderSettings.fogMode;
        SceneFogColor = RenderSettings.fogColor;
        SceneFogDensity = RenderSettings.fogDensity;
        SceneFogStart = RenderSettings.fogStartDistance;
        SceneFogEnd = RenderSettings.fogEndDistance;
        FogOverrideActive = true;
        // The renderer feature temporarily suppresses native fog keywords for this camera.
    }

    internal void EndFogOverride()
    {
        // Clear the capture flag so the next frame can pick up any changes to the scene's fog settings.
        if (!FogOverrideActive) return;
        FogOverrideActive = false;
    }

    private void OnDisable()
    {
        // An inactive or removed flashlight should not retain a pending render state or emit light.
        EndFogOverride();
        SetLightOn(false);
    }
}
