Shader "Last Light/UI/Compass Dial"
{
    Properties
    {
        [PerRendererData] _MainTex ("Compass Artwork", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (0.10, 0.11, 0.13, 1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "CanUseSpriteAtlas" = "False"
        }

        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex Vert
            #pragma fragment Frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _RimColor;

            struct Attributes
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                output.color = input.color;
                return output;
            }

            fixed4 Frag(Varyings input) : SV_Target
            {
                const float faceRadius = 0.96;
                float2 centered = input.uv * 2.0 - 1.0;
                float radius = length(centered);
                float feather = max(fwidth(radius), 0.0001);
                float disc = 1.0 - smoothstep(1.0 - feather, 1.0, radius);
                float face = 1.0 - smoothstep(faceRadius - feather, faceRadius, radius);

                // Fit the rectangular artwork inside the round face without stretching the markings.
                float2 dimensions = _MainTex_TexelSize.zw;
                float2 aspect = max(dimensions.x, dimensions.y) / dimensions;
                float2 artworkUV = 0.5 + centered * aspect * (0.5 / faceRadius);
                fixed4 artwork = tex2D(_MainTex, artworkUV);
                fixed4 result = lerp(_RimColor, artwork, face) * input.color;
                result.a *= disc;
                return result;
            }
            ENDCG
        }
    }
}
