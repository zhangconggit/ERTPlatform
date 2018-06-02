using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 铺巾
/// </summary>
public class Pujin : StepBase
{
    /// <summary>
    /// 体位选择结束按钮
    /// </summary>
    UPageButton btnOk;
    /// <summary>
    /// 洞巾
    /// </summary>
    GameObject dongjin;
    /// <summary>
    /// 处理完成
    /// </summary>
    bool isFinish;

    public Pujin()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);

        //洞巾
        dongjin = GameObject.Find("Models").transform.Find("tools/dongjin").gameObject;
        dongjin.transform.position = new Vector3(-0.2509348f, 0.6744605f, 0.2767353f);
        dongjin.transform.rotation = Quaternion.Euler(270f, 0, 0);

        //其他初期化
        isFinish = false;

        //追加评分
        AddDefaultScore();
    }
    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //洗手动画
        Texture2D[,] tex = new Texture2D[1, 7];
        for (int i = 0; i < 7; i++)
        {
            tex[0, i] = UIRoot.Instance.UIResources[string.Format("xishou_{0}_T", i + 1)] as Texture2D;
        }
        AnimationScript.Instance.InsertSprites("xishou", tex);
        //戴手套动画
        tex = new Texture2D[1, 5];
        for (int i = 0; i < 5; i++)
        {
            tex[0, i] = UIRoot.Instance.UIResources[string.Format("shoutao_{0}_T", i + 1)] as Texture2D;
        }
        AnimationScript.Instance.InsertSprites("daishoutao", tex);

        //隐藏洞巾
        dongjin.SetActive(false);

        //设置右边按钮
        SettingBtn(new Dictionary<string, List<string>>
        {
             { "洗手",  new List<string>() { "Sterile_hole_towel_toolbar", "Sterile_hole_towel_toolbar_h" }}
            ,{ "戴手套", new List<string>() { "Medical_gloves", "Medical_gloves_h" } }
            ,{ "洞巾", new List<string>() { "Sterile_hole_towel_toolbar", "Sterile_hole_towel_toolbar_h" } }
        });

        //结束按钮
        btnOk = new UPageButton(AnchoredPosition.bottom);
        btnOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        btnOk.text = "铺巾结束";
        btnOk.LoadSprite("anniu-160");
        btnOk.LoadPressSprite("anniu-160h");
        btnOk.button.text.color = UnityEngine.Color.white;
        btnOk.onClick.AddListener(onClickOk);
        btnOk.SetActive(false);

        //语音：点击患者,选择合适体位
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_pujin");

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 步骤完成
    /// </summary>
    public override void StepFinish()
    {
        //停止并清除语音
        VoiceControlScript.Instance.VoiceStop();

        base.StepFinish();
    }

    /// <summary>
    /// 设置右边按钮
    /// </summary>
    /// <param name="lstBtnImagePath">右边按钮图片</param>
    private void SettingBtn(Dictionary<string, List<string>> lstBtnImagePath)
    {
        // 分辨率：1920 x 1080
        //图片初期位置和大小
        Rect pos = new Rect(0, 0, 120, 120);
        int i_Divisor = lstBtnImagePath.Count / 2;
        int i_Remain = lstBtnImagePath.Count % 2;
        float yStart = 0;
        if (i_Remain == 0)
        {
            yStart = -i_Divisor * pos.height;
        }
        else
        {
            yStart = -i_Divisor * pos.height - pos.height / 2;
        }
        foreach (KeyValuePair<string, List<string>> keyValue in lstBtnImagePath)
        {
            pos.y = yStart;
            CreatImageBtn(keyValue.Value[0], keyValue.Value[1], pos, keyValue.Key);
            yStart = yStart + pos.height;
        }
    }

    /// <summary>
    /// 创建右边按钮
    /// </summary>
    /// <param name="defPath">初期图片</param>
    /// <param name="path">图片</param>
    /// <param name="pos">图片位置</param>
    private void CreatImageBtn(string defPath, string path, Rect pos, string text)
    {
        UPageButton btn = CreateUI<UPageButton>();
        btn.name = path;
        btn.SetAnchored(AnchoredPosition.right);
        btn.rect = pos;
        btn.LoadSprite(defPath);
        btn.LoadPressSprite(path);
        btn.text = text;

        btn.onClick.AddListener(() => { OnClickImageButton(btn); });
    }
    
    /// <summary>
    /// 体位图click事件
    /// </summary>
    /// <param name="btn">选中体位</param>
    private void OnClickImageButton(UPageButton btn)
    {
        string sSprite = btn.pressSprite.name;
        btn.LoadPressSprite(btn.sprite.name);
        btn.LoadSprite(sSprite);
        switch (btn.text)
        {
            case "洗手":
                btn.onClick.RemoveAllListener();
                AnimationScript.Instance.GoPlaySprites("xishou", 0.5f, CallVoice1);
                //追加评分
                AddXiShouScore(1);
                break;
            case "戴手套":
                btn.onClick.RemoveAllListener();
                AnimationScript.Instance.GoPlaySprites("daishoutao", 0.5f, CallVoice2);
                //追加评分
                AddDaishoutaoScore(1);
                break;
            case "洞巾":
                btn.onClick.RemoveAllListener();
                //显示洞巾
                dongjin.SetActive(true);
                dongjin.transform.Find("chest_Sterile_hole_towel_t").GetComponent<BoxCollider>().enabled = false;
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "select_hole_towel");

                isFinish = true;
                //追加评分
                AddPujinScore(1);
                break;
        }
        if (isFinish)
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "WearGlovesFinished3",true, onClickOk);
        }
    }
    /// <summary>
    /// 回调语音
    /// </summary>
    /// <param name="voiceName"></param>
    private void CallVoice1()
    {
        //语音：洗手完成，开始戴手套
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "WearGlovesFinished2");
    }

    /// <summary>
    /// 回调语音
    /// </summary>
    /// <param name="voiceName"></param>
    private void CallVoice2()
    {
        //语音：戴手套完成，检查穿刺包器械，开始铺巾
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "WearGlovesFinished");
    }

    /// <summary>
    /// 结束按钮
    /// </summary>
    private void onClickOk()
    {
        //当前步骤结束
        State = StepStatus.did;
    }
    
    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddDefaultScore() {
        //追加评分(洗手)
        AddXiShouScore(0);
        //追加评分(戴手套)
        AddDaishoutaoScore(0);
        //追加评分(检查消毒指示卡)
        AddXiaoduScore(0);
        //追加评分(核对包内器械)
        AddQixieScore(0);
        //追加评分(检查穿刺针)
        AddChuancizhenScore(0);
        //追加评分(铺巾)
        AddPujinScore(0);
    }
    /// <summary>
    /// 追加评分(洗手)
    /// </summary>
    private void AddXiShouScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015101" } //操作前没有洗手
            ,{ -1, "10015102" } //操作前洗手错误
            ,{ 1, "10015100" } //操作前正确洗手
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
    /// <summary>
    /// 追加评分(戴手套)
    /// </summary>
    private void AddDaishoutaoScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015111" } //没有戴手套
            ,{ -1, "10015112" } //戴手套方法错误
            ,{ 1, "10015110" } //正确戴手套
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
    /// <summary>
    /// 追加评分(检查消毒指示卡)
    /// </summary>
    private void AddXiaoduScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015121" } //没有检查消毒指示卡
            ,{ 1, "10015120" } //检查过消毒指示卡
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
    /// <summary>
    /// 追加评分(核对包内器械)
    /// </summary>
    private void AddQixieScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015131" } //没有核对包内器械
            ,{ 1, "10015130" } //核对过包内器械
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
    /// <summary>
    /// 追加评分(检查穿刺针)
    /// </summary>
    private void AddChuancizhenScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015141" } //没有检查穿刺针
            ,{ 1, "10015140" } //检查过穿刺针是否通畅
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
    /// <summary>
    /// 追加评分(铺巾)
    /// </summary>
    private void AddPujinScore(int scoreKey)
    {
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { 0, "10015151" } //没有铺巾
            ,{ -1, "10015152" } //铺巾位置错误
            ,{ 1, "10015150" } //正确铺巾
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
