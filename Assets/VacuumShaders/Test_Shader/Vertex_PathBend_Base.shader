Shader "CRLuo/CRLuo_Vertex_PathBend_Base"
{
    Properties
    {
        _MainTex ("��ɫ����", 2D) = "white" {}
        _SwerveX("���������̶�", Range(-0.003, 0.003)) = 0.0
        _SwerveY("���������̶�", Range(-0.003, 0.003)) = 0.0
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

            //��ȡ����ģ������
            struct appdata
            {
                //��ȡģ�Ͷ�������
                float4 vertex : POSITION;
                //��ȡģ�͵�һ��UV����
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(4)
            };

            //��ɫ����
            sampler2D _MainTex;
            float _SwerveX;
            float _SwerveY;
            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                //��ȡģ�͵Ŀռ�����
                float3 WordPos = mul(unity_ObjectToWorld, v.vertex);

                //����������Ϊ��� 
				//����Z������ƽ����ȡ�������ߣ�ԽԶ����������ԭ�㣬����Ч��Խ���ԡ�
				//��������������������� �� ����ǿ��
				WordPos.x += pow(WordPos.z, 2)*_SwerveX;
                //����������Ϊ�µ�
                //����Z������ƽ����ȡ�����¶ȣ�ԽԶ����������ԭ�㣬������Ч��Խ���ԡ�
				//������������ͬ���ı�Y�ᣬ���������Ч��
				WordPos.y += pow(WordPos.z, 2)*_SwerveY;

				//����ģ��λ�ã�WordPos ��������������Ŀռ�λ��
				WordPos -= mul(unity_ObjectToWorld, float4(0, 0, 0, 1));

				//�޸����綥��ת�����������㡣
				v.vertex = mul(unity_WorldToObject, WordPos);

				//ת��Ϊ���пռ�
				o.vertex = UnityObjectToClipPos(v.vertex);
				
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//��ȡ��ɫ��ͼ
                fixed4 col = tex2D(_MainTex, i.uv);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
