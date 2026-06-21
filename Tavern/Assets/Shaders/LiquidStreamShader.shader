Shader "Custom/LiquidStreamShader"
{
    Properties
    {
        _Color ("Stream Color", Color) = (0.8, 0.5, 0.1, 1)
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _Active ("Active", Range(0, 1)) = 0
        _TransitionSpeed ("Transition Speed", Float) = 1.0
        _StreamWidth ("Stream Width", Range(0, 1)) = 0.3
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "StreamPass"
            Tags { "LightMode"="UniversalForward" }

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _ScrollSpeed;
                float _Active;
                float _TransitionSpeed;
                float _StreamWidth;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Scrolling UV downward
                float2 scrolledUV = IN.uv;
                scrolledUV.y -= _Time.y * _ScrollSpeed;

                // Panning reveal mask — moves top to bottom
                float revealMask = frac(scrolledUV.y);

                // Transition band — white sweeps down to reveal, black to hide
                float transition = step(revealMask, _Active);

                // Stream shape — narrow in the middle using UV x
                float streamShape = 1 - abs(IN.uv.x - 0.5) * 2;
                streamShape = step(1 - _StreamWidth, streamShape);

                float alpha = transition * streamShape;

                return half4(_Color.rgb, alpha);
            }
            ENDHLSL
        }
    }
}