/****************************************************

	添加UI步骤：
            1.拼接UI，之后点击BuildFileList->创建EditorFileList
            2.创建UI对应脚本(包括 XXX_Command、XXX_Mediator、XXX_Proxy)
            3.在EventDefine中添加 XXX_StartUp、XXX_FillInfo、XXX_Hide三个基本事件
            4.在GameFacade中注册对应UICommand
            5.如果是AB包运行，还需要在AssetBundleName.txt中设置包名和路径，点击设置包名，构架包体

*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class UI_FPS : BaseUI
{
    private Text text_FPS;

    /// <summary>
    /// 上一次更新频率的时间
    /// </summary>
    private float m_LastUpdateShowTime = 0f;

    /// <summary>
    /// 更新频率的时间间隔
    /// </summary>
    private float m_UpdateShowDeltaTime = 0.25f;

    /// <summary>
    /// 帧数
    /// </summary>
    private int m_FrameUpdate = 0;

    private float m_FPS = 0;

    public override void Awake()
    {
	    base.Awake();
        this.text_FPS = this.transform.Find("Top_Right/text_FPS").GetComponent<Text>();
    }

    void Start()
    {
        this.m_LastUpdateShowTime = Time.realtimeSinceStartup;
    }
    private void Update()
    {
        this.m_FrameUpdate++;
        if (Time.realtimeSinceStartup - this.m_LastUpdateShowTime >= this.m_UpdateShowDeltaTime)
        {
            this.m_FPS = this.m_FrameUpdate / (Time.realtimeSinceStartup - this.m_LastUpdateShowTime);
            this.m_FrameUpdate = 0;
            this.m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }

        if (this.text_FPS)
        {
            this.text_FPS.text = "FPS:" + this.m_FPS.ToString("f2");
        }
    }
}
