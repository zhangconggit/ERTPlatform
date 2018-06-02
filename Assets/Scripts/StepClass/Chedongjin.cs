using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 撤洞巾
/// </summary>
public class Chedongjin : StepBase
{
    /// <summary>
    /// 垃圾桶
    /// </summary>
    GameObject lajitong;
    /// <summary>
    /// 洞巾
    /// </summary>
    GameObject dongjin;
    /// <summary>
    /// 洞巾-折叠
    /// </summary>
    GameObject dongjin_zhedie;
    /// <summary>
    /// 洞巾-折叠原来位置
    /// </summary>
    Vector3 dongjin_zhedieOldPos;
    /// <summary>
    /// 点击洞巾时保持当前位置
    /// </summary>
    Vector3 colliderPos;
    /// <summary>
    /// 松开洞巾判定
    /// </summary>
    bool candiulaiji;
    int iFirst = 0;

    public Chedongjin()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.2376f, 0.9243f, 0.7139f);
        cameraEnterEuler = new Vector3(21.3393f, 132.2796f, -3.5276f);

        //洞巾
        dongjin = GameObject.Find("Models").transform.Find("tools/dongjin").gameObject;

        //洞巾-折叠
        dongjin_zhedie = GameObject.Find("Models").transform.Find("tools/dongjin_zhedie").gameObject;
        dongjin_zhedie.transform.position = new Vector3(-0.24f, 0.7366f, 0.2982f);
        dongjin_zhedie.transform.rotation = Quaternion.Euler(-1.2733f, 682.8341f, -359.6138f);
        dongjin_zhedie.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        dongjin_zhedie.SetActive(false);

        //垃圾桶
        lajitong = GameObject.Find("Models").transform.Find("tools/lajitong").gameObject;
        lajitong.transform.position = new Vector3(0.669f, 0f, 0.482f);
        lajitong.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        lajitong.SetActive(false);

        //追加评分
        AddScore(0);
    }

    /// <summary>
    /// 镜头移动完成
    /// </summary>
    public override void CameraMoveFinished()
    {
        //显示洞巾
        dongjin.SetActive(true);
        //显示垃圾桶
        lajitong.SetActive(true);
        //语音：拖动洞巾，放入垃圾桶
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "ThrowGarbage");
        dongjin.transform.Find("chest_Sterile_hole_towel_t").GetComponent<BoxCollider>().enabled = true;

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        VoiceControlScript.Instance.VoiceStop();
        base.StepFinish();
    }

    /// <summary>
    /// 步骤更新
    /// </summary>
    public override void StepUpdate()
    {
        if (lajitong.activeSelf)
        {
            if (Input.GetMouseButton(0))
            {
                candiulaiji = false;
                Vector3 mouseVector3 = Camera.main.ScreenToWorldPoint(new Vector3(
                          Input.mousePosition.x
                        , Input.mousePosition.y
                        , colliderPos.z
                ));
                dongjin_zhedie.transform.position = mouseVector3;
                if (iFirst < 1)
                {
                    iFirst++;
                    dongjin_zhedieOldPos = mouseVector3;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
                RaycastHit hit;//射线的终点
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "lajitong")
                    {
                        candiulaiji = true;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (candiulaiji)
                {
                    //语音：开始穿刺
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "ThrowGarbage_Finished");

                    //洞巾-折叠
                    dongjin_zhedie.SetActive(false);
                    //显示垃圾桶
                    lajitong.SetActive(false);
                    //追加评分
                    AddScore(1);
                    //步骤结束
                    State = StepStatus.did;
                }
                else
                {
                    if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                    {
                        //语音：拖动洞巾，放入垃圾桶
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "ThrowGarbage");
                    }
                    //显示洞巾
                    dongjin.SetActive(true);
                    //洞巾-折叠
                    dongjin_zhedie.SetActive(false);
                    //洞巾-折叠回归原位
                    dongjin_zhedie.transform.position = dongjin_zhedieOldPos;
                    //追加评分
                    AddScore(-1);
                }
            }
        }
        base.StepUpdate();
    }

    /// <summary>
    /// 鼠标点击坐着人物触发事件
    /// </summary>
    /// <param name="obj"></param>
    public override void MouseClickModel(RaycastHit obj)
    {
        if (obj.transform.name == "chest_Sterile_hole_towel_t")
        {
            colliderPos = obj.point;
            //洞巾
            dongjin.SetActive(false);
            //洞巾-折叠
            dongjin_zhedie.SetActive(true);
        }
        base.MouseClickModel(obj);
    }

    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10019131" } //没有撤洞巾
            ,{ -1, "10019132" } //洞巾投放位置错误
            ,{ 1, "100119130" } //洞巾投放位置正确
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
