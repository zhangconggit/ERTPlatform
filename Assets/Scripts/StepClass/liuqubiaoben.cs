using UnityEngine;
using System.Collections;
using CFramework;

/// <summary>
/// 留取标本
/// </summary>
public class liuqubiaoben : StepBase
{
    /// <summary>
    /// 试管架子
    /// </summary>
    private GameObject SGJ;

    /// <summary>
    /// 假注射器
    /// </summary>
    private GameObject JZSQ;

    /// <summary>
    /// 真注射器
    /// </summary>
    private GameObject ZZSQ;

    /// <summary>
    /// 针头
    /// </summary>
    private GameObject zt;

    /// <summary>
    /// 注射器身体
    /// </summary>
    GameObject ZSQST;

    /// <summary>
    /// 注射器把手
    /// </summary>
    GameObject ZSQBS;

    /// <summary>
    /// 液体
    /// </summary>
    GameObject yt;

    /// <summary>
    /// 病原体检查标签
    /// </summary>
    GameObject byt;

    /// <summary>
    /// 常规检查标签
    /// </summary>
    GameObject cg;

    /// <summary>
    /// 生化检查标签
    /// </summary>
    GameObject sh;

    /// <summary>
    /// 液体的长度
    /// </summary>
    float YTnum;

    /// <summary>
    /// 把手向下注入一次的向量
    /// </summary>
    Vector3 injectBSdir = new Vector3(0, 0, -0.0801f) - new Vector3(0, 0, -0.1288f);

    /// <summary>
    /// 液体被注入一次时位置移动的向量
    /// </summary>
    Vector3 intoYtsdir = new Vector3(0, 0, 0) - new Vector3(0, 0, -0.025f);

    /// <summary>
    /// 注射器向上移动一次的向量
    /// </summary>
    Vector3 injectup = new Vector3(-2.1333f, 1.0056f, 0.2962f) - new Vector3(-2.1333f, 0.9564f, 0.2962f);// new Vector3(-1.058f, -0.2416f, -1.7316f) - new Vector3(-1.051f, -0.288f, -1.726f);

    /// <summary>
    /// 注射器向左边瓶子移动一次的向量
    /// </summary>
    Vector3 injectDir = new Vector3(-2.1333f, 0.9564f, 0.2962f) - new Vector3(-2.1333f, 0.9564f, 0.3225f);// new Vector3(-1.075f, -0.29f, -1.714f) - new Vector3(-1.099f, -0.292f, -1.702f);// Vector3.zero;

    /// <summary>
    /// 注射器的初始世界坐标
    /// </summary>
    Vector3 injectInitPosition;

    /// <summary>
    /// 注射器的初始世界旋转欧拉角 
    /// </summary>
    Vector3 injectIniteulerAngles;

    public liuqubiaoben()
    {
        //设置摄像机的位置
        cameraEnterPosition = new Vector3(-1.803f, 1.009f, 0.341f);
        //设置摄像机的旋转角度
        cameraEnterEuler = new Vector3(2.5648f, 273.479f, 360.2362f);

        SGJ = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02").gameObject;
        JZSQ = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sj1_injector").gameObject;
        ZZSQ = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi").gameObject;

        ZSQST = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/zhusheqi").gameObject;
        ZSQBS = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/bashou").gameObject;

        injectInitPosition = ZZSQ.transform.position;
        injectIniteulerAngles = ZZSQ.transform.eulerAngles;

        //获取液体
        yt = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/yeti/Cylinder").gameObject;
        //获取针头
        zt = GameObject.Find("Models").transform.Find("tools/胸穿针/zhusheqi/sj1_injector_needle").gameObject;

        //获取病原体检查标签
        byt = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/byt").gameObject;
        //获取常规检查标签
        cg = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/cg").gameObject;
        //获取生化检查标签
        sh = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sh").gameObject;

        //留取标本数等于3小瓶
        AddScoreItem("10018170");
    }

    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);
        
        SGJ.SetActive(true);
        JZSQ.SetActive(false);
        ZZSQ.transform.position = new Vector3(-2.1333f, 0.9564f, 0.321f);
        ZZSQ.transform.eulerAngles = new Vector3(90, 90, 0);
        ZZSQ.SetActive(true);
        zt.SetActive(true);
        byt.SetActive(false);
        cg.SetActive(false);
        sh.SetActive(false);

    }
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        YTnum = yt.transform.localScale.y / 3;
        ModelCtrol.Instance.MoveModel(ZSQBS, ZSQBS.transform.localPosition + injectBSdir / 3, 2, callOneInto);
        ModelCtrol.Instance.MoveModel(yt, yt.transform.localPosition + intoYtsdir / 3, 2, null);
        ModelCtrol.Instance.ScaleModel(yt, yt.transform.localScale = new Vector3(yt.transform.localScale.x, yt.transform.localScale.y - YTnum, yt.transform.localScale.z), 2, null);
    }

    private void callOneInto()
    {
        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position + injectup, 1, callOneUp, true);

    }

    private void callOneUp()
    {

        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position + injectDir, 1, callOneDown, true);
    }

    private void callOneDown()
    {
        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position - injectup, 1, callOneMove, true);

    }

    private void callOneMove()
    {
        ModelCtrol.Instance.ScaleModel(yt, yt.transform.localScale = new Vector3(yt.transform.localScale.x, yt.transform.localScale.y - YTnum, yt.transform.localScale.z), 2, null);
        ModelCtrol.Instance.MoveModel(yt, yt.transform.localPosition + intoYtsdir / 3, 2, null);
        ModelCtrol.Instance.MoveModel(ZSQBS, ZSQBS.transform.localPosition + injectBSdir / 3, 2, callTwoInto);
    }

    private void callTwoInto()
    {
        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position + injectup, 1, callTwoUp, true);

    }

    private void callTwoUp()
    {

        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position + injectDir, 1, callTwoDown, true);
    }

    private void callTwoDown()
    {
        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position - injectup, 1, callTwoMove, true);

    }

    private void callTwoMove()
    {
        ModelCtrol.Instance.ScaleModel(yt, yt.transform.localScale = new Vector3(yt.transform.localScale.x, yt.transform.localScale.y - YTnum, yt.transform.localScale.z), 2, null);
        ModelCtrol.Instance.MoveModel(yt, yt.transform.localPosition + intoYtsdir / 3, 2, null);
        ModelCtrol.Instance.MoveModel(ZSQBS, ZSQBS.transform.localPosition + injectBSdir / 3, 2, EndUp);
    }
    private void EndUp()
    {
        ModelCtrol.Instance.MoveModel(ZZSQ, ZZSQ.transform.position + injectup, 1, hide, true);

    }

    private void hide()
    {
        ZZSQ.SetActive(false);
        SGJ.SetActive(false);
        State = StepStatus.did;
    }


    public override void StepFinish()
    {
        SGJ.SetActive(true);
        JZSQ.SetActive(false);
        ZZSQ.transform.position = injectInitPosition;
        ZZSQ.transform.eulerAngles = injectIniteulerAngles;
        ZZSQ.SetActive(true);
        zt.SetActive(true);
        byt.SetActive(false);
        cg.SetActive(false);
        sh.SetActive(false);
        base.StepFinish();
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
