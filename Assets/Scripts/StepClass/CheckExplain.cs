using UnityEngine;
using System.Collections;
using CFramework;

public class CheckExplain : StepBase
{
    ///// <summary>
    ///// 核对信息得分
    /////           =0没有核对
    /////           =1核对错误
    /////           =2核对信息
    ///// </summary>
    //int heshixinxidefen = 0;
    ///// <summary>
    ///// 解释说明得分
    /////          =0没有解释
    /////          =-2解释错误
    /////          =2有做解释
    ///// 
    ///// </summary>
    //int jieshishuomingdefen = 0;
    ///// <summary>
    ///// 签同意书
    /////          =0不签同意书
    /////          =2签同意书
    ///// </summary>
    //int agreebookdefen = 0;
    GameObject camera = null;
    GameObject books = null;
    GameObject lastautograph= null;
    GameObject peopelemodel = null;
    GameObject donghua2 = null;
    bool m_startPlayaAni = false;
    float m_Anitime = 0;
    public CheckExplain()
    {
        donghua2 = GameObject.Find("Models").transform.Find("completejiludonghua").gameObject;
        books = GameObject.Find("Models").transform.Find("guanchang_bingli_B").gameObject;
        donghua2.SetActive(false);
        //books.SetActive(true);
        //gameobject 对应指针指向对象
        camera = GameObject.Find("Models").transform.Find("yizhubenCamera").gameObject;//给gameobject对象赋值（transform是事物（），getcomponent是组键（组成事物的元素），gameobject是节点（存储位置信息）） 
        camera.SetActive(false);
        lastautograph = GameObject.Find("Models").transform.Find("jiludonghua").gameObject;
        //lastautograph.transform.position = new Vector3();
        lastautograph.SetActive(false);
        //peopelemodel = GameObject.Find("Models").transform.Find("chest_body-new/chest_body_s").gameObject;
        peopelemodel.SetActive(false);
        // ModelCtrol.Find("yizhubenCamera")
    }
    public override void EnterStep(float cameraTime = 1)
    {
        cameraEnterPosition = new Vector3(1.354f, 1.366f, 1.605f);
        cameraEnterEuler = new Vector3(45f, -356f, -356f);
        base.EnterStep(cameraTime);//继承基类，镜头旋转 
        m_startPlayaAni = false;
    }
    public override void CameraMoveFinished()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "openyizhuben", true);
        base.CameraMoveFinished();
        books.SetActive(true);
        peopelemodel.SetActive(true);
    }
    public override void MouseClickModel(RaycastHit obj)
    {
        base.MouseClickModel(obj);
        if(obj.collider.name == "guanchang_bingli_B")
        {
            ModelCtrol.Instance.CameraPath(new Vector3(-0.968f, 1.001f, 1.123f), new Vector3(0, 90.60759f, 0), 1f, () =>
            {
                camera.SetActive(true);
                peopelemodel.SetActive(true);
                chushihuayizhuben();
                askname();
            }, false, DG.Tweening.Ease.Linear);
        }
    }
    void chushihuayizhuben()
    {
        ModelCtrol.Find("guanchang_bingli_B/kaishi/advice").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.advice;
        ModelCtrol.Find("guanchang_bingli_B/kaishi/name").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.name;
        ModelCtrol.Find("guanchang_bingli_B/kaishi/sex").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.sex.ToString();
        ModelCtrol.Find("guanchang_bingli_B/kaishi/age").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.age;
        ModelCtrol.Find("guanchang_bingli_B/kaishi/bed").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.bedNumber;
        ModelCtrol.Find("guanchang_bingli_B/kaishi/doctor").GetComponent<TextMesh>().text = "王医师";
        ModelCtrol.Find("guanchang_bingli_B/kaishi/date").GetComponent<TextMesh>().text = System.DateTime.Now.Month + "." + System.DateTime.Now.Day;   
       
        
    }
    void askname()
    {
        
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "whatname", true);
        VoiceControlScript.Instance.SetCallBack(()=> { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, Answername); });
        AddScoreItem("100121021001");

    }
    void Answername()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "wangyi", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, askBed); });
    }
    void askBed()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "Bed", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, AnswerBed); });
    }
    void AnswerBed()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "three", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, describecircumstances); });
        AddScoreItem("100121001001");

    }
    void describecircumstances()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "peihecheck", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, Answerwell); });
    }
    void Answerwell()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "well", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, needingattention); });
    }
    void needingattention()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "chuancizhong", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, Answergood); });
    }
    void Answergood()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "good", true);
        AddScoreItem("100121201001");
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, qianming); });
    }
    void qianming()
    {
        VoiceControlScript.Instance.AudioPlay(AudioStyle.NurseQuestions, "writename", true);
        VoiceControlScript.Instance.SetCallBack(() => { VoiceControlScript.Instance.SetCallBack(null); TimeManager.Instance.WaitTime(0.5f, palyqianming); });
        AddScoreItem("100121301001");


    }
    void palyqianming()
    {
        camera.SetActive(false);
        lastautograph.SetActive(true);
        ModelCtrol.Find("jiludonghua/autograph").GetComponent<TextMesh>().text = "";
        lastautograph.GetComponent<Animation>().Play("Take 001");
        lastautograph.GetComponent<Animation>()["Take 001"].speed = 0;
        m_Anitime = 0;
        m_startPlayaAni = true;
        VoiceControlScript.Instance.SetCallBack(null);
    }


    // Use this for initialization
    void Start () {
	
	}
    //public override void StepFinish()
    //{
    //    //核实信息结果
    //    switch (heshixinxidefen)
    //    {
    //        case 0:
    //            break;
    //        case 1:
    //            break;
    //        case 2:
    //            break;

    //    }
    //    switch(jieshishuomingdefen)
    //    {
    //        case 0:
    //            break;
            



    //    }
    //    base.StepFinish();
    //}
    public override void StepUpdate()
    {
        
        base.StepUpdate();
        if (m_startPlayaAni)
        {
            m_Anitime += Time.deltaTime;
            lastautograph.GetComponent<Animation>()["Take 001"].time = m_Anitime;
            if (m_Anitime >= lastautograph.GetComponent<Animation>()["Take 001"].length)
            {
                ModelCtrol.Find("jiludonghua/autograph").GetComponent<TextMesh>().text = CGlobal.currentSceneInfo.name;
                m_startPlayaAni = false;
                lastautograph.SetActive(false);
                TimeManager.Instance.WaitTime(2f, () => { State = StepStatus.did; });
               
            }
        }
    }

}
