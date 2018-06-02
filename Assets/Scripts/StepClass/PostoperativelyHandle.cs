using UnityEngine;
using System.Collections;
using CFramework;

public class PostoperativelyHandle : StepBase
{
    ///// <summary>
    ///// 术后处理操作评分
    /////               =0没有洗手
    /////               =1洗手
    ///// </summary>
    //int shuhouxishou = 0;
    ///// <summary>
    ///// 术后挤压穿刺点
    /////              =0没有按压
    /////              =1有按压
    ///// </summary>
    //int shuhouanya = 0;
    ///// <summary>
    ///// 术后固定
    /////              =0有固定
    /////              =1没有固定
    ///// </summary>
    //int shuhouguding = 0;
    ///// <summary>
    ///// 术后送检
    /////              =5送检
    /////              =0没有送检
    ///// </summary>
    //int shuhouseeds = 0;
    ///// <summary>
    ///// 术后记录
    /////             =0没有记录
    /////             =1有记录
    ///// </summary>
    GameObject sendover = null;
    GameObject shabuu = null;
    GameObject dashabuu = null;
    bool m_startPlayaAni = false;
    float m_Anitime = 0;
    /// <summary>
    /// 定义时间
    /// </summary>
    UImage waitTime = null;
    public PostoperativelyHandle()
    {
        sendover= GameObject.Find("Models").transform.Find("completejiludonghua").gameObject;//获取完成记录模型
        //sendover.transform.localScale = (0.7; 0.7);
        shabuu=GameObject.Find("Models").transform.Find("shabu_anya").gameObject;
        //shabuu.transform.position = new Vector3(-1.133f,0.028f,0.383f);
        dashabuu = GameObject.Find("Models").transform.Find("tools/tieshabu").gameObject;
        shabuu.SetActive(false);
        dashabuu.SetActive(false);
        sendover.SetActive(false);
        UPageButton button0 = CreateUI<UPageButton>();//定义选择按钮
        button0.rect = new Rect(800, -70, 180, 70);
        button0.text = "按压";
        button0.LoadSprite("anniu-160");
        button0.LoadPressSprite("anniu-160h");
        UPageButton button1 = CreateUI<UPageButton>();
        button1.rect = new Rect(800, 30, 180, 70);
        button1.text = "固定";
        button1.LoadSprite("anniu-160");
        button1.LoadPressSprite("anniu-160h");
        UPageButton button2 = CreateUI<UPageButton>();
        button2.rect = new Rect(800, 130, 180, 70);
        button2.text = "洗手";
        button2.LoadSprite("anniu-160");
        button2.LoadPressSprite("anniu-160h");
        UPageButton button3 = CreateUI<UPageButton>();
        button3.rect = new Rect(800, 230, 180, 70);
        button3.text = "对话";
        button3.LoadSprite("anniu-160");
        button3.LoadPressSprite("anniu-160h");
        UPageButton button4 = CreateUI<UPageButton>();
        button4.rect = new Rect(800, 330, 180, 70);
        button4.text = "送检";
        button4.LoadSprite("anniu-160");
        button4.LoadPressSprite("anniu-160h");
        UPageButton button5 = CreateUI<UPageButton>();
        button5.rect = new Rect(800, 430, 180, 70);
        button5.text = "记录";
        button5.LoadSprite("anniu-160");
        button5.LoadPressSprite("anniu-160h");
        button0.onClick.AddListener(() => { OnClickImageButton(button0); });
        button1.onClick.AddListener(() => { OnClickImageButton(button1); });
        button2.onClick.AddListener(() => { OnClickImageButton(button2); });
        button3.onClick.AddListener(() => { OnClickImageButton(button3); });
        button4.onClick.AddListener(() => { OnClickImageButton(button4); });
        button5.onClick.AddListener(() => { OnClickImageButton(button5); });
        waitTime = new UImage(AnchoredPosition.full);
        waitTime.SetBorderSpace(0, 0, 0, 0);
        waitTime.color = new Color(0, 0, 0, 0.5f);
        UText txt = new UText();
        txt.text = "3分钟后......";
        txt.SetParent(waitTime);
        txt.rect = new Rect(0, 0, 300, 60);
        waitTime.SetActive(false);
    }
    public override void EnterStep(float cameraTime = 0)
    {

        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);
        base.EnterStep(cameraTime);
    }
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
    }
    private void OnClickImageButton(UPageButton btn)
    {   
        string sSprite = btn.pressSprite.name;
        //if (sSprite.IndexOf("h") < 0)
        //{
        //    return;
        //}
        btn.LoadPressSprite(btn.sprite.name);
        btn.LoadSprite(sSprite);
        switch (btn.text)
        {
            case "按压":
                shabuu.SetActive(true);
                //shabuu.transform.position = Model_PunctureInfo.Instance.m_PuncturePoint;
                //ModelCtrol.Instance.setModelsOnNormalline(shabuu, Model_PunctureInfo.Instance.m_PunctureNormal, new Vector3(0, 0, -1), 0);
                TimeManager.Instance.WaitTime(1f, () => {
                    waitTime.SetActive(true);
                    TimeManager.Instance.WaitTime(3f, () => {
                        waitTime.SetActive(false);
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "fixed", true); 
                    });
                });
                AddScoreItem("100191101001");
                break;
            case "固定":
                if(shabuu.activeSelf)
                    dashabuu.SetActive(true);
                AddScoreItem("100191201001");

                break;
            case "洗手":
                AnimationScript.Instance.GoPlaySprites("xishou", 0.5f,null);//播放洗手动画
                //fenshuKbn_Xishou = 1;
                break;
            case "对话":
                VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "chuanciover", true);
                VoiceControlScript.Instance.SetCallBack(() => { 
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "good", true);
                    VoiceControlScript.Instance.SetCallBack(() =>
                    {
                        VoiceControlScript.Instance.VoiceStop();
                        VoiceControlScript.Instance.SetCallBack(null);
                    });
                });
                //fenshuKbn_Daishoutao = 1;
                break;
            case "送检":
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "sendover", true);
                break;
            case "记录":
                sendover.SetActive(true);
                sendover.GetComponent<Animation>().Play ("Take 001");//播放记录动画
                sendover.GetComponent<Animation>()["Take 001"].speed = 0;
                m_Anitime = 0;
                m_startPlayaAni = true;
                break;
        }
        //State = StepStatus.did;
    }

    void thanks()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "thanks", true); 
    }

    //private void CallVoice1()
    //{
    //    //
    //    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "WearGlovesFinished2");
    //}
    //public override void CameraMoveFinished()
    //{

    //    base.CameraMoveFinished();

    //}
    public void xishou()
    {

    }
   // GameObject shajin = GameObject.Find("").transform.Find("").gameObject;
    public  void dianji()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "fixed", true);
        //shajin.transform.position = new Vector3(860,0,20);

    }
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    public override void StepUpdate()
    {
        base.StepUpdate();
        if (m_startPlayaAni)
        {
            m_Anitime += Time.deltaTime;
            sendover.GetComponent<Animation>()["Take 001"].time = m_Anitime;
            if (m_Anitime >= sendover.GetComponent<Animation>()["Take 001"].length)
            {
                //ModelCtrol.Find("completejiludonghua/autograph").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.name;
                m_startPlayaAni = false;
                sendover.SetActive(false);
                TimeManager.Instance.WaitTime(2f, () => { State = StepStatus.did; });

            }
        }
    }
    public override void StepFinish()
    {

        base.StepFinish();
    }
    //void Update()
    //{
    //    base.StepUpdate();
    //    if (m_startPlayaAni)
    //    {
    //        m_Anitime += Time.deltaTime;
    //        sendover.GetComponent<Animation>()["Take 001"].time = m_Anitime;
    //        if (m_Anitime >= sendover.GetComponent<Animation>()["Take 001"].length)
    //        {
    //            //ModelCtrol.Find("completejiludonghua/autograph").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.name;
    //            m_startPlayaAni = false;
    //            sendover.SetActive(false);
    //            TimeManager.Instance.WaitTime(2f, () => { State = StepStatus.did; });

    //        }
    //    }
    //}
}
