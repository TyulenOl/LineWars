// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OutlineV2"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutLineSpread ("Outline Spread", Range(0,0.1)) = 0.007
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog
        {
            Mode Off
        }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile DUMMY PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            float _OutLineSpread;

            fixed4 frag(v2f IN) : COLOR
            {
                fixed4 backColor = tex2D(_MainTex, IN.texcoord + float2(_OutLineSpread, _OutLineSpread))
                    + tex2D(_MainTex, IN.texcoord - float2(_OutLineSpread, _OutLineSpread));
                backColor += (tex2D(_MainTex, IN.texcoord + float2(-_OutLineSpread, _OutLineSpread))
                    + tex2D(_MainTex, IN.texcoord - float2(-_OutLineSpread, _OutLineSpread)));
                
                backColor *= fixed4(0, 0, 0, 1);
                //backColor *= IN.color;
                
                fixed4 mainColor = tex2D(_MainTex, IN.texcoord);

                if (mainColor.a > 0.1f)
                    return mainColor;
                else
                    return backColor;
            }
            ENDCG
        }
    }
    Fallback Off
}