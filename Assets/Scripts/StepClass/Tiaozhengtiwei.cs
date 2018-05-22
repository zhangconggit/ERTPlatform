using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 调整体位
/// </summary>
public class Tiaozhengtiwei : StepBase
{
    UPageButton currentButton = null;
    GameObject StandingPeople;
    GameObject SitPeople;
    bool isShow;
    public Tiaozhengtiwei()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-1.581f, 0.808f, 0.939f);
        cameraEnterEuler = new Vector3(0, 90.6076f, 0);

        //站立人model
        StandingPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body_s").gameObject;
        StandingPeople.transform.localPosition = new Vector3(-0.5139443f, -0.002361169f, -0.3850695f);
        StandingPeople.transform.localRotation = Quaternion.Euler(-90, 92.6531f, 0);
        StandingPeople.SetActive(true);

        //坐着人model
        SitPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body").gameObject;
        SitPeople.transform.localPosition = new Vector3(1.187028f, -0.002361169f, 0.2012178f);
        SitPeople.transform.localRotation = Quaternion.Euler(270, 0, 0);
        SitPeople.SetActive(false);

        //其他初期化
        isShow = false;
    }
    /// <summary>
    /// 调整体位_人物点击事件
    /// </summary>
    /// <param name="obj">被点击的model</param>
    public override void OnClickModel(RaycastHit obj)
    {
        //调整体位_人物被点中
        if (obj.collider.name == "chest_body_s")
        {
            if (!isShow)
            {
                //创建体位图
                SetTiweiImages(new Dictionary<string, string>
                {
                     { "check_posture_1","check_posture_1_h" }
                    ,{ "check_posture_2","check_posture_2_h" }
                    ,{ "check_posture_3","check_posture_3_h" }
                    ,{ "check_posture_4_ok","check_posture_4_ok_h" }
                });
            }
        }

        base.OnClickModel(obj);
    }

    /// <summary>
    /// 创建体位图
    /// </summary>
    /// <param name="lstTiweiImagePath"></param>
    private void SetTiweiImages(Dictionary<string, string> lstTiweiImagePath)
    {
        //分辨率：1920 x 1080
        //图片初期位置和大小
        Rect pos = new Rect(0, 0, 400, 600);
        //图片间的间距
        int diff = 50;
        int i_imagesCnt = lstTiweiImagePath.Count;
        //随机数
        int[] ran = MyCommon.RandomRepeat(i_imagesCnt);

        int index = 0;
        foreach (KeyValuePair<string, string> keyValue in lstTiweiImagePath)
        {
            pos.x = ran[index] * (pos.width + diff) - (pos.width + diff) * i_imagesCnt / 2 + pos.width / 2 + diff / 2;

            CreatImageBtn(keyValue.Key, keyValue.Value, pos);
            index++;
        }
        isShow = true;
    }

    /// <summary>
    /// 创建体位图按钮
    /// </summary>
    /// <param name="defPath">初期图片</param>
    /// <param name="path">图片</param>
    /// <param name="pos">图片位置</param>
    private void CreatImageBtn(string defPath, string path, Rect pos)
    {
        UPageButton btn = CreateUI<UPageButton>();
        btn.name = path;
        btn.SetAnchored(AnchoredPosition.center);
        btn.rect = pos;
        btn.LoadSprite(defPath);
        btn.LoadPressSprite(path);

        btn.onClick.AddListener(() => { OnClickImageButton(btn); });
    }

    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickImageButton(UPageButton btn)
    {
        if (currentButton != null)
        {
            currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        }
        currentButton = btn;
        currentButton.LoadSprite(btn.pressSprite.name);

        //体位选择正确
        if (btn.sprite.name.IndexOf("ok") > -1)
        {
            StandingPeople.SetActive(false);
            SitPeople.SetActive(true);

            //结束当前步骤
            State = StepStatus.did;
        }
        //体位选择错误
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "pricked_bone");
        }
    }
}
