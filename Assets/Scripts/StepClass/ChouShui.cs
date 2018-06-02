using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 抽取胸水
/// </summary>
public class ChouShui : StepBase
{
    /// <summary>
    /// 穿刺针
    /// </summary>
    GameObject needle;

    /// <summary>
    /// 止血钳子
    /// </summary>
    GameObject pliers;

    /// <summary>
    /// 胸穿刺针下的注射器
    /// </summary>
    GameObject syringe;

    /// <summary>
    /// 抽液中的胸穿刺针
    /// </summary>
    GameObject MyNeedle;

    /// <summary>
    /// 胶管打开
    /// </summary>
    GameObject OpenOr;

    /// <summary>
    /// 胶管关闭
    /// </summary>
    GameObject CloseOr;

    /// <summary>
    /// 注射器身体
    /// </summary>
    GameObject ZSQST;

    /// <summary>
    /// 注射器把手
    /// </summary>
    GameObject ZSQBS;

    /// <summary>
    /// 抽取的液体
    /// </summary>
    GameObject YT;

    /// <summary>
    /// 连接和取掉注射器按钮
    /// </summary>
    UPageButton zhusheqi;


    /// <summary>
    /// 止血钳图片按钮
    /// </summary>
    UPageButton ZhiShuiQian;

    /// <summary>
    /// 抽取的液体ml
    /// </summary>
    int ytml;

    /// <summary>
    /// 注射器中是否有液体
    /// </summary>
    bool isHasCy;

    /// <summary>
    /// 抽液抽满需要多少长度
    /// </summary>
    float cy;

    /// <summary>
    /// 注射器把手的本地初始z坐标
    /// </summary>
    float zsqbsZ;

    /// <summary>
    /// 装液体的瓶子
    /// </summary>
    UImage ytMisze;

    /// <summary>
    /// 显示
    /// </summary>
    UText text;

    /// <summary>
    /// 构造函数
    /// </summary>
    public ChouShui()
    {
        cy = 0.05f;
        isHasCy = false;
        //设置摄像机的位置
        cameraEnterPosition = new Vector3(-0.364f, 0.769f, 0.172f);
        //设置摄像机的旋转角度
        cameraEnterEuler = new Vector3(46.78023f, 72.23509f, 4.46308f);

        UImage backgroup = CreateUI<UImage>();
        backgroup.SetAnchored(AnchoredPosition.right);
        backgroup.rect = new Rect(-10, 0, 100, 500);
        backgroup.LoadImage("rihgtMenuBg");

        //止血钳按钮的位置
        ZhiShuiQian = CreateUI<UPageButton>();
        ZhiShuiQian.SetAnchored(AnchoredPosition.right);
        ZhiShuiQian.rect = new Rect(-10, -100, 100, 100);//-150
        ZhiShuiQian.LoadSprite("Hmostatic_forceps");
        ZhiShuiQian.LoadPressSprite("Hmostatic_forceps");
        ZhiShuiQian.text = "止血钳";
        ZhiShuiQian.onClick.AddListener(OnZhiShuiQian);

        //注射器按钮的位置
        zhusheqi = CreateUI<UPageButton>();
        zhusheqi.SetAnchored(AnchoredPosition.right);
        zhusheqi.rect = new Rect(-10, 100, 100, 100);//0
        zhusheqi.LoadSprite("Injector_50ml");
        zhusheqi.LoadPressSprite("Injector_50ml");
        zhusheqi.text = "50ml";
        zhusheqi.onClick.AddListener(OnZhusSheQi);


        //获取胸腔穿刺针的对象
        needle = GameObject.Find("Models").transform.Find("tools/chuancizhenfather/chuancizhen").gameObject;

        //得到自己的胸穿刺针的对象
        MyNeedle = GameObject.Find("Models").transform.Find("tools/胸穿针").gameObject;

        //注射器
        syringe = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi").gameObject;

        //止血钳
        pliers = GameObject.Find("Models").transform.Find("tools/胸穿针/助手扶针").gameObject;

        //胶管的开启开关
        OpenOr = GameObject.Find("Models").transform.Find("tools/胸穿针/Tube_clip_open").gameObject;
        CloseOr = GameObject.Find("Models").transform.Find("tools/胸穿针/Tube_clip_cl").gameObject;

        //让助手扶针和胸腔穿刺针位置融合
        MyNeedle.transform.position = needle.transform.position;
        MyNeedle.transform.rotation = needle.transform.rotation;

        //获取注射器的组件
        ZSQST = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/zhusheqi").gameObject;
        ZSQBS = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/bashou").gameObject;
        YT = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/yeti/Cylinder").gameObject;
        zsqbsZ = ZSQST.transform.localPosition.z;

        //未使用钳子固定
        AddScoreItem("10018101");
        //添加已连接50ml注射器的评分
        AddScoreItem("10018110");
        //添加松开钳子的评分
        AddScoreItem("10018120");
        //抽液有动作但没有抽到液体
        AddScoreItem("10018130");
        //抽液量不对
        AddScoreItem("10018141");
        //抽液完成后未关闭夹闭管开关
        AddScoreItem("10018161");
        //出现异常情况及时停止操作并退针-默认给分
        AddScoreItem("10018150");

    }



