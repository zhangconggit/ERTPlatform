using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFramework;

public class CCDisinfection : StepBase
{
    //标记点
    Vector2 markPoint = Vector2.zero;

    //消毒模型目标点
    GameObject man;

    //消毒模型目标点
    GameObject target;

    //镊子
    Model_Tweezer tweezer;


    //消毒
    public MisTexturePoint.CTexturePoint texturePoint;

    bool isNoLeaveWhite = false;//是否留白
    bool isInRegion = false;//是否在区域内
    bool isFromInToOut = false;//是否从外到里
    bool isSameDirection = true;//是否同方向-不用了

    int forwardDirection = 0;//上一方向


    Color mainColorExit = new Color(1, 1, 1);
    Color mainColorEnter = new Color(143 / 255.0f, 143 / 255.0f, 143 / 255.0f);

    //消毒层次
    int index = 0;

    //不同层次消毒的颜色
    Color[] disinfectionColor = new Color[] { new Color(1, 228 / 255.0f, 159 / 255.0f, 1), new Color(1, 202 / 255.0f, 66 / 255.0f, 1), new Color(1, 188 / 255.0f, 13 / 255.0f, 1) };

    public CCDisinfection()
    {
        if (CGlobal.productName == "xqcc")
        {
            cameraEnterPosition = new Vector3(-0.32f, 0.75f, 0.34f);
            cameraEnterEuler = new Vector3(11.6962f, 147.2081f, 354.9553f);
            man = ModelCtrol.Find("chest_body-new/chest_body");
        }
        else if (CGlobal.productName == "yzcc")
        {
            cameraEnterPosition = new Vector3(0.345f, 0.765f, -1.643f);
            cameraEnterEuler = new Vector3(0, -105.0465f);
            man = ModelCtrol.Find("lumbar_body-new/lumbar_adult_body");
        }
        else if (CGlobal.productName == "fqcc")
        { 
            cameraEnterPosition = new Vector3(-0.517f, 1.015f, -1.965f);
            cameraEnterEuler = new Vector3(62.4794f, 110.2238f, 18.0929f);
            man = ModelCtrol.Find("fuchuanPeople/abdo_body");
        }
        else if (CGlobal.productName == "gscc")
        {
            cameraEnterPosition = new Vector3(-0.632f, 0.87f, -2.03f);
            cameraEnterEuler = new Vector3(30.70366f, 91.88552f, 9.891573f);
            man = ModelCtrol.Find("fuchuanPeople/abdo_body");
        }
        

        //target = man.transform.Find("pos").gameObject;
        tweezer = Model_Tweezer.Instance;
        texturePoint = man.GetComponent<MisTexturePoint.CTexturePoint>();
        texturePoint.material = man.transform.GetComponent<MeshRenderer>().materials[1];

        tweezer.EnabledTweezel(false);
        texturePoint.enabled = false;

        UPageButton mianqian = CreateUI<UPageButton>();
        mianqian.SetAnchored(AnchoredPosition.right);
        mianqian.rect = new Rect(-150, 0, 200, 200);
        mianqian.LoadSprite("cotton_ball");
        mianqian.LoadPressSprite("cotton_ball");

    }

    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);//调用此接口方能计数
        Debug.Log("进入步骤：" + GetType().Name);
        string ste =fileHelper.ReadIni(GetType().Name, "test", "StepConfig");
        Debug.Log("test=" + ste);
    }

    public override void StepFinish()
    {
        base.StepFinish();
        Debug.Log("退出步骤：" + GetType().Name);
        //texturePoint.resetMaterial(markPoint, new Color(140 / 255.0f, 28 / 255.0f, 146 / 255.0f), 100, 3);
    }
    public override void MouseClickModel(RaycastHit obj)
    {
        base.MouseClickModel(obj);
        //Debug.Log(obj.point);
        Model_Tweezer.Instance.SetTweezerPos(obj.point, obj.normal);
        ////Vector3 moDir = new Vector3(0, 0, 0);
        //ModelCtrol.Instance.setModelsOnNormalline(Model_Tweezer.Instance.modelObject, obj.normal, Model_Tweezer.Instance.GetNormorl(), 0);
        //Model_Tweezer.Instance.modelObject.transform.position = obj.point + obj.normal* Model_Tweezer.Instance.GetLength();
        ////Model_Tweezer.Instance.SetTweezerPos();
    }
    //设置开始状态
    public override void StepStartState()
    {
        base.StepStartState();
        //AnimPlay.mInstance.PlayWearGolve(false);
        tweezer.EnabledTweezel(false);
        texturePoint.clearMark();
        texturePoint.cancleDraw();
        texturePoint.setMainTextureColor(mainColorExit);
        texturePoint.resetMaterial();
        resetDraw();
        texturePoint.enabled = false;
        ModelCtrol.Instance.EnabledCameraRotate(false, target);
    }

    //设置退出状态
    public override void StepEndState()
    {
        base.StepEndState();
        //AnimPlay.mInstance.PlayWearGolve(false);
        tweezer.EnabledTweezel(false);
        texturePoint.enabled = true;
        //texturePoint.resetMaterial();
        markPoint = CGlobal.PunctureTexturePoint;//new Vector2(741, 393);
        texturePoint.resetMaterial(markPoint, texturePoint.areaColor, 100, 3);

        texturePoint.enabled = false;
       // OnClickModel.Instance.SkinLucency(false);

        if (texturePoint.isExistAlbedo2())
        {
            // Debug.Log("带消毒");
            texturePoint.setMainTextureColor(mainColorEnter);
        }
        ModelCtrol.Instance.EnabledCameraRotate(false, target);
    }

    //进入步骤相机移动结束
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        Model_Tweezer.Instance.modelObject.SetActive(true);
        texturePoint.setRenderParam(disinfectionColor[0], 20);
        texturePoint.SetCallBack(leaveWhite, clock, radius, radiusDistance);
        index = 0;
        texturePoint.startCheckLeaveWhite(disinfectionColor[index], 0.01f);
        if (texturePoint.isExistAlbedo2())
        {
            texturePoint.setMainTextureColor(mainColorEnter);
        }
        texturePoint.enabled = true;

        markPoint = CGlobal.PunctureTexturePoint;//new Vector2(741, 393);
        texturePoint.correctionMarkPoint(markPoint, texturePoint.areaColor, 100, 3);//130
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "select_nie_zi", true);
        AddScoreItem("2302449");
    }

    public override void StepUpdate()
    {
        base.StepUpdate();
        if(Input.GetMouseButton(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;//射线的终点
            if (Physics.Raycast(_ray, out hit))
            {
                if(hit.collider.name=="")
                {

                }
            }
        }
        

        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    Model_Tweezer.Instance.Insert(0.02f);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Model_Tweezer.Instance.Insert(-0.02f);
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    Model_Tweezer.Instance.RotateY(2f);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    Model_Tweezer.Instance.RotateY(-2f);
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    Model_Tweezer.Instance.RotateX(2f);
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    Model_Tweezer.Instance.RotateX(-2f);
        //}
    }
    //镊子棉球
    public void niezi(bool isClick)
    {
        tweezer.EnabledTweezel(isClick);
        texturePoint.enabled = isClick;
        if (isClick == true)
        {
            texturePoint.EnableCheckThread();
            if (texturePoint.isExistAlbedo2())
            {
                texturePoint.setMainTextureColor(mainColorEnter);
            }
            if (index < 3)
                setColor(disinfectionColor[index]);
            if (index == 0)
            {
                AddScoreItem("2302300");
                texturePoint.resetDrawContent();
                texturePoint.setAreaByPoints(markPoint, 130);//150
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "start_sterilize", true);
            }
            else if (index == 1)
            {
                AddScoreItem("2302340");
                texturePoint.setAreaByPoints(markPoint, 130 * 10 / 15);//150
            }
            else if (index == 2)
            {
                AddScoreItem("2302380");
                texturePoint.setAreaByPoints(markPoint, 130 * 8 / 15);//150
            }
        }
        else
        {
            texturePoint.EnableCheckThread(false);//closeLeaveWhite();
        }
    }
    //撤销步骤
    public void resetStep(bool isClick)
    {
        texturePoint.resetDrawContent();
        tweezer.resetLocalModelTran();
        index = 0;
    }

    //视角控制
    public void ViewCtl(bool isClick)
    {
        texturePoint.enabled = false;
        niezi(false);//ModelCtrol.Instance.EnabledTweezers(tweezer,false);
        ModelCtrol.Instance.EnabledCameraRotate(isClick, target);
    }

    //消毒错误 撤销
    public void resetDraw()
    {
        index = 0;
        texturePoint.resetDrawContentWithoutBoard(markPoint, 130);
        texturePoint.clearCache();
        clearStatus();
        // setColor(disinfectionColor[0]);
    }

    //清理检测状态
    void clearStatus()
    {
        isNoLeaveWhite = false;//是否留白
        isInRegion = false;//是否在区域内
        isFromInToOut = false;//是否从外到里
        isSameDirection = true;//是否顺时针
        forwardDirection = 0;//上一方向
    }

    //设置消毒颜色
    public void setColor(Color pColor)
    {
        //pColor = Color.red;
        //disinfectionColor = pColor;
        texturePoint.MColor = pColor;
        texturePoint.mRadius = 30;
    }


    //留白检测回调
    void leaveWhite(bool re)
    {
        if (index == 0)
        {
            if (!IsExistCode("2302320") && !IsExistCode("2302321") && !IsExistCode("2302322"))
            {
                float dis = texturePoint.getStartDistance() * 15 / 130;
                Debug.Log("Dis=" + dis);
                if (dis <= 1)
                {
                    AddScoreItem("2302320");
                }
                else if (dis > 1 && dis <= 3)
                {
                    AddScoreItem("2302321");
                }
                else
                {
                    AddScoreItem("2302322");
                }
            }
        }
        else if (index == 1)
        {
            if (!IsExistCode("2302360") && !IsExistCode("2302361") && !IsExistCode("2302362"))
            {
                float dis = texturePoint.getStartDistance() * 15 / 130;
                if (dis <= 1)
                {
                    AddScoreItem("2302360");
                }
                else if (dis > 1 && dis <= 3)
                {
                    AddScoreItem("2302361");
                }
                else
                {
                    AddScoreItem("2302362");
                }
            }
        }
        else if (index == 2)
        {
            if (!IsExistCode("2302390") && !IsExistCode("2302391") && !IsExistCode("2302392"))
            {
                float dis = texturePoint.getStartDistance() * 15 / 130;
                if (dis <= 1)
                {
                    AddScoreItem("2302390");
                }
                else if (dis > 1 && dis <= 3)
                {
                    AddScoreItem("2302391");
                }
                else
                {
                    AddScoreItem("2302392");
                }
            }
        }
        if (re)
        {
            isNoLeaveWhite = false;
            //Debug.Log("语音：消毒区域有留白");
            if (IsExistCode("2302430"))
                RemoveScoreItem("2302430");
            AddScoreItem("2302431");
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeBlank", true);
            //texturePoint.writeLeaveNote();
            resetDraw();
            texturePoint.enabled = false;

        }
        else
        {
            //Debug.Log("无留白");
            isNoLeaveWhite = true;
            if (isInRegion && isFromInToOut && isSameDirection)
            {
                Debug.Log("首次消毒完成");
                exchangeLevel();
            }
        }

    }

    //范围检测回调
    void radius(bool re)
    {
        if (re)
        {
            isInRegion = true;
            // Debug.Log("范围内");
            if (isNoLeaveWhite && isFromInToOut && isSameDirection)
            {
                Debug.Log("首次消毒完成");
                exchangeLevel();
            }
        }
        else
        {
            isInRegion = false;
            //Debug.Log("范围过大");
            if (index == 0)
            {
                if (!IsExistCode("2302330"))
                {
                    AddScoreItem("2302333");
                }

            }
            else if (index == 1)
            {
                if (!IsExistCode("2302370"))
                {
                    AddScoreItem("2302373");
                }
            }
            else if (index == 2)
            {
                if (!IsExistCode("2302410"))
                {
                    AddScoreItem("2302413");
                }
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeRadiusBigError", true);
            resetDraw();
        }
    }

    //方向检测回调
    void radiusDistance(bool re)
    {
        if (re)
        {
            isFromInToOut = true;
            // Debug.Log("从里向外");
            if (isNoLeaveWhite && isInRegion && isSameDirection)
            {
                Debug.Log("首次消毒完成");
                exchangeLevel();
            }
        }
        else
        {
            isFromInToOut = false;
            //if (CGlobal.projectName != ProjectName.腹腔穿刺)
            //{
            //    if (IsExistCode("2302420"))
            //        RemoveScoreItem("2302420");
            //    AddScoreItem("2302421");
            //}
            //Debug.Log("从外向里");

            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeDirectionError", true);
            niezi(false);
            resetDraw();
        }
    }

    //顺时针逆时针检测回调-暂不用
    void clock(bool re)//
    {
        // if (re)
        // {
        //     if (forwardDirection == -1)
        //     {
        //         isSameDirection = false;
        //         Debug.Log("方向不一致");
        //         forwardDirection = 0;
        //         if(IsExistCode("2302420"))
        //             RemoveScoreItem("2302420");
        //         AddScoreItem("2302421",0);
        //         VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions,"SterilizeDirectionError",true);
        //         resetDraw();
        //     }
        //     else if (forwardDirection == 1)
        //     {
        //         isSameDirection = true;
        //         if (isNoLeaveWhite && isInRegion && isFromInToOut)
        //         {
        //             Debug.Log("首次消毒完成");
        //             exchangeLevel();
        //         }
        //     }
        //     forwardDirection = 1;
        //     Debug.Log("顺时针");
        // }

        // else
        // {
        //     if (forwardDirection == 1)
        //     {
        //         Debug.Log("方向不一致");
        //         forwardDirection = 0;
        //         if(IsExistCode("2302420"))
        //             RemoveScoreItem("2302420");
        //         AddScoreItem("2302421",0);
        //         VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions,"SterilizeDirectionError",true);
        //         isSameDirection = false;
        //         resetDraw();
        //     }
        //     else if (forwardDirection == -1)
        //     {
        //         isSameDirection = true;
        //         if (isNoLeaveWhite && isInRegion && isFromInToOut)
        //         {
        //             Debug.Log("首次消毒完成");
        //             exchangeLevel();
        //         }
        //     }
        //     forwardDirection = -1;
        //     Debug.Log("逆时针");

        // }
    }


    //消毒成功一次        
    void exchangeLevel()
    {
        texturePoint.clearCache();
        clearStatus();
        index++;
        // if (index < 3)
        //     setColor(disinfectionColor[index]);
        if (index == 1)
        {
            RemoveScoreItem("2302449");
            AddScoreItem("2302441");
            if (!IsExistCode("2302333"))
            {
                AddScoreItem("2302330");
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeSecond", true);
        }
        else if (index == 2)
        {
            RemoveScoreItem("2302441");
            AddScoreItem("2302440");
            if (!IsExistCode("2302373"))
            {
                AddScoreItem("2302370");
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeThird", true);
        }
        else if (index == 3)
        {
            if (!IsExistCode("2302413"))
            {
                AddScoreItem("2302410");
            }
            //index = 0;
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizeFinished", true);
           State  = StepStatus.did;
        }
        else if (index > 3)
        {
            RemoveScoreItem("2302440");
            AddScoreItem("2302442");
        }
        //if (CGlobal.projectName != ProjectName.腹腔穿刺)
        //{
        //    if (!IsExistCode("2302421"))
        //        AddScoreItem("2302420", 5);
        //}


        if (!IsExistCode("2302431"))
        {
            AddScoreItem("2302430");
        }
        texturePoint.enabled = false;
    }

}
