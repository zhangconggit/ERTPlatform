using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFramework;

public class VerifyNarcotic : StepBase
{
    SelectImage ui = null;
    bool isScore = false;
    public VerifyNarcotic()
    {
        switch (CGlobal.productName)
        {
            case "xqcc":
                cameraEnterPosition = new Vector3(0.21f, 1.014f, 1.11f);
                cameraEnterEuler = new Vector3(15.4009f, -182.6279f, 0);
                break;
            case "yzcc":
                cameraEnterPosition = new Vector3(-0.872f, 1.188f, 0.042f);
                cameraEnterEuler = new Vector3(0.8371f, 238.1956f, -0.147f);
                break;
            default:
                break;
        }
      
        ui = CreateUI<SelectImage>();
      
        List<string> pathListDefault = new List<string>();
        pathListDefault.Add("check_anesthetic_1");
        pathListDefault.Add("check_anesthetic_2");
        pathListDefault.Add("check_anesthetic_3");
        pathListDefault.Add("check_anesthetic_4_ok");

        List<string> pathList = new List<string>();
        pathList.Add("check_anesthetic_1_h");
        pathList.Add("check_anesthetic_2_h");
        pathList.Add("check_anesthetic_3_h");
        pathList.Add("check_anesthetic_4_ok_h");
        ui.SetButton(pathListDefault, pathList);
        ui.OnOkbutton.AddListener(OnClickOkButton);
    }

    public override void StepStartState()
    {
        base.StepEndState();
    }

    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "start_check_anesthetic");
    }

    void OnClickOkButton(string buttonName)
    {
        if (buttonName.Contains("ok"))
        {
            if (!isScore)
            {
                isScore = true;
                AddScoreItem("10016100");
            }
            State = StepStatus.did;
        }
        else
        {
            if (!isScore)
            {
                isScore = true;
                AddScoreItem("10016102");
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_anesthetic_wrong");
        }
    }
}
