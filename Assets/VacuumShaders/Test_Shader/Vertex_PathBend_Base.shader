Shader "CRLuo/CRLuo_Vertex_PathBend_Base"
{
    Properties
    {
        _MainTex ("颜色纹理", 2D) = "white" {}
        _SwerveX("左右弯曲程度", Range(-0.003, 0.003)) = 0.0
        _SwerveY("上下弯曲程度", Range(-0.003, 0.003)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            //获取场景模型数据
            struct appdata
            {
                //获取模型顶点数据
                float4 vertex : POSITION;
                //获取模型第一套UV数据
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(4)
            };

            //颜色纹理
            sampler2D _MainTex;
            float _SwerveX;
            float _SwerveY;
            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                //获取模型的空间坐标
                float3 WordPos = mul(unity_ObjectToWorld, v.vertex);

                //左右坐标作为弯道 
				//依据Z坐标求平方获取弯曲曲线，越远离世界坐标原点，弯曲效果越明显。
				//最后乘以左右弯道弯曲方向 和 弯曲强度
				WordPos.x += pow(WordPos.z, 2)*_SwerveX;
                //上下坐标作为坡道
                //根据Z坐标求平方获取上下坡度，越远离世界坐标原点，上下坡效果越明显。
				//方法与上面相同，改变Y轴，获得上下坡效果
				WordPos.y += pow(WordPos.z, 2)*_SwerveY;

				//修正模型位置，WordPos 不包含物体自身的空间位移
				WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));

				//修改世界顶点转回物体自身顶点。
				v.vertex = mul(unity_WorldToObject, WordPos);

				//转换为裁切空间
				o.vertex = UnityObjectToClipPos(v.vertex);
				
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//获取颜色贴图
                fixed4 col = tex2D(_MainTex, i.uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
