/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Keeps fog gray, clears and lights the flashlight cone, and resolves MSAA silhouette depth.
 */

Shader "LastLight/Flashlight Fog"
{
    Properties
    {
        [Header(Skybox Haze)]
        _SkyDetailStrength ("Sky Detail Strength", Range(0, 0.1)) = 0.015
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            ZWrite Off ZTest Always Cull Off
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            float4 _SceneFogColor;
            float4 _SceneFogSettings;
            float _SceneFogEnabled;
            float4 _FlashlightPosition;
            float3 _FlashlightDirection;
            float4 _FlashlightCone;
            TEXTURE2D_X_FLOAT(_FlashlightSilhouetteDepth);
            TEXTURE2D_X(_FlashlightSkyBackground);
            float _UseSilhouetteDepth;
            float _BlendSkyboxWithFog;
            float _SkyDetailStrength;
            float3 _FogViewForward;
            float3 _FogViewUp;
            float3 _FogViewRight;
            half3 _FlashlightFogLight;
            float4 _VisibleBeamShape;

            float BeamCoverage(float3 fromLight)
            {
                // Share the spotlight's inner and outer angles between the mist and surface fog clearing.
                float alignment = dot(fromLight / max(length(fromLight), 0.0001), _FlashlightDirection);
                return smoothstep(_FlashlightCone.x, max(_FlashlightCone.x + 0.0001, _FlashlightCone.y), alignment);
            }

            float BeamRangeFade(float lightDistance)
            {
                return 1.0 - smoothstep(_FlashlightCone.z * 0.8, max(0.001, _FlashlightCone.z), lightDistance);
            }

            half3 IntegrateBeamMist(float3 rayDirection, float surfaceDistance)
            {
                if (_FlashlightPosition.w < 0.5 || _VisibleBeamShape.x <= 0.0) return 0;
                float axialRate = dot(rayDirection, _FlashlightDirection);
                if (axialRate <= 0.0001) return 0;

                // Clip the camera ray to the cone's start and end planes and stop at opaque scenery.
                float axialOrigin = dot(_WorldSpaceCameraPos - _FlashlightPosition.xyz, _FlashlightDirection);
                float entry = max(0.0, -axialOrigin / axialRate);
                float exit = min(surfaceDistance, (_VisibleBeamShape.x - axialOrigin) / axialRate);
                if (exit <= entry) return 0;

                // Find the exact segment inside the cone so samples cannot miss thin slices and leave rings.
                float3 radialOrigin = _WorldSpaceCameraPos - _FlashlightPosition.xyz - _FlashlightDirection * axialOrigin;
                float3 radialRay = rayDirection - _FlashlightDirection * axialRate;
                float radiusOrigin = _VisibleBeamShape.z + axialOrigin * _VisibleBeamShape.y;
                float radiusRate = axialRate * _VisibleBeamShape.y;
                float quadratic = dot(radialRay, radialRay) - radiusRate * radiusRate;
                float linearTerm = dot(radialOrigin, radialRay) - radiusOrigin * radiusRate;
                float constant = dot(radialOrigin, radialOrigin) - radiusOrigin * radiusOrigin;
                if (abs(quadratic) < 0.0000001)
                {
                    // A ray parallel to the cone wall has a linear intersection instead of two roots.
                    if (abs(linearTerm) > 0.0000001)
                    {
                        float boundary = -constant / (2.0 * linearTerm);
                        if (linearTerm > 0.0) exit = min(exit, boundary);
                        else entry = max(entry, boundary);
                    }
                    else if (constant > 0.0) return 0;
                }
                else
                {
                    float discriminant = linearTerm * linearTerm - quadratic * constant;
                    if (discriminant <= 0.0)
                    {
                        if (quadratic > 0.0) return 0;
                    }
                    else
                    {
                        float rootOffset = sqrt(discriminant);
                        float firstRoot = (-linearTerm - rootOffset) / quadratic;
                        float secondRoot = (-linearTerm + rootOffset) / quadratic;
                        float nearRoot = min(firstRoot, secondRoot);
                        float farRoot = max(firstRoot, secondRoot);
                        if (quadratic > 0.0)
                        {
                            entry = max(entry, nearRoot);
                            exit = min(exit, farRoot);
                        }
                        else
                        {
                            // Only the forward half of the cone survives the start and end plane clipping.
                            float startDistance = (quadratic * entry + 2.0 * linearTerm) * entry + constant;
                            float endDistance = (quadratic * exit + 2.0 * linearTerm) * exit + constant;
                            if (startDistance > 0.0) entry = max(entry, farRoot);
                            else if (endDistance > 0.0) exit = min(exit, nearRoot);
                        }
                    }
                }
                if (exit <= entry) return 0;

                float scatteredLight = 0.0;
                const int sampleCount = 24;
                UNITY_LOOP
                for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
                {
                    // Sample only the continuous interior, concentrating detail near the flashlight head.
                    float lower = (float)sampleIndex / sampleCount;
                    float upper = (float)(sampleIndex + 1) / sampleCount;
                    lower *= lower;
                    upper *= upper;
                    float rayDistance = lerp(entry, exit, (lower + upper) * 0.5);
                    float stepLength = (upper - lower) * (exit - entry);
                    float3 fromLight = _WorldSpaceCameraPos + rayDirection * rayDistance - _FlashlightPosition.xyz;
                    float alongBeam = dot(fromLight, _FlashlightDirection);
                    // Offset the cone apex slightly behind the lens to retain its small starting radius.
                    float apexOffset = _VisibleBeamShape.z / max(_VisibleBeamShape.y, 0.0001);
                    float coverage = BeamCoverage(fromLight + _FlashlightDirection * apexOffset);
                    // Use the same reach and edge falloff as the light on scenery, without an early cutoff.
                    float endFade = BeamRangeFade(length(fromLight));
                    float attenuation = rcp(0.02 + alongBeam * alongBeam);
                    scatteredLight += coverage * endFade * attenuation * stepLength;
                }

                float mistAmount = 1.0 - exp(-_SceneFogSettings.y * scatteredLight * 4.0);
                return _FlashlightFogLight * mistAmount;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                if (_SceneFogEnabled < 0.5) return color;
                // Covered MSAA samples keep a tree's edge from being mistaken for empty sky.
                float depth = _UseSilhouetteDepth > 0.5
                    ? LOAD_TEXTURE2D_X(_FlashlightSilhouetteDepth, uint2(input.positionCS.xy)).r
                    : SampleSceneDepth(uv);
                // Sky has no surface distance; use the flashlight range only for light scattering in the mist.
                #if UNITY_REVERSED_Z
                    bool isSky = depth < 0.000001;
                #else
                    bool isSky = depth > 0.999999;
                #endif
                // The sky can add faint texture to the gray haze, but must never darken the fog when looking up.
                half3 atmosphereColor = _SceneFogColor.rgb;
                if (_BlendSkyboxWithFog > 0.5)
                {
                    half3 skyColor = SAMPLE_TEXTURE2D_X(_FlashlightSkyBackground, sampler_LinearClamp, uv).rgb;
                    float skyDetail = saturate(dot(skyColor, float3(0.2126, 0.7152, 0.0722))) * _SkyDetailStrength;
                    atmosphereColor += skyDetail;
                }

                // Reconstruct a camera ray directly so projection flips cannot move fog clearing away from the beam.
                // Blit UVs already account for API texture orientation; flipping Y again detaches the beam from its lens.
                float2 viewCoordinates = uv * 2.0 - 1.0;
                float3 viewRay = _FogViewForward + viewCoordinates.x * _FogViewRight + viewCoordinates.y * _FogViewUp;
                float rayLength = length(viewRay);
                float eyeDepth = isSky ? 0.0 : LinearEyeDepth(depth, _ZBufferParams);
                float distanceFromCamera = isSky ? _FlashlightCone.z : max(0.0, eyeDepth * rayLength - _ProjectionParams.y);

                // Integrate light along the actual volume from the lens, rather than painting a disk at one depth.
                half3 mistLight = IntegrateBeamMist(viewRay / max(rayLength, 0.0001), distanceFromCamera);
                if (isSky)
                {
                    if (_BlendSkyboxWithFog > 0.5) color.rgb = atmosphereColor;
                    color.rgb += mistLight;
                    return color;
                }

                // True surface distance and the light cone now use the same camera ray at every viewing angle.
                float3 worldPosition = _WorldSpaceCameraPos + viewRay * eyeDepth;
                float3 fromLight = worldPosition - _FlashlightPosition.xyz;
                float lightDistance = length(fromLight);
                // Soften the cone's boundary and fade its clearing effect before the spotlight's maximum range.
                float cone = BeamCoverage(fromLight);
                float rangeFade = BeamRangeFade(lightDistance);
                // A switched-off flashlight leaves the normal fog density everywhere in the view.
                float densityScale = lerp(1.0, _FlashlightCone.w, cone * rangeFade * _FlashlightPosition.w);
                float fogDistance = distanceFromCamera * densityScale;
                float visibility;
                // Preserve the scene's selected fog mode while extending visibility inside the lit cone.
                if (_SceneFogSettings.x < 1.5)
                    visibility = saturate((_SceneFogSettings.w - fogDistance) / max(0.001, _SceneFogSettings.w - _SceneFogSettings.z));
                else if (_SceneFogSettings.x < 2.5)
                    visibility = exp(-_SceneFogSettings.y * fogDistance);
                else
                {
                    float density = _SceneFogSettings.y * fogDistance;
                    visibility = exp(-density * density);
                }
                // Blend distant surfaces into fog without changing the scene color's alpha channel.
                color.rgb = lerp(atmosphereColor, color.rgb, visibility) + mistLight;
                return color;
            }
            ENDHLSL
        }
        Pass
        {
            Name "Resolve Silhouette Depth"
            ZWrite Off ZTest Always Cull Off
            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment ResolveSilhouetteDepth
            #pragma multi_compile_local _FLASHLIGHT_DEPTH_MSAA_2 _FLASHLIGHT_DEPTH_MSAA_4 _FLASHLIGHT_DEPTH_MSAA_8
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            // Match the camera's sample count so every covered part of an edge contributes depth.
            #if defined(_FLASHLIGHT_DEPTH_MSAA_4)
                #define FLASHLIGHT_DEPTH_SAMPLES 4
            #elif defined(_FLASHLIGHT_DEPTH_MSAA_8)
                #define FLASHLIGHT_DEPTH_SAMPLES 8
            #else
                #define FLASHLIGHT_DEPTH_SAMPLES 2
            #endif

            // Stereo views store each eye in a texture-array slice; ordinary cameras use a single texture.
            #if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
                Texture2DMSArray<float, FLASHLIGHT_DEPTH_SAMPLES> _FlashlightNativeDepth;
                #define LOAD_FLASHLIGHT_DEPTH(pixel, sampleIndex) LOAD_TEXTURE2D_ARRAY_MSAA(_FlashlightNativeDepth, pixel, unity_StereoEyeIndex, sampleIndex)
            #else
                Texture2DMS<float, FLASHLIGHT_DEPTH_SAMPLES> _FlashlightNativeDepth;
                #define LOAD_FLASHLIGHT_DEPTH(pixel, sampleIndex) LOAD_TEXTURE2D_MSAA(_FlashlightNativeDepth, pixel, sampleIndex)
            #endif
            float4 _FlashlightDepthSize;

            float ResolveSilhouetteDepth(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                int2 pixel = int2(input.texcoord * _FlashlightDepthSize.xy);
                // Initialize to sky depth; reversed-Z platforms put nearer surfaces at larger values.
                #if UNITY_REVERSED_Z
                    float nearestDepth = 0.0;
                #else
                    float nearestDepth = 1.0;
                #endif
                UNITY_UNROLL
                for (int sampleIndex = 0; sampleIndex < FLASHLIGHT_DEPTH_SAMPLES; sampleIndex++)
                {
                    // Keep the nearest covered sample instead of allowing an uncovered sky sample to win.
                    float depth = LOAD_FLASHLIGHT_DEPTH(pixel, sampleIndex);
                    #if UNITY_REVERSED_Z
                        nearestDepth = max(nearestDepth, depth);
                    #else
                        nearestDepth = min(nearestDepth, depth);
                    #endif
                }
                return nearestDepth;
            }
            ENDHLSL
        }
    }
}
