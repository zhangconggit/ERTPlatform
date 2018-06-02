using UnityEngine;
using System.Collections;
using CFramework;
//using Puncture;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TimeManager : UIManager
{
    VoidDelegate timeoutCallback;
    /// <summary>
    /// 更新帧
    /// </summary>
    public CEvent<float> UpdateAction;
    public CEvent ScreenChange = new CEvent();
    public CEvent TimeOut = new CEvent();

    public GameObject timeText = null;
    float f_time = 0;
    bool startTime = false;
    bool isCountdown = false;
    float CountdownTime = 30 * 60; //30分钟
    int screenX = 1920;
    int screenY = 1080;


    public static new TimeManager Instance
    {
        get
        {
            return CMonoSingleton<TimeManager>.Instance();
        }
    }

    public new void OnDestroy()
    {
        CMonoSingleton<TimeManager>.OnDestroy();
    }
    public TimeManager()
    {
        UpdateAction = new CEvent<float>();
    }

    // Use this for initialization
    void Awake()
    {
        screenX = Screen.width;
        screenY = Screen.height;
    }
    // Use this for initialization
    void Start()
    {
        //timeText = UIRoot.Instance.GetCustomObject("time");
        //UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime)
        {
            f_time += Time.deltaTime;
            UpdateTime();
        }
        UpdateAction.Invoke(Time.deltaTime);

        if (screenX != Screen.width || screenY != Screen.height)
        {
            screenX = Screen.width;
            screenY = Screen.height;
            ScreenChange.Invoke();
        }
    }
    public override void ProcessMsg(CMsg msg)
    {

    }
    /// <summary>
    /// 开始计时
    /// </summary>
    /// <param name="abs"></param>
    public void StartTime(bool abs = true)
    {
        if (startTime == false)
        {
            //IDataComponent.GetInstance().addStartTime();
            f_time = 0;
            startTime = true;
            isCountdown = !abs;
            if (timeText != null)
                timeText.SetActive(true);
        }
    }
    /// <summary>
    /// 停止计时
    /// </summary>
    /// <param name="abs"></param>
    public void StopTime(bool abs = true)
    {
        startTime = false;
    }
    void UpdateTime()
    {
        if (timeText == null)
            timeText = UIRoot.Instance.GetCustomObject("time");

        float t = isCountdown ? (CountdownTime - f_time) : f_time;
        if (t < 0)
            t = 0;

        DateTime date = new DateTime(1970, 1, 1).AddSeconds((int)t);//
        if (timeText == null)
            Debug.Log("Time text is Null！");
        else
            timeText.GetComponent<Text>().text = date.ToString("HH:mm:ss");

        if (this.IsOutTime())
        {
            this.StopTime(true);
            if (CGlobal.userName == "")
            {
                UMessageBox.Show("提示", "当前时间已到，请退出程序！", "确定", () =>
                {
                    Application.Quit();
                });
                return;
            }
            else
            {
                UMessageBox.Show("提示", "操作时间已到，请点是提交成绩！点否将退出程序，成绩无效！", "提交成绩", () => { TimeOut.Invoke(); }, "放弃提交", () =>
                {
                    Application.Quit();
                });
            }
        }
    }

    /// <summary>
    ///  用时
    /// </summary>
    /// <returns>单位秒</returns>
    public float GetUseTime()
    {
        return f_time;
    }
    /// <summary>
    ///  设置最大时间
    /// </summary>
    /// <returns>单位秒</returns>
    public void SetMaxTime(float maxTime)
    {
        CountdownTime = maxTime;
    }
    /// <summary>
    /// 是否超时
    /// </summary>
    /// <returns></returns>
    public bool IsOutTime()
    {
        return f_time > CountdownTime;
    }
    public void WaitTime(float _time, VoidDelegate _timeOutCallBack)
    {
        timeoutCallback = _timeOutCallBack;
        StartCoroutine(waittime(_time));
    }
    IEnumerator waittime(float f)
    {
        yield return new WaitForSeconds(f);
        if (timeoutCallback != null)
            timeoutCallback();
    }
}
