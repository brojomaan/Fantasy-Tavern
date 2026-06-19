Shader "Custom/LiquidShader"
{
    Properties
    {
        _Color ("Liquid Color", Color) = (0.8, 0.5, 0.1, 1)
        _FillLevel ("Fill Level", Range(0, 1)) = 0.5
        _WobbleX ("Wobble X", Float) = 0
        _WobbleZ ("Wobble Z", Float) = 0
        _WobbleAmount ("Wobble Amount", Range(0, 0.1)) = 0.02
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamThickness ("Foam Thickness", Range(0, 0.1)) = 0.02
        _MinHeight ("Min Height", Float) = -0.1
        _MaxHeight ("Max Height", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Name "LiquidPass"
            Tags { "LightMode"="UniversalForward" }
            
            Cull Off
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _FillLevel;
                float _WobbleX;
                float _WobbleZ;
                float _WobbleAmount;
                float4 _FoamColor;
                float _FoamThickness;
                float _MinHeight;
                float _MaxHeight;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN, bool isFrontFace : SV_IsFrontFace) : SV_Target
            {
                // Wobble offset on the clip plane
                float wobble = sin(_WobbleX + IN.positionWS.x * 10) * _WobbleAmount
                             + sin(_WobbleZ + IN.positionWS.z * 10) * _WobbleAmount;

                // World Y fill threshold
                float objectHeight = UNITY_MATRIX_M._m13;
                float fillHeight = objectHeight + lerp(_MinHeight, _MaxHeight, _FillLevel);

                // Clip above fill height
                clip(fillHeight - IN.positionWS.y);

                // Foam at the surface
                float foam = step(fillHeight - _FoamThickness, IN.positionWS.y);
                half4 color = lerp(_Color, _FoamColor, foam);

                // Slightly darken back faces for interior feel
                if (!isFrontFace) color.rgb *= 0.7;

                return color;
            }
            ENDHLSL
        }
    }
}