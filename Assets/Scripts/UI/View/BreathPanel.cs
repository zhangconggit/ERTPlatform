using UnityEngine;
using System.Collections;
using CFramework;


/// <summary>
/// 叮嘱呼吸界面
/// </summary>
public class BreathPanel : UPageBase
{
    private static BreathPanel _Instance;
    public static BreathPanel Instance()
    {
        if (_Instance == null)
        {
            _Instance = new BreathPanel();
        }
        return _Instance;
    }

    public UPlane back;
    public UText theTitle;//界面标题

    public UPlane start;
    public UText starttxt;
    UPageButton startbutton;

    public UPlane stop;
    public UText stoptxt;
    UPageButton stopbutton;

    public delegate void CallBack();
    CallBack StartCallBack;
    CallBack EndCallBack;
    public void SetStartCallBack(CallBack callback)
    {
        StartCallBack = callback;
    }

    public void SetEndCallBack(CallBack callback)
    {
        EndCallBack = callback;
    }
    /// <summary>
    /// 创建叮嘱呼吸UI
    /// </summary>
    public void Create()
    {

        back = new UPlane();
        this.name = "背景";
        SetBorderSpace(0, 0, 0, 0);
        //back = new UPlane("Images/login_image/大背景");
        back.name = "背景";
        back.SetParent(this);
        back.SetBorderSpace(0, 0, 0, 0);
        back.color = new Color(1, 1, 0.7f);
        back.SetBorderSpace(200, 1685, 650, 50);

        theTitle = new UText();
        theTitle.SetParent(back);
        theTitle.SetAnchored(AnchoredPosition.full);
        theTitle.SetBorderSpace(0, 10, 180, 10);
        theTitle.baseText.fontSize = 40;
        theTitle.baseText.color = Color.red;
        theTitle.baseText.alignment = TextAnchor.MiddleLeft;
        theTitle.text = "叮嘱呼吸";
        theTitle.name = "叮嘱呼吸";

        start = new UPlane();
        start.SetParent(back);
        start.SetBorderSpace(0, 0, 0, 0);
        start.color = new Color(1, 1, 1);
        start.SetBorderSpace(50, 20, 20, 20);

        startbutton = new UPageButton();
        startbutton.SetParent(start);
        startbutton.SetAnchored(AnchoredPosition.full);
        startbutton.SetBorderSpace(0, 0, 0, 0);
        startbutton.onClick.AddListener(onclickstart);

        starttxt = new UText();
        starttxt.SetParent(startbutton);
        starttxt.SetAnchored(AnchoredPosition.full);
        starttxt.SetBorderSpace(0, 30, 10, 30);
        starttxt.baseText.fontSize = 40;
        starttxt.baseText.color = Color.red;
        starttxt.baseText.alignment = TextAnchor.MiddleLeft;
        starttxt.text = "开始呼吸";
        starttxt.name = "开始呼吸";

        stop = new UPlane();
        stop.SetParent(back);
        stop.SetBorderSpace(0, 0, 0, 0);
        stop.color = new Color(1, 1, 1);
        stop.SetBorderSpace(50, 20, 20, 20);

        stopbutton = new UPageButton();
        stopbutton.SetParent(stop);
        stopbutton.SetAnchored(AnchoredPosition.full);
        stopbutton.SetBorderSpace(0, 0, 0, 0);
        stopbutton.onClick.AddListener(onclickstop);

        stoptxt = new UText();
        stoptxt.SetParent(stopbutton);
        stoptxt.SetAnchored(AnchoredPosition.full);
        stoptxt.SetBorderSpace(0, 30, 10, 30);
        stoptxt.baseText.fontSize = 40;
        stoptxt.baseText.color = Color.red;
        stoptxt.baseText.alignment = TextAnchor.MiddleLeft;
        stoptxt.text = "停止呼吸";
        stoptxt.name = "停止呼吸";

        stop.transform.gameObject.SetActive(false);
    }

    void onclickstart()
    {
        stop.transform.gameObject.SetActive(true);
        start.transform.gameObject.SetActive(false);

        if (StartCallBack != null)
        {
            StartCallBack();
        }
    }

    void onclickstop()
    {
        stop.transform.gameObject.SetActive(false);
        start.transform.gameObject.SetActive(true);

        if (EndCallBack != null)
        {
            EndCallBack();
        }
    }

    public void Destroy()
    {
        back.Destroy();
    }

}
