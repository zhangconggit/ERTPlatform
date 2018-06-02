using UnityEngine;
using System.Collections;
using CFramework;
using Assets.Scripts.UI.View;
using System.Collections.Generic;

public class HitPichu : StepBase
{
    Transform transform;
    Transform JumazhenObject;
    Vector3 PuncturePoint;
    Vector3 PunctureNormal = Vector3.zero;

    SkinnedMeshRenderer zhenguan;
    SkinnedMeshRenderer zhenye;

    int deflection_Y_Number;
    List<string> record;
    int stepIndex = 0;

    UPuncture puncture;

    string state = string.Empty;
    bool isBack;//是否回抽

    public HitPichu()
    {
        switch (CGlobal.productName)
        {
            case "xqcc":
                cameraEnterPosition = new Vector3(-0.22f, 0.82f, 0.37f);
                cameraEnterEuler = new Vector3(30.2087f, 139.0721f, 349.0117f);
                break;
            case "yzcc":
                cameraEnterPosition = new Vector3(0.408f, 0.765f, -1.671f);
                cameraEnterEuler = new Vector3(0f, 249f, 0f);
                break;
            default:
                break;
        }
     


        UPageButton okButton = CreateUI<UPageButton>();
        okButton.name = "okButton";
        okButton.SetAnchored(AnchoredPosition.bottom);
        okButton.rect = new Rect(0, -100, 180, 60);
        okButton.LoadSprite("keys_030");
        okButton.LoadPressSprite("keys_031");
        okButton.text = "确定";
        okButton.onClick.AddListener(OnClickOkButton);
    }

    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();

        record = new List<string>();
        puncture = new UPuncture(CreateUI<UImage>);

        JumazhenObject = GameObject.Find("Models").transform.Find("tools/jumazhenParent");
        JumazhenObject.gameObject.SetActive(true);
        JumazhenObject.localPosition = new Vector3(0.1120344f, 0.7583759f, -1.658137f);
        JumazhenObject.localEulerAngles = new Vector3(357.7632f, 60.93891f, 358.6839f);

