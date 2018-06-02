using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bazhen : StepBase
{


    /// <summary>
    /// 抽液中的胸穿刺针
    /// </summary>
    GameObject MyNeedle;
    int temp = 0;

    public bazhen()
    {
        ////设置摄像机的位置
        //cameraEnterPosition = new Vector3(-0.498f, 0.834f, 0.341f);
        ////设置摄像机的旋转角度
        //cameraEnterEuler = new Vector3(17.134f, 102.742f, 366.473f);

        //设置摄像机的位置
        cameraEnterPosition = new Vector3(-0.364f, 0.769f, 0.172f);
        //设置摄像机的旋转角度
        cameraEnterEuler = new Vector3(46.78023f, 72.23509f, 4.46308f);
        //得到自己的胸穿刺针的对象
        MyNeedle = GameObject.Find("Models").transform.Find("tools/胸穿针").gameObject;

        //在呼气屏住呼吸时拔出穿刺针-默认给分
        AddScoreItem("10018190");
    }

    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);

        //显示穿持针
        MyNeedle.SetActive(true);
        //隐藏注射器
        GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi").gameObject.SetActive(false);
        //隐藏止血钳
        GameObject.Find("Models").transform.Find("tools/胸穿针/助手扶针").gameObject.SetActive(false);

        GameObject.Find("Models").transform.Find("tools/胸穿针/Tube_clip_open").gameObject.SetActive(false);
        GameObject.Find("Models").transform.Find("tools/胸穿针/Tube_clip_cl").gameObject.SetActive(true);
    }

    /// <summary>
    /// 镜头移动结束时
    /// </summary>
    public override void CameraMoveFinished()
    {

        //设置按钮
        SettingBtn(new Dictionary<string, KeyCode>
        {
            { "拔针", KeyCode.Alpha3 }
        });

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    /// <param name="lstBtnSetting">按钮设定</param>
    private void SettingBtn(Dictionary<string, KeyCode> lstBtnSetting)
    {
        // 分辨率：1920 x 1080
        //图片初期位置和大小
        Vector2 baseRec = new Vector2(-500, 500);
        int iCnt = 0;
        int firstRow = 3;
        int scendRow = 3;
        float btnWidth = 80;
        float btnHeight = 80;
        foreach (KeyValuePair<string, KeyCode> keyValue in lstBtnSetting)
        {
            Vector2 newRec;
            if (iCnt < firstRow)
            {
                newRec = new Vector2(baseRec.x + iCnt * btnWidth, baseRec.y);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            else if (iCnt < (firstRow + scendRow) && iCnt >= firstRow)
            {
                newRec = new Vector2(baseRec.x + (iCnt % firstRow) * btnWidth, baseRec.y - btnHeight * 1.5f);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            else
            {
                newRec = new Vector2(baseRec.x + (iCnt % (firstRow + scendRow)) * btnWidth, baseRec.y - btnHeight * 3);
                ButtonVirtualScript.Instance.VirtualButton(keyValue.Value, newRec, keyValue.Key);
            }
            KeyCode key = keyValue.Value;
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).EnabledLongDown = true;
            ButtonVirtualScript.Instance.GetButton(keyValue.Value).onClick.AddListener(() => { OnClickButton(key); });
            iCnt++;
        }
    }

    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickButton(KeyCode key)
    {
        switch (key)
        {

            case KeyCode.Alpha3://拔针
                                //穿刺针-开
                MyNeedle.transform.Find("Tube_clip_cl").localPosition = new Vector3(MyNeedle.transform.Find("Tube_clip_cl").localPosition.x - 0.01f
                                                                 , MyNeedle.transform.Find("Tube_clip_cl").localPosition.y 
                                                                 , MyNeedle.transform.Find("Tube_clip_cl").localPosition.z);
                MyNeedle.transform.Find("xiongchuanzhen").localPosition = new Vector3(MyNeedle.transform.Find("xiongchuanzhen").localPosition.x - 0.01f
                                                                 , MyNeedle.transform.Find("xiongchuanzhen").localPosition.y
                                                                 , MyNeedle.transform.Find("xiongchuanzhen").localPosition.z);
                //MyNeedle.transform.localPosition = new Vector3(MyNeedle.transform.localPosition.x
                //                                                 , MyNeedle.transform.localPosition.y + 0.01f
                //                                                 , MyNeedle.transform.localPosition.z);
                temp++;
                if (temp == 5)
                {
                    MyNeedle.SetActive(false);
                    ButtonVirtualScript.Instance.RemoveVirtualButton();
                    ButtonVirtualScript.Instance.ClearDicButton();
                    State = StepStatus.did;
                    temp = 0;
                }
                //穿刺针-关
                //chuancizhen_close.SetActive(!chuancizhen_close.activeSelf);
                break;
            default:
                break;
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
