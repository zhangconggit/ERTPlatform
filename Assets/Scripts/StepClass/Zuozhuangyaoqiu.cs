
using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 着装要求
/// </summary>
class Zuozhuangyaoqiu : StepBase
{
    SelectImage ui = null;
    public Zuozhuangyaoqiu()
    {
        cameraEnterPosition = new Vector3(0.21f, 1.014f, 1.11f);
        cameraEnterEuler = new Vector3(15.4009f, -182.6279f, 0);
        ui = CreateUI<SelectImage>();
        List<string> pathList = new List<string>();
        List<string> alfterList = new List<string>();

        pathList.Add("hushi1");
        pathList.Add("hushi2");
        pathList.Add("hushi3");
        pathList.Add("hushi4");
        alfterList.Add("hushi1_h");
        alfterList.Add("hushi2_h");
        alfterList.Add("hushi3_h");
        alfterList.Add("hushi4_h");

        ui.SetButtomImage(pathList, alfterList);
        ui.OnOkbutton.AddListener(OnClickOkButton);

    }
    void OnClickOkButton(string name)
    {
        if (name == "hushi1")
        {
            AddScoreItem("10010100");
            State = StepStatus.did;
        }
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "chooseError", true);
            AddScoreItem("10010101");
        }
        IsAllowAddCode(true);
    }
}
