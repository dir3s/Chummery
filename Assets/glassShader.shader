Shader "Custom/Dormitory_Final_Fixed" {
    Properties {
        [Header(Glass Settings)]
        _Radius("Blur Radius", Range(0, 20)) = 5
        _Color("Glass Tint", Color) = (1, 1, 1, 0.2)
        
        [Header(Outline Settings)]
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineThickness("Outline Thickness", Range(0, 0.5)) = 0.05
        _EdgeSoftness("Edge Softness", Range(0.001, 0.1)) = 0.01
        
        [HideInInspector] _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off

        GrabPass { "_BackgroundTexture" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float4 color : COLOR;
            };

            sampler2D _BackgroundTexture;
            sampler2D _MainTex;
            float _Radius;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _EdgeSoftness;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                o.uv = v.texcoord;
                o.color = v.color; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float spriteAlpha = tex2D(_MainTex, i.uv).a;
                clip(spriteAlpha - 0.01);

                // РАМКА (через UV для товщини)
                float2 dists = min(i.uv, 1.0 - i.uv);
                float minDist = min(dists.x, dists.y);
                float outlineFactor = 1.0 - smoothstep(_OutlineThickness, _OutlineThickness + _EdgeSoftness, minDist);

                // РОЗМИТТЯ
                // Використовуємо i.color.a як множник для радіусу (якщо міняєш альфу в Button - міняється розмиття)
                float r = _Radius * 0.001;
                float4 sum = float4(0,0,0,0);
                sum += tex2Dproj(_BackgroundTexture, float4(i.grabPos.x - r, i.grabPos.y - r, i.grabPos.z, i.grabPos.w));
                sum += tex2Dproj(_BackgroundTexture, float4(i.grabPos.x + r, i.grabPos.y - r, i.grabPos.z, i.grabPos.w));
                sum += tex2Dproj(_BackgroundTexture, float4(i.grabPos.x - r, i.grabPos.y + r, i.grabPos.z, i.grabPos.w));
                sum += tex2Dproj(_BackgroundTexture, float4(i.grabPos.x + r, i.grabPos.y + r, i.grabPos.z, i.grabPos.w));
                float4 blurredColor = sum / 4;

                // СКЛО
                float4 glassColor = lerp(blurredColor, _Color, _Color.a);
                
                // ЗМІШУВАННЯ (Скло + Рамка)
                float4 finalColor = lerp(glassColor, _OutlineColor, outlineFactor * _OutlineColor.a);
                
                // МНОЖИМО НА COLOR ВІД КНОПКИ (щоб темнішало)
                // Ми множимо тільки RGB, щоб не втратити прозорість кутів
                finalColor.rgb *= i.color.rgb;
                finalColor.a *= spriteAlpha * i.color.a;

                return finalColor;
            }
            ENDCG
        }
    }
}