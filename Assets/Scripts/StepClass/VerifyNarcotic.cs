using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFramework;

public class VerifyNarcotic : StepBase
{
    SelectImage ui = null;
    public VerifyNarcotic()
    {
        cameraEnterPosition = new Vector3(0.21f, 1.014f, 1.11f);
        cameraEnterEuler = new Vector3(15.4009f, -182.6279f, 0);
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
    void OnClickOkButton(string buttonName)
    {
        if(buttonName.Contains("ok"))
            State = StepStatus.did;
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "");
        }
    }
}
