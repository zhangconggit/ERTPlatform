using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class CAnimationPlay : MonoBehaviour
{
    public Texture2D m_PlayBg;

    //戴手套大图
    public Texture2D m_texWearGlove;
    //戴手套小图
    private Texture2D[,] m_texWearGloves;

    //洗手大图
    public Texture2D m_texSevenWashHand;
    //洗手小图
    private Texture2D[] m_texSevenWashHands;

    //当前帧
    private int m_iCurFram;
    //当前动画
    private int m_iCurAnimation;
    //动画帧总数
    private int m_iFrameCount;

    //戴手套小图的宽和高
    public int m_iMinPicWidth_w = 322;
    public int m_iMinPicHeight_w = 242;
    //戴手套一行有多少个小图
    public int m_iMinPicRowCount_w = 8;
    //戴手套一列有多少个小图
    public int m_iMinPicColumnCount_w = 8;


    //戴手套小图的宽和高
    public int m_iMinPicWidth_r = 319;
    public int m_iMinPicHeight_r = 240;

    //洗手一列有多少个小图
    public int m_iMinPicColumnCoun_r = 7;

    private float timerInterval = 0.2f;
    private float lasttime = 0;

    //动画控制
    public bool isPlayWearGloves = false;
    public bool isPlaySevenWashHand = false;

    void Start()
    {
        m_texWearGloves = new Texture2D[8, 8];
        for (int i = 0; i < m_iMinPicColumnCount_w; ++i)
        {
            for (int j = 0; j < m_iMinPicRowCount_w; ++j)
            {
                DePackTexture(i, j);
            }
        }
        m_iCurFram = 0;
        m_iCurAnimation = 7;

        m_texSevenWashHands = new Texture2D[7];
        for (int i = 0; i < m_iMinPicColumnCoun_r; ++i)
        {
            DePackTexture(i);
        }
    }

    void Update()
    {
           
    }

    public void PlayWearGloves()
    {
        StopWearGloves();
        timerInterval = 0.1f;
        isPlayWearGloves = true;
    }

    public void StopWearGloves()
    {
        m_iCurFram = 0;
        m_iCurAnimation = 7;
        isPlayWearGloves = false;
        isPlaySevenWashHand = false;
    }

    public void PlaySevenWashHand()
    {
        StopSevenWashHand();
        timerInterval = 0.4f;
        isPlaySevenWashHand = true;
    }

    public void StopSevenWashHand()
    {
        m_iCurFram = 0;
        isPlayWearGloves = false;
        isPlaySevenWashHand = false;
    }

    void OnGUI()
    {
        if (isPlayWearGloves)
        {
            ////CModelManager.isEnterLimitView = true;
            //CUIManager.GetInstance().showOrHideJoystickLeft(false);
            DrawAnimationWearGloves(m_texWearGloves, new Rect(50+80, 60, m_iMinPicWidth_w, m_iMinPicHeight_w));
            //绘制当前帧
            GUI.DrawTexture(new Rect(40+80, -18, m_iMinPicWidth_w+20, m_iMinPicHeight_w+210), m_PlayBg, ScaleMode.StretchToFill, true, 0);
            return;
        }
        if (isPlaySevenWashHand)
        {
            ////CModelManager.isEnterLimitView = true;
           // CUIManager.GetInstance().showOrHideJoystickLeft(false);
            DrawAnimationSevenWashHand(m_texSevenWashHands, new Rect(Screen.width-420, Screen.height-350, m_iMinPicWidth_r, m_iMinPicHeight_r));
            GUI.DrawTexture(new Rect(Screen.width - 420, Screen.height - 420, 320, 420), m_PlayBg, ScaleMode.StretchToFill, true, 0);
        }
        
    }

    //切图 戴手套
    void DePackTexture(int i, int j)
    {
        int cur_x = i * m_iMinPicWidth_w;
        int cur_y = j * m_iMinPicHeight_w;

        Texture2D newTexture = new Texture2D(m_iMinPicWidth_w, m_iMinPicHeight_w);

        for (int m = cur_y; m < cur_y + m_iMinPicHeight_w; ++m)
        {
            for (int n = cur_x; n < cur_x + m_iMinPicWidth_w; ++n)
            {
                newTexture.SetPixel(n - cur_x, m - cur_y, m_texWearGlove.GetPixel(n, m));
            }
        }     

        newTexture.Apply();
        m_texWearGloves[i, j] = newTexture;
    }

    //切图 洗手
    void DePackTexture(int i)
    {
        int cur_x = i * m_iMinPicWidth_r;
        int cur_y = 0;

        Texture2D newTexture = new Texture2D(m_iMinPicWidth_r, m_iMinPicHeight_w);

        for (int m = cur_y; m < cur_y + m_iMinPicHeight_w; ++m)
        {
            for (int n = cur_x; n < cur_x + m_iMinPicWidth_r; ++n)
            {
                newTexture.SetPixel(n - cur_x, m - cur_y, m_texSevenWashHand.GetPixel(n, m));
            }
        }

        newTexture.Apply();
        m_texSevenWashHands[i] = newTexture;
    }


    void DrawAnimationWearGloves(Texture[,] tex, Rect rect)
    {
        //绘制当前帧
        GUI.DrawTexture(rect, tex[m_iCurFram, m_iCurAnimation], ScaleMode.StretchToFill, true, 0);

        if (Time.time - timerInterval > lasttime)
        {
            m_iCurFram = m_iCurFram % 8;
            lasttime = Time.time;

            m_iCurFram++;

            if (m_iCurFram >= 8)
            {        
                m_iCurAnimation--;
                if (m_iCurFram >= 8 && m_iCurAnimation < 0)
                {
                    isPlayWearGloves = false;
                    //播放完戴手套之后
                    // //CModelManager.isEnterLimitView = false;
                    //CUIManager.GetInstance().showOrHideJoystickLeft(true);

                    
                }
                    
                if (m_iCurAnimation < 0)
                    m_iCurAnimation = 7;
                m_iCurFram = 0;
                
            }
            
        }
    }

    void DrawAnimationSevenWashHand(Texture[] tex, Rect rect)
    {
        //绘制当前帧
        GUI.DrawTexture(rect, tex[m_iCurFram], ScaleMode.StretchToFill, true, 0);

        if (Time.time - timerInterval > lasttime)
        {
            m_iCurFram = m_iCurFram % 7;
            lasttime = Time.time;

            m_iCurFram++;

            if (m_iCurFram >= 7)
            {
                isPlaySevenWashHand = false;
                //洗手结束
                ////CModelManager.isEnterLimitView = false;
                //CUIManager.GetInstance().showOrHideJoystickLeft(true);
                //if (COperationData.GetInstance().getIsDragCurvePlateOnRub() == false || COperationData.GetInstance().getIsDragSteriletowelOnRub() == false)
                //{
                //    CUIManager.GetInstance().showAssist("请您点击手套并检查手套包装日期，然后带上手套。");
                //    Speak.Instance.playAudio("wearGloves");
                //}
                //else
                //{
                //    CUIManager.GetInstance().showAssist("请协助患者平躺。");
                //    Speak.Instance.playAudio("pingtang");
                //}
                m_iCurFram = 0;
            }

        }
    }

}