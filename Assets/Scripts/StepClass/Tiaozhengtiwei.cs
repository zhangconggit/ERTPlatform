using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 调整体位
/// </summary>
public class Tiaozhengtiwei : StepBase
{
    UPageButton currentButton = null;
    /// <summary>
    /// 站立人model
    /// </summary>
    GameObject standingPeople;
    /// <summary>
    /// 坐着人model
    /// </summary>
    GameObject sitPeople;
    /// <summary>
    /// 显示判定
    /// </summary>
    bool isShow;
    ///// <summary>
    ///// 体位选择结束按钮
    ///// </summary>
    //UPageButton btnTiweixuanzeOk;
    /// <summary>
    /// 处理完成
    /// </summary>
    bool isFinish;

    public Tiaozhengtiwei()
    {
        //设置镜头的位置和欧拉角
        string[] CameraPos = fileHelper.ReadIni(GetType().Name, "CameraPos", "StepConfig").Split(',');
        string[] CameraRot = fileHelper.ReadIni(GetType().Name, "CameraRot", "StepConfig").Split(',');
        cameraEnterPosition = new Vector3(float.Parse(CameraPos[0]), float.Parse(CameraPos[1]), float.Parse(CameraPos[2]));
        cameraEnterEuler = new Vector3(float.Parse(CameraRot[0]), float.Parse(CameraRot[1]), float.Parse(CameraRot[2]));

        //设置未开始手术的人的位置和欧拉角
        string Stand = fileHelper.ReadIni(GetType().Name, "StandPeople", "StepConfig");
        standingPeople = GameObject.Find("Models").transform.Find(Stand).gameObject;
        string[] StandLocalPos = fileHelper.ReadIni(GetType().Name, "StandLocalPos", "StepConfig").Split(',');
        if (StandLocalPos != null & StandLocalPos.Length == 3)
        {
            standingPeople.transform.localPosition = new Vector3(float.Parse(StandLocalPos[0]), float.Parse(StandLocalPos[1]), float.Parse(StandLocalPos[2])); // new Vector3(-0.5139443f, -0.002361169f, -0.269f);
        }
        string[] StandLocalRot = fileHelper.ReadIni(GetType().Name, "StandLocalRot", "StepConfig").Split(',');
        if (StandLocalRot != null & StandLocalRot.Length == 3)
        {
            standingPeople.transform.localRotation = Quaternion.Euler(float.Parse(StandLocalRot[0]), float.Parse(StandLocalRot[1]), float.Parse(StandLocalRot[2])); //Quaternion.Euler(-90, 92.6531f, 0);
        }

        //设置开始手术的人的位置和欧拉角
        string People = fileHelper.ReadIni(GetType().Name, "People", "StepConfig");
        sitPeople = GameObject.Find("Models").transform.Find(People).gameObject;
        string[] PeopleLocalPos = fileHelper.ReadIni(GetType().Name, "PeopleLocalPos", "StepConfig").Split(',');
        if (PeopleLocalPos != null & PeopleLocalPos.Length == 3)
        {
            sitPeople.transform.localPosition = new Vector3(float.Parse(PeopleLocalPos[0]), float.Parse(PeopleLocalPos[1]), float.Parse(PeopleLocalPos[2]));
        }
        string[] PeopleLocalRot = fileHelper.ReadIni(GetType().Name, "PeopleLocalRot", "StepConfig").Split(',');
        if (PeopleLocalRot != null & PeopleLocalRot.Length == 3)
        {
            sitPeople.transform.localRotation = Quaternion.Euler(float.Parse(PeopleLocalRot[0]), float.Parse(PeopleLocalRot[1]), float.Parse(PeopleLocalRot[2]));
        }
        //其他初期化
        isShow = false;
        isFinish = false;

        //追加评分
        AddScore(0);
    }

    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //显示手术未开始的人
        standingPeople.SetActive(true);
        //隐藏手术已经开始的人
        sitPeople.SetActive(false);

        ////体位选择结束按钮
        //btnTiweixuanzeOk = new UPageButton(AnchoredPosition.bottom);
        //btnTiweixuanzeOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        //btnTiweixuanzeOk.text = "标记穿刺点结束";
        //btnTiweixuanzeOk.LoadSprite("anniu-160");
        //btnTiweixuanzeOk.LoadPressSprite("anniu-160h");
        //btnTiweixuanzeOk.button.text.color = UnityEngine.Color.white;
        //btnTiweixuanzeOk.onClick.AddListener(onClickTiweixuanzeOk);
        //btnTiweixuanzeOk.SetActive(false);

