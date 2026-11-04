/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Applies beam-aware distance fog that hides distant silhouettes against the sky at any view angle.
 */

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FlashlightFogRendererFeature : ScriptableRendererFeature
{
    [Header("Fog Material")]
    public Material fogMaterial;

    private FogPass fogPass;
    private NativeFogPass nativeFogPass;
    private SkyBackgroundPass skyBackgroundPass;

    public override void Create()
    {
        // Unity can rebuild renderer features after Inspector changes, so release any previous textures first.
        fogPass?.Dispose();
        skyBackgroundPass?.Dispose();
        // Keep an unobstructed sky image so fully fogged objects can blend into the same background.
        skyBackgroundPass = new SkyBackgroundPass();
        // Replace native opaque fog with the beam-aware pass before transparent objects are drawn.
        nativeFogPass = new NativeFogPass { renderPassEvent = RenderPassEvent.BeforeRenderingOpaques };
        fogPass = new FogPass { renderPassEvent = RenderPassEvent.BeforeRenderingTransparents };
        // The fog shader needs scene color and depth to reconstruct the distance of each visible surface.
        fogPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
    }

    private FlashlightController GetFlashlight(Camera camera)
    {
        // Editor previews and cameras without a flashlight should keep Unity's normal fog rendering.
        if (fogMaterial == null || camera.cameraType != CameraType.Game) return null;
        return camera.GetComponentInChildren<FlashlightController>();
    }

    public override void OnCameraPreCull(ScriptableRenderer renderer, in CameraData cameraData)
    {
        // Save this camera's fog settings before either render pass changes how they are applied.
        FlashlightController flashlight = GetFlashlight(cameraData.camera);
        if (flashlight != null) flashlight.BeginFogOverride();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        FlashlightController flashlight = GetFlashlight(renderingData.cameraData.camera);
        if (flashlight == null || !flashlight.FogOverrideActive) return;
        fogPass.flashlight = flashlight;
        fogPass.material = fogMaterial;
        bool usesSkybox = renderingData.cameraData.camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null;
        fogPass.skyBackground = usesSkybox ? skyBackgroundPass : null;
        if (usesSkybox) renderer.EnqueuePass(skyBackgroundPass);
        // Run both passes for the same camera so native fog is never removed without its replacement.
        renderer.EnqueuePass(nativeFogPass);
        renderer.EnqueuePass(fogPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // URP makes camera targets available here, after it has allocated them for the current frame.
        fogPass.source = renderer.cameraColorTargetHandle;
        fogPass.depthSource = renderer.cameraDepthTargetHandle;
    }

    protected override void Dispose(bool disposing)
    {
        // Render textures survive between frames and must be released when the renderer is discarded.
        fogPass?.Dispose();
        skyBackgroundPass?.Dispose();
    }

    private class SkyBackgroundPass : DrawSkyboxPass
    {
        public RTHandle color;

        public SkyBackgroundPass() : base(RenderPassEvent.BeforeRenderingOpaques) { }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Render only the sky into its own color and depth buffer before scenery can cover it.
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.depthBufferBits = 24;
            RenderingUtils.ReAllocateIfNeeded(ref color, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_FlashlightSkyBackground");
            ConfigureTarget(color, color);
            ConfigureClear(ClearFlag.All, renderingData.cameraData.camera.backgroundColor);
        }

        public void Dispose() { color?.Release(); }
    }

    private class NativeFogPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Suppress built-in fog on opaque geometry so the later custom pass does not fog it twice.
            CommandBuffer cmd = CommandBufferPool.Get("Flashlight native fog override");
            CoreUtils.SetKeyword(cmd, "FOG_LINEAR", false);
            CoreUtils.SetKeyword(cmd, "FOG_EXP", false);
            CoreUtils.SetKeyword(cmd, "FOG_EXP2", false);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private class FogPass : ScriptableRenderPass
    {
        public FlashlightController flashlight;
        public Material material;
        public RTHandle source;
        public RTHandle depthSource;
        public SkyBackgroundPass skyBackground;
        private RTHandle temporary;
        private RTHandle silhouetteDepth;
        private int depthSamples;

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Match the camera's size and color format, but use a resolved color texture for the full-screen shader.
            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;
            descriptor.msaaSamples = 1;
            RenderingUtils.ReAllocateIfNeeded(ref temporary, descriptor, FilterMode.Bilinear, TextureWrapMode.Clamp, name: "_FlashlightFogColor");
            // Individual depth samples are readable only when the camera depth texture exposes its MSAA data.
            depthSamples = depthSource != null && depthSource.rt != null && depthSource.rt.bindTextureMS
                ? depthSource.rt.antiAliasing : 1;
            if (depthSamples > 1)
            {
                // Keep depth in a separate floating-point texture so resolving silhouettes cannot alter scene color.
                descriptor.graphicsFormat = GraphicsFormat.R32_SFloat;
                RenderingUtils.ReAllocateIfNeeded(ref silhouetteDepth, descriptor, FilterMode.Point, TextureWrapMode.Clamp, name: "_FlashlightSilhouetteDepth");
            }
            ConfigureTarget(source);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (flashlight == null || source == null || temporary == null) return;
            // Match the fog color to the camera's working color space before sending it to the shader.
            Color fogColor = flashlight.SceneFogColor;
            if (QualitySettings.activeColorSpace == ColorSpace.Linear) fogColor = fogColor.linear;
            material.SetColor("_SceneFogColor", fogColor);
            material.SetVector("_SceneFogSettings", new Vector4((int)flashlight.SceneFogMode, flashlight.SceneFogDensity, flashlight.SceneFogStart, flashlight.SceneFogEnd));
            material.SetFloat("_SceneFogEnabled", flashlight.SceneFogEnabled ? 1f : 0f);
            // Blend a visible skybox into the fog at the horizon without changing solid camera backgrounds.
            Camera camera = renderingData.cameraData.camera;
            bool blendSkybox = skyBackground != null && skyBackground.color != null;
            material.SetFloat("_BlendSkyboxWithFog", blendSkybox ? 1f : 0f);
            // Use the camera's actual rays for both distance fog and the light cone at every viewing angle.
            float halfHeight = Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            material.SetVector("_FogViewForward", camera.transform.forward);
            material.SetVector("_FogViewUp", camera.transform.up * halfHeight);
            material.SetVector("_FogViewRight", camera.transform.right * halfHeight * camera.aspect);

            // Use the actual spotlight origin and direction so the clearer cone follows the visible beam.
            Light beam = flashlight.beamLight;
            bool lit = flashlight.IsOn && beam != null && beam.isActiveAndEnabled;
            // A little light scattered by the mist makes the beam visible without changing the gray fog color.
            Color glowColor = beam != null ? beam.color : Color.white;
            if (QualitySettings.activeColorSpace == ColorSpace.Linear) glowColor = glowColor.linear;
            material.SetColor("_FlashlightFogLight", glowColor * flashlight.fogGlowStrength);
            Vector3 position = beam != null ? beam.transform.position : flashlight.transform.position;
            Vector3 direction = beam != null ? beam.transform.forward : flashlight.ViewCamera.transform.forward;
            material.SetVector("_FlashlightPosition", new Vector4(position.x, position.y, position.z, lit ? 1f : 0f));
            material.SetVector("_FlashlightDirection", direction);
            // Match the spotlight's full spread and reach so the visible cone joins its lit footprint.
            material.SetVector("_VisibleBeamShape", new Vector4(
                beam != null ? beam.range : 0f,
                beam != null ? Mathf.Tan(beam.spotAngle * 0.5f * Mathf.Deg2Rad) : 0f,
                flashlight.beamStartRadius, 0f));
            // Cosine thresholds give the shader the same inner and outer cone angles as the spotlight.
            material.SetVector("_FlashlightCone", new Vector4(
                beam != null ? Mathf.Cos(beam.spotAngle * 0.5f * Mathf.Deg2Rad) : 1f,
                beam != null ? Mathf.Cos(beam.innerSpotAngle * 0.5f * Mathf.Deg2Rad) : 1f,
                beam != null ? beam.range : 0f, flashlight.beamFogDensityMultiplier));

            CommandBuffer cmd = CommandBufferPool.Get("Flashlight cone fog");
            if (blendSkybox)
            {
                // Use the sky behind each surface, rather than a flat gray that would reveal its outline.
                cmd.SetGlobalTexture("_FlashlightSkyBackground", skyBackground.color.nameID);
            }
            bool resolveSilhouettes = depthSamples > 1 && silhouetteDepth != null;
            material.SetFloat("_UseSilhouetteDepth", resolveSilhouettes ? 1f : 0f);
            if (resolveSilhouettes)
            {
                // URP resolves to the farthest depth sample, which becomes sky along MSAA silhouettes.
                // Use the nearest covered sample for fog so partially covered trees cannot leak color.
                CoreUtils.SetKeyword(material, "_FLASHLIGHT_DEPTH_MSAA_2", depthSamples == 2);
                CoreUtils.SetKeyword(material, "_FLASHLIGHT_DEPTH_MSAA_4", depthSamples == 4);
                CoreUtils.SetKeyword(material, "_FLASHLIGHT_DEPTH_MSAA_8", depthSamples == 8);
                material.SetVector("_FlashlightDepthSize", new Vector4(depthSource.rt.width, depthSource.rt.height, 0f, 0f));
                cmd.SetGlobalTexture("_FlashlightNativeDepth", depthSource.nameID);
                Blitter.BlitCameraTexture(cmd, source, silhouetteDepth, material, 1);
                cmd.SetGlobalTexture("_FlashlightSilhouetteDepth", silhouetteDepth.nameID);
            }
            // Apply fog into a temporary texture before copying back; sampling and writing one target is unsafe.
            Blitter.BlitCameraTexture(cmd, source, temporary, material, 0);
            Blitter.BlitCameraTexture(cmd, temporary, source);
            // Transparent particles and glass retain their existing fog; they have no opaque depth here.
            CoreUtils.SetKeyword(cmd, "FOG_LINEAR", flashlight.SceneFogEnabled && flashlight.SceneFogMode == FogMode.Linear);
            CoreUtils.SetKeyword(cmd, "FOG_EXP", flashlight.SceneFogEnabled && flashlight.SceneFogMode == FogMode.Exponential);
            CoreUtils.SetKeyword(cmd, "FOG_EXP2", flashlight.SceneFogEnabled && flashlight.SceneFogMode == FogMode.ExponentialSquared);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (flashlight == null) return;
            // Restore the correct fog variant for subsequent cameras and allow a fresh capture next frame.
            bool enabled = flashlight.SceneFogEnabled;
            CoreUtils.SetKeyword(cmd, "FOG_LINEAR", enabled && flashlight.SceneFogMode == FogMode.Linear);
            CoreUtils.SetKeyword(cmd, "FOG_EXP", enabled && flashlight.SceneFogMode == FogMode.Exponential);
            CoreUtils.SetKeyword(cmd, "FOG_EXP2", enabled && flashlight.SceneFogMode == FogMode.ExponentialSquared);
            flashlight.EndFogOverride();
        }

        public void Dispose()
        {
            temporary?.Release();
            silhouetteDepth?.Release();
        }
    }
}
