using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 叩诊
/// </summary>
public class Kouzhen : StepBase
{
    UKouzhenHand ui = null;

    public Kouzhen()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);

        //设置人物(Models下)
        ui = CreateUI<UKouzhenHand>();
    }
    public override void MouseClickModel(RaycastHit obj)
    {
        bool isKouzhen = true;
        switch (obj.transform.name)
        {
            case "chest_gap6"://"第六肋间隙"
            case "chest_rib7"://"第七肋"
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Environment, "knock_hollow", false);//播放清音
                break;
            case "chest_blade":// "肩胛骨"
            case "chest_rib8": //第八肋
            case "chest_rib9": //第九肋
            case "chest_rib10": //第十肋
            case "chest_rib11": //第十一肋
            case "chest_gap7": //第七肋间隙
            case "chest_gap8": //第八肋间隙
            case "chest_gap9": //第九肋间隙
            case "chest_gap10": //第十肋间隙
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Environment, "knock_solid", false);//播放实音
                break;
            default:
                isKouzhen = false;
                break;
        }
        if (isKouzhen)
        {
            ui.MoveHands(obj.point, obj.normal);
        }
        base.MouseClickModel(obj);
    }
}