        //语音：点击患者,选择合适体位
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "clickBody");

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 步骤更新
    /// </summary>
    public override void StepUpdate()
    {
        if (!isShow)
        {
            //按下鼠标左键
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
                RaycastHit rHit;
                if (Physics.Raycast(ray, out rHit))
                {
                    string  StanepeopleName= fileHelper.ReadIni(GetType().Name, "StanepeopleName", "StepConfig");
                    //站立人
                    if (rHit.collider.name != StanepeopleName)
                    {
                        if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                        {
                            //语音：点击患者,选择合适体位
                            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "clickBody");
                        }
                    }
                }
                else
                {
                    if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                    {
                        //语音：点击患者,选择合适体位
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "clickBody");
                    }
                }
            }
        }
        base.StepUpdate();
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        ////摧毁体位选择结束按钮
        //btnTiweixuanzeOk.Destroy();

        //显示坐着人model
        sitPeople.SetActive(true);
        //隐藏站着人model
        standingPeople.SetActive(false);
        //停止并清除语音
        VoiceControlScript.Instance.VoiceStop();

        base.StepFinish();
    }

    public override void StepStartState()
    {
        base.StepStartState();
        //站立人model
        standingPeople.SetActive(true);
        //坐着人model
        sitPeople.SetActive(false);
    }

    /// <summary>
    /// 点击体位选择结束按钮
    /// </summary>
    private void onClickTiweixuanzeOk()
    {
        //当前步骤结束
        State = StepStatus.did;
    }

    /// <summary>
    /// 调整体位_人物点击事件
    /// </summary>
    /// <param name="obj">被点击的model</param>
    public override void OnClickModel(RaycastHit obj)
    {
        string PeopleName = fileHelper.ReadIni(GetType().Name, "StanepeopleName", "StepConfig");
        //调整体位_人物被点中
        if (obj.collider.name == PeopleName)
        {
            if (!isShow)
            {
                string[] Picture1 = fileHelper.ReadIni(GetType().Name, "Picture1", "StepConfig").Split(',');
                string[] Picture2 = fileHelper.ReadIni(GetType().Name, "Picture2", "StepConfig").Split(',');
                string[] Picture3 = fileHelper.ReadIni(GetType().Name, "Picture3", "StepConfig").Split(',');
                string[] Picture4 = fileHelper.ReadIni(GetType().Name, "Picture4", "StepConfig").Split(',');
                //创建体位图
                SetTiweiImages(new Dictionary<string, string>
                {
                     { Picture1[0],Picture1[1] }
                    ,{ Picture2[0],Picture2[1]  }
                    ,{ Picture3[0],Picture3[1]  }
                    ,{ Picture4[0],Picture4[1]  }
                });
            }
        }

        base.OnClickModel(obj);
    }

    /// <summary>
    /// 创建体位图
    /// </summary>
    /// <param name="lstTiweiImagePath"></param>
    private void SetTiweiImages(Dictionary<string, string> lstTiweiImagePath)
    {
        string[] Size = fileHelper.ReadIni(GetType().Name, "Size", "StepConfig").Split(',');
        //分辨率：1920 x 1080
        //图片初期位置和大小
        Rect pos = new Rect(0, 0, int.Parse(Size[0]), int.Parse(Size[1]));
        //图片间的间距
        int diff = 50;
        int i_imagesCnt = lstTiweiImagePath.Count;
        //随机数
        int[] ran = MyCommon.RandomRepeat(i_imagesCnt);

        int index = 0;
        foreach (KeyValuePair<string, string> keyValue in lstTiweiImagePath)
        {
            pos.x = ran[index] * (pos.width + diff) - (pos.width + diff) * i_imagesCnt / 2 + pos.width / 2 + diff / 2;

            CreatImageBtn(keyValue.Key, keyValue.Value, pos);
            index++;
        }
        isShow = true;
    }

    /// <summary>
    /// 创建体位图按钮
    /// </summary>
    /// <param name="defPath">初期图片</param>
    /// <param name="path">图片</param>
    /// <param name="pos">图片位置</param>
    private void CreatImageBtn(string defPath, string path, Rect pos)
    {
        UPageButton btn = CreateUI<UPageButton>();
        btn.name = path;
        btn.SetAnchored(AnchoredPosition.center);
        btn.rect = pos;
        btn.LoadSprite(defPath);
        btn.LoadPressSprite(path);

        btn.onClick.AddListener(() => { OnClickImageButton(btn); });
    }

    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickImageButton(UPageButton btn)
    {
        if (isFinish)
        {
            return;
        }
        if (currentButton != null)
        {
            currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        }
        currentButton = btn;
        currentButton.LoadSprite(btn.pressSprite.name);

        //体位选择正确
        if (btn.sprite.name.IndexOf("ok") > -1)
        {
            isFinish = true;
            //追加评分
            AddScore(1);
            //语音：选择体位正确,开始叩诊
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "posture_correct", true, onClickTiweixuanzeOk);
        }
        //体位选择错误
        else
        {
            //追加评分
            AddScore(-1);
            //语音：选择体位错误
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "posture_wrong");
        }
    }

    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddScore(int scoreKey)
    {
        string[] NoTZ= fileHelper.ReadIni(GetType().Name, "NoTZ", "StepConfig").Split(',');
        string[] ErrorTZ = fileHelper.ReadIni(GetType().Name, "ErrorTZ", "StepConfig").Split(',');
        string[] OkTZ = fileHelper.ReadIni(GetType().Name, "OkTZ", "StepConfig").Split(',');
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { int.Parse(NoTZ[0]), NoTZ[1] } //没有调整体位
            ,{ int.Parse(ErrorTZ[0]), ErrorTZ[1] } //调整体位错误
            ,{ int.Parse(OkTZ[0]), OkTZ[1] } //调整体位正确
        };
        if (scoreKey == 0)
        {
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    //移除评分编码
                    RemoveScoreItem(scoreItem);
                }
            }
            //追加评分编码
            AddScoreItem(dicScore[scoreKey]);
        }
        else
        {
            int recordCnt = 0;
            bool isShouldAdd = true;
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    if (recordCnt == 0)
                    {
                        //移除评分编码
                        RemoveScoreItem(scoreItem);
                    }
                    else
                    {
                        isShouldAdd = false;
                        break;
                    }
                }
                recordCnt++;
            }
            if (isShouldAdd)
            {
                //追加评分编码
                AddScoreItem(dicScore[scoreKey]);
            }
        }
    }
}
