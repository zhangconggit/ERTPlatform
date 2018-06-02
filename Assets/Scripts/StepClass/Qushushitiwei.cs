using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 协助患者取舒适体位
/// </summary>
public class Qushushitiwei : StepBase
{
    /// <summary>
    /// 坐着人model
    /// </summary>
    GameObject SitPeople;
    /// <summary>
    /// 处理完成
    /// </summary>
    bool isFinish;

    public Qushushitiwei()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.302f, 0.839f, 0.817f);
        cameraEnterEuler = new Vector3(10.9025f, 174.8688f, -4.269f);

        //坐着人model
        SitPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body").gameObject;

        //其他初期化
        isFinish = false;

        //追加评分
        AddScore(0);
    }

    /// <summary>
    /// 镜头移动完成
    /// </summary>
    public override void CameraMoveFinished()
    {
        //语音：协助患者取舒适体位完成
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SupinePosition");

        base.CameraMoveFinished();
    }


    /// <summary>
    /// 步骤更新
    /// </summary>
    public override void StepUpdate()
    {
        if (!isFinish)
        {
            //按下鼠标左键
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
                RaycastHit rHit;
                if (Physics.Raycast(ray, out rHit))
                {
                    //坐着人
                    if (rHit.collider.name != "chest_body")
                    {
                        if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                        {
                            //语音：点击患者,选择合适体位
                            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SupinePosition");
                        }
                    }
                }
                else
                {
                    if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                    {
                        //语音：点击患者,选择合适体位
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SupinePosition");
                    }
                }
            }
        }
        base.StepUpdate();
    }

    /// <summary>
    /// model点击事件
    /// </summary>
    /// <param name="obj">被点击的model</param>
    public override void OnClickModel(RaycastHit obj)
    {
        if (isFinish)
        {
            return;
        }
        //坐着人
        if (obj.collider.name == "chest_body")
        {
            //追加评分
            AddScore(1);
            isFinish = true;
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SupinePosition_Finished", true, Finish);
        }

        base.OnClickModel(obj);
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        VoiceControlScript.Instance.VoiceStop();
        base.StepFinish();
    }

    private void Finish()
    {
        //步骤完成
        State = StepStatus.did;
    }

    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10019151" } //没有协助患者取舒适体位
            ,{ 1, "100119150" } //协助患者取舒适体位
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
