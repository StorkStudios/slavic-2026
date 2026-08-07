Shader "Particles/Blood Effect URP"
{
    Properties
    {
        [Header(Color Controls)]
        [HDR] _BaseColor ("Base Color Mult", Color) = (1,1,1,1)
        _LightStr ("Lighting Strength", Float) = 0.85
        _AlphaMin ("Alpha Clip Min", Range(-0.01,1.01)) = 0.1
        _AlphaSoft ("Alpha Clip Softness", Range(0,1)) = 0.022
        _EdgeDarken ("Edge Darkening", Float) = 1.0
        _ProcMask ("Procedural Mask Strength", Float) = 1.0

        [Header(Mask Controls)]
        _MainTex ("Mask Texture", 2D) = "white" {}
        _MaskStr ("Mask Strength", Float) = 0.7
        _Columns ("Flipbook Columns", Int) = 1
        _Rows ("Flipbook Rows", Int) = 1
        _ChannelMask ("Channel Mask", Vector) = (1,0,0,0)
        [Toggle] _FlipU ("Flip U Randomly", Float) = 0
        [Toggle] _FlipV ("Flip V Randomly", Float) = 0

        [Header(Noise Controls)]
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseAlphaStr ("Noise Strength", Float) = 0.8
        _NoiseColorStr ("Noise Color Strength", Float) = 0
        _ChannelMask2 ("Noise Channel Mask", Vector) = (1,0,0,0)
        _Randomize ("Randomize Noise", Float) = 1.0

        [Header(UV Warp)]
        _WarpTex ("Warp Texture", 2D) = "gray" {}
        _WarpStr ("Warp Strength", Float) = 0

        [Header(Vertex Physics)]
        _FallOffset ("Gravity Offset", Range(-1,0)) = -1.0
        _FallRandomness ("Gravity Randomness", Float) = 0.25

        [Header(Optional Specular)]
        [HDR] _SpecularColor ("Specular Color", Color) = (0,0,0,0)
        _ReflectionTex ("Reflection Texture", 2D) = "black" {}
        _ReflectionSat ("Reflection Saturation", Range(0,1)) = 1
        _SpecularPower ("Specular Power", Range(0.1,8)) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Name "Forward"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);    SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);   SAMPLER(sampler_NoiseTex);
            TEXTURE2D(_WarpTex);    SAMPLER(sampler_WarpTex);
            TEXTURE2D(_ReflectionTex); SAMPLER(sampler_ReflectionTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half _LightStr;
                half _AlphaMin;
                half _AlphaSoft;
                half _EdgeDarken;
                half _ProcMask;

                half4 _MainTex_ST;
                half _MaskStr;
                half _Columns;
                half _Rows;
                half4 _ChannelMask;
                half _FlipU;
                half _FlipV;

                half4 _NoiseTex_ST;
                half _NoiseAlphaStr;
                half _NoiseColorStr;
                half4 _ChannelMask2;
                half _Randomize;

                half4 _WarpTex_ST;
                half _WarpStr;

                half _FallOffset;
                half _FallRandomness;

                half4 _ReflectionTex_ST;
                half _ReflectionSat;
                half4 _SpecularColor;
                half _SpecularPower;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 uv0        : TEXCOORD0; // Z=random, W=lifetime
                float3 uv1        : TEXCOORD1; // X=pan, Y=warp, Z=gravity
                half4 color       : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 uv         : TEXCOORD0;
                half4 color       : COLOR;
                float3 normalWS   : TEXCOORD1;
                float3 customData : TEXCOORD2;
                float3 positionWS : TEXCOORD3;
                float fogFactor   : TEXCOORD4;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float lifetime = v.uv0.w;
                lifetime = lifetime * lifetime +
                           (_FallOffset + ((v.uv0.z - 0.5) * _FallRandomness)) * lifetime;

                float3 fallPos = lifetime * float3(0, v.uv1.z, 0);

                float2 uvFlip = round(frac(float2(v.uv0.z * 13.0, v.uv0.z * 8.0)));
                uvFlip = uvFlip * 2.0 - 1.0;
                uvFlip = lerp(1.0, uvFlip, float2(_FlipU, _FlipV));

                float3 positionWS = TransformObjectToWorld(v.positionOS.xyz) + fallPos;

                o.positionWS = positionWS;
                o.positionCS = TransformWorldToHClip(positionWS);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);

                o.color = v.color;
                o.color.a *= o.color.a;
                o.color.a += _AlphaMin;

                o.customData = float3(v.uv1.xy, v.uv0.z);

                o.uv.xy = TRANSFORM_TEX(v.uv0.xy * uvFlip, _MainTex);
                o.uv.zw = o.uv.xy * half2(_Columns, _Rows)
                        + v.uv0.z * half2(3,8) * _Randomize;

                o.fogFactor = ComputeFogFactor(o.positionCS.z);
                return o;
            }

            half3 GetAmbient(half3 normalWS)
            {
                // URP SH lighting replaces the old ShadeSH9/unity_Ambient* path.
                half3 sh = SampleSH(normalWS);
                sh = max(sh, half3(0.15, 0.15, 0.15));
                return lerp(half3(1,1,1), sh, _LightStr);
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 warpUV = i.uv.zw * _WarpTex_ST.xy
                              + _WarpTex_ST.zw * (i.customData.x + 1.0)
                              + float2(5,8) * i.customData.z;

                half4 uvWarp = SAMPLE_TEXTURE2D(_WarpTex, sampler_WarpTex, warpUV);
                float2 warp = (uvWarp.xy * 2.0 - 1.0) * _WarpStr * i.customData.y;

                half4 mask = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy + warp);
                mask = saturate(lerp(half4(1,1,1,1), mask, _MaskStr));

                half2 tempUV = frac(i.uv.xy * half2(_Columns, _Rows)) - 0.5;
                tempUV *= tempUV * 4.0;
                half edgeMask = saturate(tempUV.x + tempUV.y);
                edgeMask *= edgeMask;
                edgeMask = 1.0 - edgeMask;
                edgeMask = lerp(1.0, edgeMask, _ProcMask);

                mask *= edgeMask;

                half4 col = max(0.001h, i.color);
                col.a = saturate(dot(mask, _ChannelMask));

                half4 noise4 = SAMPLE_TEXTURE2D(
                    _NoiseTex, sampler_NoiseTex,
                    i.uv.zw * _NoiseTex_ST.xy + _NoiseTex_ST.zw * i.customData.x + warp
                );

                half noise = saturate(lerp(1.0h, dot(noise4, _ChannelMask2), _NoiseAlphaStr));

                col.a *= noise;
                half preClipAlpha = col.a;
                half clippedAlpha = saturate(
                    (preClipAlpha * i.color.a - _AlphaMin) / max(_AlphaSoft, 0.0001h)
                );
                col.a = clippedAlpha;

                half3 baseLighting = GetAmbient(i.normalWS);

                // Optional main-light contribution. This keeps the shader useful in URP
                // without depending on the legacy ForwardBase lighting path.
                Light mainLight = GetMainLight();
                half NdotL = saturate(dot(i.normalWS, mainLight.direction));
                baseLighting *= lerp(1.0h, mainLight.color * NdotL + 0.05h, 0.5h);

                half edge = 1.0 - saturate(preClipAlpha * clippedAlpha);
                edge *= edge;
                edge = 1.0 - edge;
                edge += lerp(0.0h, noise - 0.5h, _NoiseColorStr);

                edge = saturate(lerp(0.71h, edge * edge, _EdgeDarken));
                col.a *= saturate(lerp(1.25h, _BaseColor.a, edge));

                col.rgb *= lerp(
                    min(col.rgb * col.rgb * col.rgb * 0.3h, 1.0h),
                    0.71h,
                    edge
                );

                col.rgb *= max(0.0h, baseLighting * _BaseColor.rgb);

                // Lightweight reflection/specular replacement for the old built-in path.
                // Assign a reflection texture if the material needs it.
                if (_SpecularColor.a > 0.001h)
                {
                    float3 viewDirWS = SafeNormalize(GetWorldSpaceViewDir(i.positionWS));
                    float3 reflectionDir = reflect(-viewDirWS, normalize(i.normalWS));

                    float2 reflectionUV;
                    reflectionUV.x = atan2(reflectionDir.x, reflectionDir.z) * 0.31831 + 0.5;
                    reflectionUV.y = reflectionDir.y * 0.5 + 0.5;

                    reflectionUV = reflectionUV * _ReflectionTex_ST.xy +
                                   _ReflectionTex_ST.zw * (_Time.y + i.customData.z);

                    half3 reflection = SAMPLE_TEXTURE2D(
                        _ReflectionTex, sampler_ReflectionTex, reflectionUV
                    ).rgb;

                    half luminance = dot(reflection, half3(0.3333,0.3333,0.3333));
                    reflection = lerp(luminance.xxx, reflection, _ReflectionSat);

                    half fresnel = pow(
                        1.0h - saturate(dot(viewDirWS, normalize(i.normalWS))),
                        _SpecularPower
                    );

                    col.rgb += baseLighting * reflection * _SpecularColor.rgb *
                               fresnel * preClipAlpha;
                }

                col.rgb = MixFog(col.rgb, i.fogFactor);
                return col;
            }
            ENDHLSL
        }
    }
}
