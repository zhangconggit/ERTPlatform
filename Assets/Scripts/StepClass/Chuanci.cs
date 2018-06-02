using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.UI.View;

/// <summary>
/// 穿刺
/// </summary>
public class Chuanci : StepBase
{
    /// <summary>
    /// 穿刺针父节点
    /// </summary>
    GameObject chuancizhenfather;
    /// <summary>
    /// 助手扶针
    /// </summary>
    GameObject zhushoufuzhen;
    /// <summary>
    /// 穿刺针
    /// </summary>
    GameObject chuancizhen;
    /// <summary>
    /// 穿刺针-开
    /// </summary>
    GameObject chuancizhen_open;
    /// <summary>
    /// 穿刺针-关
    /// </summary>
    GameObject chuancizhen_close;
    /// <summary>
    /// 默认垂直角度
    /// </summary>
    Quaternion defaultChuizhi;
    /// <summary>
    /// 胸腔穿刺结束按钮
    /// </summary>
    UPageButton btnChuanciOk;
    bool isCanRotat;

    UPuncture puncture;

    public Chuanci()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);

        //穿刺针父节点
        chuancizhenfather = GameObject.Find("Models").transform.Find("tools/chuancizhenfather").gameObject;
        chuancizhenfather.SetActive(false);

        //助手扶针
        zhushoufuzhen = GameObject.Find("Models").transform.Find("tools/chuancizhenfather/zhushoufuzhen").gameObject;
        zhushoufuzhen.SetActive(false);

        //穿刺针
        chuancizhen = GameObject.Find("Models").transform.Find("tools/chuancizhenfather/chuancizhen").gameObject;

        //穿刺针-开
        chuancizhen_open = GameObject.Find("Models").transform.Find("tools/chuancizhenfather/chuancizhen/tube_clip_open").gameObject;
        chuancizhen_open.SetActive(true);

        //穿刺针-关
        chuancizhen_close = GameObject.Find("Models").transform.Find("tools/chuancizhenfather/chuancizhen/tube_clip_cl").gameObject;
        chuancizhen_close.SetActive(false);

        //胸腔穿刺剖面图-针
        puncture = new UPuncture(CreateUI<UImage>);

        isCanRotat = true;

        //追加评分
        AddDefaultScore();
    }
    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //显示穿刺针父节点
        chuancizhenfather.transform.position = Model_PunctureInfo.Instance.m_PuncturePoint;
        defaultChuizhi = Quaternion.FromToRotation(Vector3.left, Model_PunctureInfo.Instance.m_PunctureNormal);
        chuancizhenfather.transform.rotation = defaultChuizhi;
        chuancizhenfather.SetActive(true);

        //胸腔穿刺结束按钮
        btnChuanciOk = new UPageButton(AnchoredPosition.bottom);
        btnChuanciOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        btnChuanciOk.text = "胸腔穿刺结束";
        btnChuanciOk.LoadSprite("anniu-160");
        btnChuanciOk.LoadPressSprite("anniu-160h");
        btnChuanciOk.button.text.color = UnityEngine.Color.white;
        btnChuanciOk.onClick.AddListener(onClickChuanciOk);
        btnChuanciOk.SetActive(true);

        //设置按钮
        SettingBtn(new Dictionary<string, KeyCode>
        {
             { "前进", KeyCode.Alpha1 }
            ,{ "后退", KeyCode.Alpha2 }
            ,{ "开关", KeyCode.Alpha4 }
            ,{ "顺时针", KeyCode.Q }
            ,{ "上转", KeyCode.W }
            ,{ "逆时针", KeyCode.E }
            ,{ "左转", KeyCode.A }
            ,{ "下转" , KeyCode.S}
            ,{ "右转", KeyCode.D}
        });
        //语音：开始穿刺
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "start_cc");

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        //摧毁胸腔穿刺结束按钮
        btnChuanciOk.Destroy();
        ButtonVirtualScript.Instance.RemoveVirtualButton();
        ButtonVirtualScript.Instance.ClearDicButton();

        VoiceControlScript.Instance.VoiceStop();
        base.StepFinish();
    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    /// <param name="lstBtnSetting">按钮设定</param>
    private void SettingBtn(Dictionary<string, KeyCode> lstBtnSetting)
    {
        // 分辨率：1920 x 1080
        //图片初期位置和大小
        Vector2 baseRec = new Vector2(-750, 500);
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
            }
            else if (iCnt < (firstRow + scendRow) && iCnt >= firstRow)
            {
                newRec = new Vector2(baseRec.x + (iCnt % firstRow) * btnWidth, baseRec.y - btnHeight * 1.5f);
            }
            else
            {
                newRec = new Vector2(baseRec.x + (iCnt % (firstRow + scendRow)) * btnWidth, baseRec.y - btnHeight * 3);
            }

            KeyCode key = keyValue.Value;
            ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).EnabledLongDown = true;
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).onClick.AddListener(() => { OnClickButton(key); });

            iCnt++;
        }
    }

    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickButton(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Alpha1://前进
                if (zhushoufuzhen.activeSelf == false)
                {
                    switch (puncture.GetXQCCLocation())
                    {
                        case "体外":
                            if (chuancizhen_open.activeSelf)
                            {
                                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                                {
                                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "rube_clip_no_gabi_maybe_qixiong");
                                }
                            }
                            break;
                        case "皮下及脂肪":
                            if (chuancizhenfather.transform.rotation != defaultChuizhi)
                            {
                                isCanRotat = false;
                                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                                {
                                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "VerticalIntoNeedle");
                                }
                            }
                            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                            {
                                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Pi");
                            }
                            break;
                        case "背阔肌":
                            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                            {
                                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Beikouji");
                            }
                            break;
                        case "肋间内肌肉":
                            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                            {
                                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Neiji");
                            }
                            break;
                        case "胸腔":
                            if (chuancizhen_open.activeSelf)
                            {
                                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                                {
                                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "rube_clip_no_gabi");
                                }
                            }

                            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                            {
                                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Xiongqiang");
                            }
                            break;
                        case "肺":
                            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                            {
                                //咳嗽
                                VoiceControlScript.Instance.AudioPlay(AudioStyle.Environment, "patient_light_cough");
                            }
                            return;
                        default:
                            break;
                    }

                    chuancizhen.transform.localPosition = new Vector3(chuancizhen.transform.localPosition.x + 0.001f
                                                               , chuancizhen.transform.localPosition.y
                                                               , chuancizhen.transform.localPosition.z);
                    //胸腔侧面剖视图
                    puncture.SyringeForward(true);
                }
                break;
            case KeyCode.Alpha2://后退
                if (zhushoufuzhen.activeSelf == false)
                {
                    if (puncture.GetXQCCLocation() == "体外")
                    {
                        isCanRotat = true;
                    }
                    chuancizhen.transform.localPosition = new Vector3(chuancizhen.transform.localPosition.x - 0.001f
                                                                , chuancizhen.transform.localPosition.y
                                                                , chuancizhen.transform.localPosition.z);
                    puncture.SyringeForward(false);                    
                }
                break;
            case KeyCode.Alpha4://开关
                //穿刺针-开
                chuancizhen_open.SetActive(!chuancizhen_open.activeSelf);
                if (chuancizhen_open.activeSelf)
                {
                    if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                    {
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "rube_clip_no_gabi_maybe_qixiong");
                    }
                }
                //穿刺针-关
                chuancizhen_close.SetActive(!chuancizhen_close.activeSelf);
                break;
            case KeyCode.Q://顺时针
                if (isCanRotat)
                {
                    chuancizhen.transform.Rotate(new Vector3(1, 0, 0), -10f);
                }
                break;
            case KeyCode.W://上转
                if (isCanRotat)
                {
                    chuancizhenfather.transform.Rotate(new Vector3(0, 0, 1), -1f);

                    puncture.DeflectionUp(true);
                }
                break;
            case KeyCode.E://逆时针
                if (isCanRotat)
                {
                    chuancizhen.transform.Rotate(new Vector3(1, 0, 0), 10f);
                }
                break;
            case KeyCode.A://左转
                if (isCanRotat)
                {
                    chuancizhenfather.transform.Rotate(new Vector3(0, 1, 0), 1f);
                }
                break;
            case KeyCode.S://下转
                if (isCanRotat)
                {
                    chuancizhenfather.transform.Rotate(new Vector3(0, 0, 1), 1f);

                    puncture.DeflectionUp(false);
                }
                break;
            case KeyCode.D://右转
                if (isCanRotat)
                {
                    chuancizhenfather.transform.Rotate(new Vector3(0, 1, 0), -1f);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 点击胸腔穿刺结束按钮
    /// </summary>
    private void onClickChuanciOk()
    {
        //步骤结束
        State = StepStatus.did;

        if (chuancizhen_open.activeSelf == false)
        {
            //追加评分(夹闭橡皮胶管)
            AddJiaoguanScore(1);
        }
        if (chuancizhenfather.transform.rotation == defaultChuizhi)
        {
            //追加评分(穿刺针进针角度)
            AddJiaoduScore(1);
        }
        float dept = puncture.GetDept();
        if (dept < 130f)
        {
            //追加评分(穿刺深度)
            AddShenduScore(0);
        }
        else if (dept > 225f)
        {
            //追加评分(穿刺深度)
            AddShenduScore(-1);
        }
        else
        {
            //追加评分(穿刺深度)
            AddShenduScore(1);
        }
    }
    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddDefaultScore()
    {
        //追加评分(夹闭橡皮胶管)
        AddJiaoguanScore(0);
        //追加评分(穿刺针进针角度)
        AddJiaoduScore(0);
        //追加评分(穿刺深度)
        AddShenduScore(0);
    }
    /// <summary>
    /// 追加评分(夹闭橡皮胶管)
    /// </summary>
    private void AddJiaoguanScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10017101" } //没有夹住橡皮胶管，空气进入胸腔，发生气胸
            ,{ 1, "10017100" } //止血钳夹住橡皮胶管方法正确
        };
        if (scoreKey == 0)
        {
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    //移除评分编码
                    RemoveScoreItem(scoreItem);
                }
            }
            //追加评分编码
            AddScoreItem(dicScore[scoreKey]);
        }
        else
        {
            int recordCnt = 0;
            bool isShouldAdd = true;
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    if (recordCnt == 0)
                    {
                        //移除评分编码
                        RemoveScoreItem(scoreItem);
                    }
                    else
                    {
                        isShouldAdd = false;
                        break;
                    }
                }
                recordCnt++;
            }
            if (isShouldAdd)
            {
                //追加评分编码
                AddScoreItem(dicScore[scoreKey]);
            }
        }
    }

    /// <summary>
    /// 追加评分(穿刺针进针角度)
    /// </summary>
    private void AddJiaoduScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10017111" } //不是垂直进针
            ,{ 1, "10017110" } //垂直进针   
        };
        if (scoreKey == 0)
        {
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    //移除评分编码
                    RemoveScoreItem(scoreItem);
                }
            }
            //追加评分编码
            AddScoreItem(dicScore[scoreKey]);
        }
        else
        {
            int recordCnt = 0;
            bool isShouldAdd = true;
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    if (recordCnt == 0)
                    {
                        //移除评分编码
                        RemoveScoreItem(scoreItem);
                    }
                    else
                    {
                        isShouldAdd = false;
                        break;
                    }
                }
                recordCnt++;
            }
            if (isShouldAdd)
            {
                //追加评分编码
                AddScoreItem(dicScore[scoreKey]);
            }
        }
    }

    /// <summary>
    /// 追加评分(穿刺深度)
    /// </summary>
    private void AddShenduScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10017121" } //穿刺过浅（没有到胸膜腔）
            ,{ -1, "10017122" } //穿刺过深（穿刺到肺）   
            ,{ 1, "10017120" } //穿刺深度合适(进入胸膜腔)   
        };
        if (scoreKey == 0)
        {
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    //移除评分编码
                    RemoveScoreItem(scoreItem);
                }
            }
            //追加评分编码
            AddScoreItem(dicScore[scoreKey]);
        }
        else
        {
            int recordCnt = 0;
            bool isShouldAdd = true;
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    if (recordCnt == 0)
                    {
                        //移除评分编码
                        RemoveScoreItem(scoreItem);
                    }
                    else
                    {
                        isShouldAdd = false;
                        break;
                    }
                }
                recordCnt++;
            }
            if (isShouldAdd)
            {
                //追加评分编码
                AddScoreItem(dicScore[scoreKey]);
            }
        }
    }
}
