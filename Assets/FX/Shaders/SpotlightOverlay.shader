Shader "UI/SpotlightOverlay" {
    Properties {
        _Color ("Overlay Color", Color) = (0, 0, 0, 0.8)
        _SpotlightCenter ("Spotlight Center", Vector) = (0.5, 0.5, 0, 0)
        _SpotlightRadius ("Spotlight Radius", Float) = 0.25
        _Softness ("Softness", Float) = 0.1
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _SpotlightCenter;
            float _SpotlightRadius;
            float _Softness;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calcula la distancia desde el centro del spotlight (en coordenadas UV, de 0 a 1)
                float dist = distance(i.uv, _SpotlightCenter.xy);
                // Usa smoothstep para lograr una transición suave en el borde
                float alpha = smoothstep(_SpotlightRadius, _SpotlightRadius - _Softness, dist);
                fixed4 col = _Color;
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
