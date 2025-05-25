Shader "UI/RectSpotlightOverlay"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color ("Overlay Color", Color) = (0,0,0,0.8)
        _RectCenter ("Rectangle Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _RectSize ("Rectangle Size (UV)", Vector) = (0.3, 0.2, 0, 0)
        _Softness ("Edge Softness", Float) = 0.02
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        Cull Off
        ZTest Always
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _RectCenter; // xy: centro en UV (0-1)
            float4 _RectSize;   // xy: tamaño en UV (0-1)
            float  _Softness;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV del pixel
                float2 uv = i.uv;
                // Centro y mitad del tamaño en UV
                float2 center = _RectCenter.xy;
                float2 halfSize = _RectSize.xy * 0.5;
                // Calcula cuanto se sale el pixel fuera del rectangulo
                float2 delta = abs(uv - center) - halfSize;
                // Divide por la suavidad para lograr un borde difuminado
                float2 smoothDelta = delta / _Softness;
                float outside = max(smoothDelta.x, smoothDelta.y);
                float mask = smoothstep(0.0, 1.0, outside);
                fixed4 col = _Color;
                // La opacidad se anula dentro del rectangulo
                col.a *= mask;
                return col;
            }
            ENDCG
        }
    }
}
