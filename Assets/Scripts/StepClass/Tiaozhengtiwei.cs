
using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 调整体位
/// </summary>
public class Tiaozhengtiwei : StepBase
{
    UTiaozhengtiwei ui = null;
    UPageButton currentButton = null;
    public Tiaozhengtiwei()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-1.581f, 0.808f, 0.939f);
        cameraEnterEuler = new Vector3(0, 90.6076f, 0);
        //设置人物(Models下)
        ui = CreateUI<UTiaozhengtiwei>();
    }
    /// <summary>
    /// 镜头移动完成开始执行步骤
    /// </summary>
    public override void CameraMoveFinished()
    {
        ui.StandPeople.SetActive(true);

        base.CameraMoveFinished();
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
            if (!ui.isShow)
            {
                ui.clickEvent += new ClickEventHandler(OnClickImageButton);

                //创建体位图
                ui.SetTiweiImages(new Dictionary<string, string>
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
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    public void OnClickImageButton(UPageButton btn)
    {
        if (currentButton != null)
        {
            currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        }
        currentButton = btn;
        currentButton.LoadSprite(btn.sprite.name + "_h");

        //体位选择错误
        if (btn.sprite.name.IndexOf("ok") > -1)
        {
            ui.StandPeople.SetActive(false);
            ui.SitPeople.SetActive(true);
            State = StepStatus.did;
        }
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "pricked_bone");
        }
    }
}
