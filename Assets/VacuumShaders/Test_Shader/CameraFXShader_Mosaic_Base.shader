//���ʵ�·��������
Shader "CRLuo/CameraFXShader_Mosaic_Base"
{
    //���ʵı�������
    Properties
    {
        _MainTex ("��Ҫ�����ͼ��", 2D) = "white" {}
        _TileSize("��ߴ�", Range(0, 100)) = 10
    }
    //����ɫ��
    SubShader
    {
        //��Ⱦ����˳��
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            //�������ʹ�õĺ�����
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //��ȡ����ģ������
            struct appdata
            {
                //��ȡģ�Ͷ�������
                float4 vertex : POSITION;
                //��ȡģ�͵�һ��UV����
                float2 uv : TEXCOORD0;
            };

            //���嶥��Ƭ����ɫ����������
            struct v2f
            {
                //UV����
                float2 uv : TEXCOORD0;
                //��������
                float4 vertex : SV_POSITION;
            };

            //�������������ͼ
            sampler2D _MainTex;
            //��ȡ��ͼ��ƫ�����ظ�����
            float4 _MainTex_ST;
            //���������˳ߴ�
            uniform int _TileSize;

            //����Ƭ����ɫ������
            v2f vert(appdata v)
            {
                //�����������
                v2f o;
                //����ת��Ϊ������ӽ�����
                o.vertex = UnityObjectToClipPos(v.vertex);
                //Ӧ����ͼ��ƫ�����ظ�
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //���ģ�ͽ��
                return o;
            }

            //������ɫ������
            fixed4 frag(v2f i) : SV_Target
            {
                //��ȡ�����˿����� = ��Ļ���ظ���/������ظ���
                float2 TileSum = _ScreenParams / _TileSize;
                //����UV����һ�������˿��е�UVͳһΪһ������
                float2 uv_Mosaic = ceil(i.uv * TileSum) / TileSum;
                //��������UV���������Ⱦͼ�񣬻�������˻���
                fixed4 col_Mosaic = tex2D(_MainTex, uv_Mosaic);
                //���ͼ�� = ������ͼ��
                fixed4 col = col_Mosaic;
                //�����ͼ���
                return col;
            }
            ENDCG
        }
    }
}
