//材质的路径与名称
Shader "CRLuo/CameraFXShader_Mosaic_Base"
{
    //材质的变量定义
    Properties
    {
        _MainTex ("需要处理的图像", 2D) = "white" {}
        _TileSize("块尺寸", Range(0, 100)) = 10
    }
    //子着色器
    SubShader
    {
        //渲染队列顺序
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            //引入程序使用的函数集
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //获取场景模型数据
            struct appdata
            {
                //获取模型顶点数据
                float4 vertex : POSITION;
                //获取模型第一套UV数据
                float2 uv : TEXCOORD0;
            };

            //定义顶点片段着色器数据类型
            struct v2f
            {
                //UV数据
                float2 uv : TEXCOORD0;
                //顶点数据
                float4 vertex : SV_POSITION;
            };

            //载入材质球主贴图
            sampler2D _MainTex;
            //获取贴图的偏移与重复数据
            float4 _MainTex_ST;
            //载入马赛克尺寸
            uniform int _TileSize;

            //顶点片段着色器程序
            v2f vert(appdata v)
            {
                //定义输出数据
                v2f o;
                //顶点转换为摄像机视角坐标
                o.vertex = UnityObjectToClipPos(v.vertex);
                //应用贴图的偏移与重复
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //输出模型结果
                return o;
            }

            //表面着色器程序
            fixed4 frag(v2f i) : SV_Target
            {
                //获取马赛克块数量 = 屏幕像素个数/块的像素个数
                float2 TileSum = _ScreenParams / _TileSize;
                //整理UV，让一个马赛克块中的UV统一为一个数据
                float2 uv_Mosaic = ceil(i.uv * TileSum) / TileSum;
                //把整理后的UV结果赋给渲染图像，获得马赛克画面
                fixed4 col_Mosaic = tex2D(_MainTex, uv_Mosaic);
                //输出图像 = 马赛克图像
                fixed4 col = col_Mosaic;
                //输出贴图结果
                return col;
            }
            ENDCG
        }
    }
}
