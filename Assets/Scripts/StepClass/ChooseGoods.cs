using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;

public class ChooseGoods : StepBase
{
    UChooseItems uiStep;
    UText chooseNumber;
    public ChooseGoods()
    {
        uiStep = CreateUI< UChooseItems>();
        uiStep.OnClickFinishButton.AddListener(FinishCheck);
        string[] choose = new string[] { "碘伏", "记号笔", "试管", "胸腔穿刺针", "5毫升注射器", "50毫升注射器", "洞巾", "医用纱布", "医生工作服", "利多卡因", "止血钳", "棉签", "无菌手套", "口罩", "帽子", "镊子+棉球", "测压管", "葡萄糖", "腰穿针" };//需要选择物品
        string goods = "Models/chooseItem/qi_xie_desk";
        bool[] result = new bool[] { true,true,true,true, true, true, true, true, true, true, true, true, true, true, true, true,false,false,false };//判断物品选择是否正确
        uiStep.baseItems.Init(goods);
        uiStep.baseItems.SetSelectItem(choose, result);
    }
    public override void EnterStep(float cameraTime = 0)
    {

        cameraEnterPosition =new Vector3(1.275f, 1.312f, 2.446f) ;// uiStep.baseItems.cameraPath;
        cameraEnterEuler =new Vector3(90f,-90f,-90f); //uiStep.baseItems.cameraAngle;
        uiStep.FinishButton.gameObejct.SetActive(false);
        base.EnterStep(cameraTime);
    }
    public override void CameraMoveFinished()
    {
        uiStep.FinishButton.gameObejct.SetActive(true);
        chooseNumber = new UText(AnchoredPosition.bottom);
        chooseNumber.rect = new UnityEngine.Rect(0, -100, 260, 60);
        chooseNumber.text = "选择物品：0 / " + uiStep.baseItems.GetNeedSelectNumber() + " 个";

        uiStep.baseItems.StartChooseItems();
        uiStep.baseItems.OnItemStateChange += OnSelect;
    }

    //public override void ExitStep()
    //{
    //    uiStep.baseItems.OnItemStateChange -= OnSelect;
    //    chooseNumber.Destroy();

    //    uiStep.baseItems.EndChooseItems();
    //    uiStep.gameObejct.SetActive(false);

    //    StepConfigMain.mInstance.InitCameraPositon();
    //}

    //public override void SetModelExitStatus()
    //{

    //}

    //public override void SetModelStartStatus()
    //{

    //}
    /// <summary>
    /// 点击完成按钮时
    /// </summary>
    public void FinishCheck()
    {
        int moreNumber = uiStep.baseItems.GetMoreSelectNumber();
        int lostNumber = uiStep.baseItems.GetLostSelectNumber();
        if (moreNumber == 0 && lostNumber == 0)
        {
            //stepCallBackFun.Invoke();
            //ExitStep();
        }
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "nf_errorSelectGood", true);
        }
    }
    public void OnSelect()
    {
        chooseNumber.text = "选择物品：" + uiStep.baseItems.GetSelectedNumber() + " / " + uiStep.baseItems.GetNeedSelectNumber() + " 个";
    }


}

