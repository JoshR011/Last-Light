Shader "Last Light/WarFX Muzzle Flash URP"
{
    Properties
    {
        [MainTexture] _BaseMap("Flash Mask (Alpha)", 2D) = "white" {}
        [MainColor] _BaseColor("Tint", Color) = (0.5, 0.5, 0.5, 0.5)
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            Blend One One
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                half fogFactor : TEXCOORD1;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = input.color;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.fogFactor = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                // WarFX stores the flash silhouette in alpha, with color supplied by particles.
                half mask = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a;
                half4 color = 4.0h * input.color * _BaseColor * mask * input.color.a;
                color.rgb = MixFogColor(color.rgb, half3(0, 0, 0), input.fogFactor);
                return color;
            }
            ENDHLSL
        }
    }
}
