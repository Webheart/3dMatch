Shader "Custom/Outline"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestMask("ZTest Mask", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTestFill("ZTest Fill", Float) = 0
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent+100"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Name "Mask"
            Cull Off
            ZTest [_ZTestMask]
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 1
                Pass Replace
            }
        }

        Pass
        {
            Name "Fill"
            Cull Off
            ZTest [_ZTestFill]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB

            Stencil
            {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
            };

            uniform fixed4 _OutlineColor;
            uniform float _OutlineWidth;

            v2f vert(appdata input)
            {
                v2f output;

                UNITY_SETUP_INSTANCE_ID(input);

                const float3 viewPosition = UnityObjectToViewPos(input.vertex * (1 + _OutlineWidth));

                output.position = UnityViewToClipPos(viewPosition);
                output.color = _OutlineColor;

                return output;
            }

            fixed4 frag(v2f input) : SV_Target
            {
                return input.color;
            }
            ENDCG
        }
    }
}