    /// <summary>
    /// 进入步骤的操作
    /// </summary>
    /// <param name="cameraTime"></param>
    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);
        //显示穿持针
        MyNeedle.SetActive(true);
        //隐藏注射器
        syringe.SetActive(false);
        //隐藏止血钳
        pliers.SetActive(false);
        //隐藏胸腔穿刺针
        needle.SetActive(false);
        //隐藏胶管的打开状态
        OpenOr.SetActive(false);
        //显示胶管的关闭状态
        CloseOr.SetActive(true);
        ZSQBS.transform.localPosition = ZSQST.transform.localPosition;
    }

    /// <summary>
    /// 是否连接止血钳点击按钮事件
    /// </summary>
    protected void OnZhiShuiQian()
    {
        //已经显示止血钳
        if (pliers.activeSelf)
        {
            ZhiShuiQian.LoadSprite("Hmostatic_forceps");
            ZhiShuiQian.LoadPressSprite("Hmostatic_forceps");

        }
        else//如果未显示止血钳
        {
            ZhiShuiQian.LoadSprite("Hmostatic_forceps_h");
            ZhiShuiQian.LoadPressSprite("Hmostatic_forceps_h");
            //添加显示钳子的评分
            AddScoreItem("10018100");
            //移除未显示钳子的评分
            RemoveScoreItem("10018101"); ;
        }
        pliers.SetActive(!pliers.activeSelf);

    }




    /// <summary>
    /// 是否连接注射器按钮点击事件
    /// </summary>
    protected void OnZhusSheQi()
    {

        //已经连接上注射器
        if (syringe.activeSelf)
        {
            zhusheqi.LoadSprite("Injector_50ml");
            zhusheqi.LoadPressSprite("Injector_50ml");
            if (isHasCy)
            {
                ZSQBS.transform.localPosition = new Vector3(ZSQBS.transform.localPosition.x, ZSQBS.transform.localPosition.y, zsqbsZ - cy);
                YT.transform.localPosition = new Vector3(YT.transform.localPosition.x, YT.transform.localPosition.y, -0.025f * ((ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) / cy));
                YT.transform.localScale = new Vector3(0.015f, 0.023f * ((ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) / cy), 0.015f);
                State = StepStatus.did;
            }
        }
        else//未连接上注射器
        {
            zhusheqi.LoadSprite("Injector_50ml_h");
            zhusheqi.LoadPressSprite("Injector_50ml_h");
            isHasCy = false;

        }
        syringe.SetActive(!syringe.activeSelf);

    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    /// <param name="lstBtnSetting">按钮设定</param>
    private void SettingBtn(Dictionary<string, KeyCode> lstBtnSetting)
    {
        // 分辨率：1920 x 1080
        //图片初期位置和大小
        Vector2 baseRec = new Vector2(-500, 500);
        int iCnt = 0;
        int firstRow = 3;
        int scendRow = 3;
        float btnWidth = 80;
        float btnHeight = 80;
        foreach (KeyValuePair<string, KeyCode> keyValue in lstBtnSetting)
        {
            Vector2 newRec;
            if (iCnt < firstRow)
            {
                newRec = new Vector2(baseRec.x + iCnt * btnWidth, baseRec.y);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            else if (iCnt < (firstRow + scendRow) && iCnt >= firstRow)
            {
                newRec = new Vector2(baseRec.x + (iCnt % firstRow) * btnWidth, baseRec.y - btnHeight * 1.5f);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            else
            {
                newRec = new Vector2(baseRec.x + (iCnt % (firstRow + scendRow)) * btnWidth, baseRec.y - btnHeight * 3);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            KeyCode key = keyValue.Value;
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).EnabledLongDown = true;
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).onClick.AddListener(() => { OnClickButton(key); });
            iCnt++;
        }
    }

    /// <summary>
    /// 镜头移动结束时
    /// </summary>
    public override void CameraMoveFinished()
    {
        //设置按钮
        SettingBtn(new Dictionary<string, KeyCode>
        {
             { "抽液", KeyCode.Alpha1 }
            ,{ "开关", KeyCode.Alpha2 }
        });

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickButton(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha1://抽液
                //如果注射器已经连上
                if (syringe.activeSelf)
                {
                    //如果夹闭关已经打开
                    if (OpenOr.activeSelf)
                    {
                        //如果止血钳未松开
                        if (!pliers.activeSelf)
                        {
                            if (ytMisze == null)
                            {
                                ytml = 0;
                                ytMisze = CreateUI<UImage>();
                                ytMisze.SetAnchored(AnchoredPosition.right);
                                ytMisze.rect = new Rect(-900, 0, 29, 49);
                                ytMisze.SetActive(true);
                                ytMisze.LoadImage("TotalVol");

                                text = CreateUI<UText>();
                                text.SetAnchored(AnchoredPosition.right);
                                text.rect = new Rect(-800, 0, 200, 80);
                                text.SetActive(true);

                            }
                            if (ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z < cy - 0.0001)
                            {
                                isHasCy = true;
                                ZSQBS.transform.localPosition = new Vector3(ZSQBS.transform.localPosition.x, ZSQBS.transform.localPosition.y, ZSQBS.transform.localPosition.z - cy / 5);
                                YT.transform.localPosition = new Vector3(YT.transform.localPosition.x, YT.transform.localPosition.y, -0.025f * ((ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) / cy));
                                YT.transform.localScale = new Vector3(0.015f, 0.023f * ((ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) / cy), 0.015f);
                                ytml += 10;
                                text.text = string.Format("{0} ml", ytml);
                                if (cy - (ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) >= 0 && cy - (ZSQST.transform.localPosition.z - ZSQBS.transform.localPosition.z) < 0.01)
                                {
                                    VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "CYHSXW", true, callbackXW);

                                }
                            }
                        }
                        else//如果止血钳未松开
                        {
                            //未松开止血钳
                            AddScoreItem("10018121");
                            //移除正确松开止血钳评分
                            RemoveScoreItem("10018120");
                            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "wskqz", true, null);
                        }
                    }
                    else//如果夹闭管开关未打开
                    {
                        //移除没有抽到液体的评分
                        RemoveScoreItem("10018130");
                        //添加抽到液体的评分
                        AddScoreItem("10018132");
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "wdkjbg", true, null);
                    }
                }
                else//如果注射器未连接上
                {
                    //未连接50ml注射器
                    AddScoreItem("10018111");
                    //移除正确链接50ml注射器评分
                    RemoveScoreItem("10018110");
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "wljzsq", true, null);
                }



                break;
            case KeyCode.Alpha2://开关
                OpenOr.SetActive(!OpenOr.activeSelf);
                CloseOr.SetActive(!CloseOr.activeSelf);
                if (OpenOr.activeSelf)
                {
                    if (!syringe.activeSelf)
                    {
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "fsxq", true, null);
                    }
                }
                else
                {
                    //关闭夹闭管的评分
                    AddScoreItem("10018160");
                    //移除未关闭的评分
                    RemoveScoreItem("10018161");
                }
                break;

            default:
                break;
        }
    }

    private void callbackXW()
    {
        Thread.Sleep(500);
        if (CGlobal.currentSceneInfo.sex == PeopleSex.女)
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "XBRHD", true);

        }
        if (CGlobal.currentSceneInfo.sex == PeopleSex.男)
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "YBRHD", true);
        }
        VoiceControlScript.Instance.SetCallBack(callbackEnd);
    }

    private void callbackEnd()
    {
        VoiceControlScript.Instance.VoiceStop();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    /// <summary>
    /// 步骤执行完成后的模型状态
    /// </summary>
    public override void StepEndState()
    {
        base.StepEndState();
    }

    /// <summary>
    /// 步骤结束时调用的方法
    /// </summary>
    public override void StepFinish()
    {
        base.StepFinish();
        //显示穿持针
        MyNeedle.SetActive(true);
        //隐藏注射器
        syringe.SetActive(false);
        //隐藏止血钳
        pliers.SetActive(false);
        //隐藏胸腔穿刺针
        needle.SetActive(false);
        //隐藏胶管的打开状态
        OpenOr.SetActive(false);
        //显示胶管的关闭状态
        CloseOr.SetActive(true);
        ButtonVirtualScript.Instance.RemoveVirtualButton();
        ButtonVirtualScript.Instance.ClearDicButton();

    }
}
