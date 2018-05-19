
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

        pathList.Add("keys_019");
        pathList.Add("keys_020");
        pathList.Add("keys_021");
        pathList.Add("keys_022");
        ui.SetButtomImage(pathList);
        //ui.OnOkbutton.AddListener(OnClickOkButton);
    }
    void OnClickOkButton()
    {
        State = StepStatus.did;
    }
}
