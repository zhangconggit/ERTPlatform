using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using CFramework;
using System.Collections.Generic;
using IDataComponentDLL;

public class CsScoreManager : MonoBehaviour
{
    #region 注册单例
    private CsScoreManager()
    {

    }
    static CsScoreManager _instance = null;
    public static CsScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CsScoreManager>();
                if (FindObjectsOfType<CsScoreManager>().Length > 1)
                {
                    return _instance;
                }
                if (_instance == null)
                {
                    string instanceName = "CsCommit";
                    GameObject instanceGO = null;

                    if (FindObjectOfType<Canvas>().gameObject.transform.Find(instanceName) == null)
                    {
                        instanceGO = new GameObject();//new GameObject(instanceName);
                        instanceGO.name = instanceName;
                        instanceGO.AddComponent<CsScoreManager>();
                    }
                    else
                        instanceGO = FindObjectOfType<Canvas>().gameObject.transform.Find(instanceName).gameObject;
                    _instance = instanceGO.GetComponent<CsScoreManager>();
                }
                else
                {
                    Debug.LogWarning("Already exist: " + _instance.name);
                }
            }
            return _instance;
        }
    }
    #endregion
    public GameObject text;
    UPageBase root;

    ParamDelegate<string> _CurrentProcess = null;

    /// <summary>
    /// 提交成绩完成
    /// </summary>
    public CEvent<string> onCommitFinish = new CEvent<string>();

    /// <summary>
    /// 点击确认按钮后
    /// </summary>
    public CEvent onClickOkButton = new CEvent();

    /// <summary>
    /// 评分表编码
    /// </summary>
    public string scoreSheetCode = "";

    List<_CLASS_SCORESHEET> scoreSheetData;

    Dictionary<string, List<string>> scoreVariableList = new Dictionary<string, List<string>>();
    Dictionary<string, float> scorelist = new Dictionary<string, float>();

    // Use this for initializatio
    void Start()
    {
        //DataContainer.Instance.getScoreSheet(CGlobal.productid, (data) =>
        //{
        //    scoreSheetData = data;
        //    scoreSheetCode = data[0].code;
        //});
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    bool bInit = false;

    /// <summary>
    /// 初始化
    /// </summary>
    public void init()
    {
        if (bInit)
            return;
        bInit = true;

        UImage back = new UImage(AnchoredPosition.center);
        back.name = "CommitBox";
        back.rect = new Rect(0, 0, 700, 400);
        back.color = Color.white;
        UText utext = new UText(AnchoredPosition.center);
        utext.SetParent(back);
        utext.rect = new Rect(0, 0, 700, 200);
        utext.baseText.alignment = TextAnchor.MiddleCenter;
        utext.baseText.color = Color.black;
        text = utext.gameObejct;
        root = back;
        root.gameObejct.SetActive(false);
        TimeManager.Instance.TimeOut.AddListener(CommitScore);
        onCommitFinish.AddListener((str) => { if (_CurrentProcess != null) _CurrentProcess.Invoke(str); });

        onClickOkButton.AddListener(() =>
        {

        });
    }

    /// <summary>
    /// 提交
    /// </summary>
    public void CommitScore()
    {
        TimeManager.Instance.StopTime();
        root.gameObejct.SetActive(true);
        scoreVariableList.Clear();
        foreach (var item in StepManager.Instance.stepList)
        {
            // scoreVariableList[item.Key] = item.Value.CollectionScorePoint();
        }
        //用时统计
        try
        {

            float langtime = 60;
            float smalltime = 30;
            float currenttime = TimeManager.Instance.GetUseTime();
            scoreVariableList[GetType().Name] = new List<string>();
            if (currenttime < smalltime)
            {
                scoreVariableList[GetType().Name].Add("TimeSmall");
            }
            else if (currenttime > langtime)
            {
                scoreVariableList[GetType().Name].Add("TimeMore");
            }
            else
            {
                scoreVariableList[GetType().Name].Add("TimeOk");
            }
        }
        catch
        {
        }
        int errorCount = StepManager.Instance.ErrorTimes;
        int notDoneStepCount = StepManager.Instance.GetNotDoStepNumber();
        float percent = (float)(errorCount + notDoneStepCount) / StepManager.Instance.GetAllStepNumber();
        if (percent > 0.3f)
            scoreVariableList[GetType().Name].Add("OperationMuchUnSkilled");
        else if (percent > 0.2f)
            scoreVariableList[GetType().Name].Add("OperationUnSkilled");
        else if (percent > 0.1f)
            scoreVariableList[GetType().Name].Add("OperationSoSo");
        else
            scoreVariableList[GetType().Name].Add("OperationSkilled");
        bool NoScore;
        //scorelist = DealScoreRecursion(scoreSheetData, scoreVariableList, true, out NoScore);

        CommitScore(scorelist, (str) => { Debug.Log(str + "提交成绩"); });
    }

    private Dictionary<string, float> DealScoreRecursion(Dictionary<string, List<string>> scoreVariableList, bool v, out bool noScore)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// 提交成绩
    /// </summary>
    /// <param name="score">成绩列表</param>
    /// <param name="process">回调方法,ParamType: string </param>
    public void CommitScore(Dictionary<string, float> score, ParamDelegate<string> process)
    {

        gameObject.SetActive(true);
        this._CurrentProcess = process;
        IDataComponent.GetInstance().addScoreSheetCode(this.scoreSheetCode);
        foreach (KeyValuePair<string, float> item in score)
        {
            IDataComponent.GetInstance().addScoreItem(item.Key, item.Value, 5f, true);
            Debug.Log("-----CFrame-->>>评分:" + item.Key + "," + item.Value);
        }
        IDataComponent.GetInstance().AddSubmitListen(new IDataComponent.ReturnDelegate(this.IECommit));
        IDataComponent.GetInstance().sendScoreData(Config.webIp, true);
        this.text.transform.SetAsLastSibling();
        root.transform.SetAsLastSibling();
        this.text.GetComponent<Text>().text += "成绩数据提交中...";
    }

    public void reCommit()
    {
        IDataComponent.GetInstance().sendScoreData(Config.webIp);
    }

    void IECommit(ReturnDataStatus re)
    {
        string text = "";
        if (re.Code == "0")
        {
            {
                text = "提交完成！";
            }
            IDataComponent.GetInstance().resetScoreData();
            this.onCommitFinish.Invoke("Ok");
        }
        else
        {
            text = "由于网络原因，提交成绩出现异常";
            Debug.Log("-----CFrame-->>>**成绩提交失败:\n\t" + re.Desc + "提交成绩数据");
            this.onCommitFinish.Invoke("Error");
        }
        this.root.gameObejct.SetActive(false);
        UMessageBox.Show("提示", text, "确定", () =>
        {
            this.onClickOkButton.Invoke();
        });
        IDataComponent.GetInstance().RemoveSubmitListen(IECommit);
    }
}