        transform = GameObject.Find("Models").transform.Find("tools/jumazhenParent/jumazhen");
        zhenye = GameObject.Find("Models").transform.Find("tools/jumazhenParent/jumazhen/Cylinder001").GetComponent<SkinnedMeshRenderer>();
        zhenguan = GameObject.Find("Models").transform.Find("tools/jumazhenParent/jumazhen/injector").GetComponent<SkinnedMeshRenderer>();
        zhenguan.SetBlendShapeWeight(0, 100);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.UpArrow, new Vector2(-500, 300), "上转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.UpArrow).onClick.AddListener(DeflectionUp);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.DownArrow, new Vector2(-500, 160), "下转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.DownArrow).onClick.AddListener(DeflectionDown);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.LeftArrow, new Vector2(-600, 230), "左转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.LeftArrow).onClick.AddListener(DeflectionLeft);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.RightArrow, new Vector2(-400, 230), "右转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.RightArrow).onClick.AddListener(DeflectionRight);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.Q, new Vector2(-600, 100), "穿刺");
        ButtonVirtualScript.Instance.GetButton(KeyCode.Q).onClick.AddListener(SyringeForward);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.W, new Vector2(-400, 100), "退出");
        ButtonVirtualScript.Instance.GetButton(KeyCode.W).onClick.AddListener(SyringeBackOff);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.E, new Vector2(-600, 0), "注射");
        ButtonVirtualScript.Instance.GetButton(KeyCode.E).onClick.AddListener(SyringeInjection);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.R, new Vector2(-400, 0), "回退");
        ButtonVirtualScript.Instance.GetButton(KeyCode.R).onClick.AddListener(SyringeBack);

        transform.gameObject.SetActive(true);
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "adjustment_angle");

        On3DModelListen += SetPuncturePosition;

        Model_PunctureInfo.Instance.m_PuncturePoint = new Vector3(322f,120f,0);
        Ray ray = Camera.main.ScreenPointToRay(Model_PunctureInfo.Instance.m_PuncturePoint);
        
        //射线的起点
        RaycastHit hit;//射线的终点
        if (Physics.Raycast(ray, out hit))
        {
            On3DModelListen.Invoke(hit);
        }
        //Init();
    }

    //3D模型点击事件委托
    public delegate void OnClick3DModelDelegate(RaycastHit obj);
    OnClick3DModelDelegate On3DModelListen;

    //public override void MouseClickModel(RaycastHit obj)
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
    //    RaycastHit hit;//射线的终点
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        On3DModelListen.Invoke(hit);
    //        Debug.Log(Input.mousePosition.x+"  "+ Input.mousePosition.y + " "+ Input.mousePosition.z);
    //    }
    //}


    public void Init()
    {
        PuncturePoint = Model_PunctureInfo.Instance.m_PuncturePoint;
        PunctureNormal = Model_PunctureInfo.Instance.m_PunctureNormal;

        JumazhenObject.transform.position = PuncturePoint + 0.01f * PunctureNormal;
        ModelCtrol.Instance.setModelsOnNormalline(JumazhenObject.gameObject, Model_PunctureInfo.Instance.m_PunctureNormal, new Vector3(0, 0, 1), 0);
    }

    
    public void SetPuncturePosition(RaycastHit info)
    {
        PuncturePoint = info.point;
        PunctureNormal = info.normal;

        JumazhenObject.transform.position = PuncturePoint + 0.01f * PunctureNormal;
        ModelCtrol.Instance.setModelsOnNormalline(JumazhenObject.gameObject, info.normal, new Vector3(0, 0, 1), 0);
    }

    /// <summary>
    /// 针前进
    /// </summary>
    public void SyringeForward()
    {
        if(state == "打皮丘完成")
        {
            if (VoiceControlScript.Instance.IsVoicePlaying() == false)
            {
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "adjustment_angle2");
            }
            return;
        }
        if (puncture.GetDept() < 200)
        {
            SetPunctureMove(-0.003f);
            puncture.SyringeForward(true);
        }

        switch (puncture.GetXQCCLocation())
        {
            case "皮下及脂肪":
                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Pi");
                }
                if (string.IsNullOrEmpty(state))
                {
                    if (puncture.GetAngle() <= -85)  //针尖进针角度判断
                        AddScoreItem("10016130");
                    else
                        AddScoreItem("10016131");

                    if (puncture.GetAngle() < 0)  //针尖斜面判断
                        AddScoreItem("10016140");
                    else
                        AddScoreItem("10016141");
                }
                else if (state == "进行麻醉")
                {
                    if (puncture.GetAngle() >= 85 && puncture.GetAngle() <= 95)
                    {
                        AddScoreItem("10016160");
                    }
                    else
                    {
                        AddScoreItem("10016161");
                    }
                }
                break;
            case "背阔肌":
                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Beikouji");
                }
                break;
            case "肋间内肌肉":
                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Neiji");
                }
                break;
            case "胸腔":
                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "Poushi_Xiongqiang");
                }
                break;
            default:
                break;
        }

        isBack = false;
        
    }

    /// <summary>
    /// 针后退
    /// </summary>
    public void SyringeBackOff()
    {
        if (puncture.GetDept()>0)
        {
            SetPunctureMove(0.003f);
            puncture.SyringeForward(false);
        }
        if (puncture.GetXQCCLocation() == "体外" && state == "打皮丘完成")
        {
            state = "进行麻醉";
        }
        isBack = false;
    }

    /// <summary>
    /// 打麻药
    /// </summary>
    public void SyringeInjection()
    {
        if(puncture.GetXQCCLocation() == "体外")
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "injection_error");
            return;
        }
        if (zhenye.GetBlendShapeWeight(0) + 5 > 100)
            return;
        else
            zhenye.SetBlendShapeWeight(0, zhenye.GetBlendShapeWeight(0) + 5);

        if (zhenguan.GetBlendShapeWeight(0) - 5 < 0)
            return;
        else
            zhenguan.SetBlendShapeWeight(0, zhenguan.GetBlendShapeWeight(0) - 5);

        if (string.IsNullOrEmpty(state))
        {
            if(puncture.GetXQCCLocation() == "皮下及脂肪")
            {
                AddScoreItem("10016150");
            }
            else
            {
                AddScoreItem("10016152");
            }

            state = "打皮丘完成";
        }
        if (state == "进行麻醉")
        {
            if (puncture.GetXQCCLocation() == "皮下及脂肪")
            {
                AddScoreItem("10016180");
                if(isBack)
                    AddScoreItem("10016170");
            }
            if (puncture.GetXQCCLocation() == "背阔肌" || puncture.GetXQCCLocation() == "肋间内肌肉")
            {
                AddScoreItem("10016120");
                if (isBack)
                    AddScoreItem("10016190");
            }
            if (puncture.GetXQCCLocation() == "胸腔")
            {
                AddScoreItem("10016220");
                if (isBack)
                    AddScoreItem("10016210");
            }
        }

        puncture.InjectionMapping();
    }

    /// <summary>
    /// 回抽
    /// </summary>
    public void SyringeBack()
    {
        if (zhenye.GetBlendShapeWeight(0) - 5 < 0)
            return;
        else
            zhenye.SetBlendShapeWeight(0, zhenye.GetBlendShapeWeight(0) - 5);

        if (zhenguan.GetBlendShapeWeight(0) + 5 > 100)
            return;
        else
            zhenguan.SetBlendShapeWeight(0, zhenguan.GetBlendShapeWeight(0) + 5);

        isBack = true;
    }


    public void DeflectionUp()
    {
        if(puncture.GetXQCCLocation()!="体外")//针尖插入皮肤，不能旋转角度
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "prohibit_rotate");
            return;
        }
        if (puncture.GetAngle() < 90)
        {
            SetPunctureRoate(new Vector3(-4.5f, 0, 0));
            puncture.DeflectionUp(true);
        }
    }

    public void DeflectionDown()
    {
        if (puncture.GetXQCCLocation() != "体外")//针尖插入皮肤，不能旋转角度
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "prohibit_rotate");
            return;
        }
        if (puncture.GetAngle()> -90)
        {
            SetPunctureRoate(new Vector3(4.5f, 0, 0));
            puncture.DeflectionUp(false);
        }
    }

    public void DeflectionLeft()
    {
        if (puncture.GetXQCCLocation() != "体外")//针尖插入皮肤，不能旋转角度
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "prohibit_rotate");
            return;
        }
        if (deflection_Y_Number < 15)
        {
            deflection_Y_Number++;
            SetPunctureRoate(new Vector3(0, 4.5f, 0));
        }
    }

    public void DeflectionRight()
    {
        if (puncture.GetXQCCLocation() != "体外")//针尖插入皮肤，不能旋转角度
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "prohibit_rotate");
            return;
        }
        if (deflection_Y_Number > -15)
        {
            deflection_Y_Number--;
            SetPunctureRoate(new Vector3(0, -4.5f, 0));
        }
    }

    public void SetPunctureRoate(Vector3 v3)
    {
        //JumazhenObject.transform.Rotate(v3);
        JumazhenObject.transform.Rotate(v3);
        //JumazhenObject.transform.RotateAround(PuncturePoint, Vector3.left , v3.x * 3);
        //JumazhenObject.transform.RotateAround(PuncturePoint, Vector3.up , v3.y * 3);
    }

    /// <summary>
    /// 移动麻醉针模型
    /// </summary>
    /// <param name="f"></param>
    public void SetPunctureMove(float f)
    {
        transform.localPosition += new Vector3(0,0,f);
    }

    void OnClickOkButton()
    {

        JumazhenObject.gameObject.SetActive(false);

        string[] codes = new string[] { "10016170", "10016180", "10016190", "10016200", "10016210", "10016220" };
        foreach (var code in codes)
        {
            if(!IsExistCode(code))
            {
                AddScoreItem(code.Substring(0, code.Length-1)+"1 ");
            }
        }
        //最后
        State = StepStatus.did;
        ButtonVirtualScript.Instance.RemoveVirtualButton();
        ButtonVirtualScript.Instance.ClearDicButton();

    }
}
