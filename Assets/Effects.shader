Shader "Custom/Effects"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_DoodleMaxOffset("Doodle Max Offset", vector) = (0.005, 0.005, 0, 0)
		_DoodleFrameTime("Doodle Frame Time", Float) = 0.2
		_DoodleFrameCount("Doodle Frame Count", Int) = 24
		_DoodleNoiseScale("Doodle Noise Scale", vector) = (35, 35, 1, 1)
    }
    SubShader
    {
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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			float2 _DoodleMaxOffset;
			float _DoodleFrameTime;
			int _DoodleFrameCount;
			float2 _DoodleNoiseScale;

            float random(float2 seed)
            {
                return frac(sin(dot(seed.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float noise(float2 seed)
            {
                float2 i = floor(seed);
                float2 f = frac(seed);

                float a = random(i);
                float b = random(i + float2(1.0f, 0.0f));
                float c = random(i + float2(0.0f, 1.0f));
                float d = random(i + float2(1.0f, 1.0f));

                float2 u = f* f * (3.0f - 2.0f * f);

                return lerp(a, b, u.x) + (c - a) * u.y * (1.0f - u.x) + (d - b) * u.x * u.y;
            }

            float2 DoodleTextureOffset(float2 textureCoords, float2 maxOffset, float time, float frameTime, int frameCount, float2 noiseScale)
            {
                float timeValue = (floor(time / frameTime) % frameCount) + 1; 
                float2 offset = 0;
                float2 coordsPlusTime = (textureCoords + timeValue);
                offset.x = (noise(coordsPlusTime * noiseScale.x) * 2.0 - 1.0) * maxOffset.x;
                offset.y = (noise(coordsPlusTime * noiseScale.y) * 2.0 - 1.0) * maxOffset.y;
                return offset;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // DOODLE EFFECT
                float2 offset = 0.0;
				offset = DoodleTextureOffset(i.uv, _DoodleMaxOffset, _Time.y, _DoodleFrameTime, _DoodleFrameCount,_DoodleNoiseScale); 

				fixed4 col = tex2D(_MainTex, i.uv + offset);

                float x = (i.uv.x + 4.0 ) * (i.uv.y + 4.0 ) * (_Time.y * 10.0);
                fixed4 grain = fmod((fmod(x, 13.0) + 1.0) * (fmod(x, 123.0) + 1.0), 0.01) - 0.005;
                col += grain * 10.0;
                
                return col;
            }
            ENDCG
        }
    }
}
