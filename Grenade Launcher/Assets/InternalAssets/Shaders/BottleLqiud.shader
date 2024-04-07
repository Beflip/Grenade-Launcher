Shader "Custom/BottleLqiud"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorTop ("Color Top", Color) = (1,1,1,1)
        _Progress ("Progress", Float) = 0
        _Wobble ("Wobble", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 localPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float3 objectPos = mul(unity_ObjectToWorld, float4(0,0,0,1));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);

                half3x3 m = (half3x3)UNITY_MATRIX_M;
                half3 objectScale = half3(
                    length( half3( m[0][0], m[1][0], m[2][0] ) ),
                    length( half3( m[0][1], m[1][1], m[2][1] ) ),
                    length( half3( m[0][2], m[1][2], m[2][2] ) )
                );

                o.localPos = (worldPos - objectPos) / objectScale;
                o.uv = v.uv;
                return o;
            }

            fixed4 _Color;
            fixed4 _ColorTop;
            float _Progress;
            float4 _Wobble;

            void Unity_RotateAboutAxis_Radians_float(float3 In, float3 Axis, float Rotation, out float3 Out)
            {
                float s = sin(Rotation);
                float c = cos(Rotation);
                float one_minus_c = 1.0 - c;

                Axis = normalize(Axis);
                float3x3 rot_mat =
                {   one_minus_c * Axis.x * Axis.x + c, one_minus_c * Axis.x * Axis.y - Axis.z * s, one_minus_c * Axis.z * Axis.x + Axis.y * s,
                    one_minus_c * Axis.x * Axis.y + Axis.z * s, one_minus_c * Axis.y * Axis.y + c, one_minus_c * Axis.y * Axis.z - Axis.x * s,
                    one_minus_c * Axis.z * Axis.x - Axis.y * s, one_minus_c * Axis.y * Axis.z + Axis.x * s, one_minus_c * Axis.z * Axis.z + c
                };
                Out = mul(rot_mat,  In);
            }

            fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
            {
                float3 rotatedLocalSpace;
                Unity_RotateAboutAxis_Radians_float(i.localPos, float3(1, 0, 0), -_Wobble.x, rotatedLocalSpace);
                Unity_RotateAboutAxis_Radians_float(rotatedLocalSpace, float3(0, 0, 1), -_Wobble.y, rotatedLocalSpace);
                clip(-(rotatedLocalSpace.y - _Progress));
                return fixed4(facing > 0 ? _Color.rgb : _ColorTop.rgb, 1);
            }
            ENDCG
        }
    }
}