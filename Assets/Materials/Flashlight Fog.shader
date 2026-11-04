/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-03
 * Summary: Applies distance fog with a clearer flashlight cone and resolves MSAA silhouette depth.
 */

Shader "LastLight/Flashlight Fog"
{
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
            float _UseSilhouetteDepth;

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = input.texcoord;
                half4 color = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                // Covered MSAA samples keep a tree's edge from being mistaken for empty sky.
                float depth = _UseSilhouetteDepth > 0.5
                    ? LOAD_TEXTURE2D_X(_FlashlightSilhouetteDepth, uint2(input.positionCS.xy)).r
                    : SampleSceneDepth(uv);
                // Sky has no surface distance; preserve the camera background instead of fogging it again.
                #if UNITY_REVERSED_Z
                    if (depth < 0.000001) return color;
                #else
                    if (depth > 0.999999) return color;
                    depth = lerp(UNITY_NEAR_CLIP_VALUE, 1.0, depth);
                #endif
                if (_SceneFogEnabled < 0.5) return color;

                // Reconstruct the surface position so visibility follows depth rather than screen brightness.
                float3 worldPosition = ComputeWorldSpacePosition(uv, depth, UNITY_MATRIX_I_VP);
                float distanceFromCamera = max(0.0, -TransformWorldToView(worldPosition).z - _ProjectionParams.y);
                float3 fromLight = worldPosition - _FlashlightPosition.xyz;
                float lightDistance = length(fromLight);
                // Soften the cone's boundary and fade its clearing effect before the spotlight's maximum range.
                float cone = smoothstep(_FlashlightCone.x, max(_FlashlightCone.x + 0.0001, _FlashlightCone.y),
                    dot(fromLight / max(lightDistance, 0.0001), _FlashlightDirection));
                float rangeFade = 1.0 - smoothstep(_FlashlightCone.z * 0.8, max(0.001, _FlashlightCone.z), lightDistance);
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
                color.rgb = lerp(_SceneFogColor.rgb, color.rgb, visibility);
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
