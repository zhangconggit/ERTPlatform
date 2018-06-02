using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;

public class ChooseGoods : StepBase
{
    /// <summary>
    /// 选择穿刺包评分
    ///             =0没有选择穿刺包
    ///             =2选择正确穿刺包
    ///             =-2选择错误穿刺包
    /// </summary>
    int xuanzechuancibao = 0;
    /// <summary>
    /// 选择普通物品得分
    ///            =0没有选择物品
    ///            =2选择错误物品在1-3个内
    ///            =2选择正确物品在1-3个内
    ///            =1少选三个物品以上
    ///            =1多选三个物品以上
    ///            =5全部选对
    /// </summary>
    int xuanzewupin = 0;
    GameObject yizhubook = null;
    UChooseItems uiStep;
    UText chooseNumber;
    public ChooseGoods()
    {
        uiStep = CreateUI<UChooseItems>();
        yizhubook = GameObject.Find("Models").transform.Find("guanchang_bingli_B").gameObject;
        yizhubook.SetActive(false);
        uiStep.OnClickFinishButton.AddListener(FinishCheck);
        string[] choose = new string[] { "碘伏", "记号笔", "试管", "胸腔穿刺针", "5毫升注射器", "50毫升注射器", "洞巾", "医用纱布", "医生工作服", "利多卡因", "止血钳", "棉签", "无菌手套", "口罩", "帽子", "镊子+棉球", "测压管", "葡萄糖", "腰穿针" };//需要选择物品
        string goods = "Models/chooseItem/qi_xie_desk";
        bool[] result = new bool[] { true,true,true,true, true, true, true, true, true, true, true, true, true, true, true, true,false,false,false };//判断物品选择是否正确
        uiStep.baseItems.Init(goods);
        uiStep.baseItems.SetSelectItem(choose, result);
        chooseNumber = CreateUI <UText>(AnchoredPosition.bottom);
        chooseNumber.rect = new UnityEngine.Rect(0, -85, 260, 60);
        UPageButton button1 = CreateUI<UPageButton>();
        button1.rect = new Rect(800, 400, 180, 70);
        button1.text = "确定";
        button1.LoadSprite("anniu-160");
        button1.LoadPressSprite("anniu-160h");
        button1.onClick.AddListener(uiStep.baseItems.ShowTrueGoods);
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
        base.CameraMoveFinished();
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, ("chooseneedgoods"), true);
        //uiStep.FinishButton.gameObejct.SetActive(true);
        
        
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
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "chooseerrorgoods", true);
        }
#if UNITY_EDITOR
        State = StepStatus.did;
#endif 
    }
    public void OnSelect()
    {
        chooseNumber.text = "选择物品：" + uiStep.baseItems.GetSelectedNumber() + " / " + uiStep.baseItems.GetNeedSelectNumber() + " 个";
    }


}

