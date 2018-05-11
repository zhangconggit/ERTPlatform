using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using IDataComponentDLL;
using System.Collections.Generic;
using CFramework;

public class CsScoreManager : MonoBehaviour
{
    #region 注册单例
    private CsScoreManager() { }
    static CsScoreManager _instance = null;
    public static CsScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CsScoreManager>();
                //GameObject go = FindObjectOfType<Canvas>().gameObject; // 

                if (FindObjectsOfType<CsScoreManager>().Length > 1)
                {
                    return _instance;
                }
                if (_instance == null)
                {
                    string instanceName = "CsCommit";
                    GameObject instanceGO = null;// = FindObjectOfType<Canvas>().gameObject.transform.Find(instanceName).gameObject;

                    if (FindObjectOfType<Canvas>().gameObject.transform.Find(instanceName) == null)
                    {
                        instanceGO = Instantiate(Resources.Load<GameObject>("CsCommit"));//new GameObject(instanceName);
                        instanceGO.name = instanceName;
                    }
                    else
                        instanceGO = FindObjectOfType<Canvas>().gameObject.transform.Find(instanceName).gameObject;
                    _instance = instanceGO.GetComponent<CsScoreManager>();
                    GameObject canvas = FindObjectOfType<Canvas>().gameObject;
                    instanceGO.transform.SetParent(canvas.transform);
                    instanceGO.transform.localPosition = new Vector3(0, 0, 0);
                    instanceGO.transform.localScale = new Vector3(1, 1, 1);
                    instanceGO.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                    instanceGO.transform.GetComponent<RectTransform>().offsetMax = new Vector2(1, 1);
                    //DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
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
    //public GameObject button;
    //Dictionary<string,float> _CurrentSaveScore;
     ParamDelegate<string> _CurrentProcess = null;
    public string scoreSheetCode = "100-0-01";
    bool isAutoGo = false;
    // Use this for initializatio
    void Start()
    {
        //text = transform.Find("Text").gameObject;
        //button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void hide()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 提交成绩
    /// </summary>
    /// <param name="score">成绩列表</param>
    /// <param name="process">回调方法,ParamType: string </param>
    /// <param name="autoGo">提交成功后，是否自动跳转到成绩显示界面</param>
    public void CommitScore(Dictionary<string,float> score, ParamDelegate<string> process, bool autoGo = false)
    {
        //button.SetActive(false);
        gameObject.SetActive(true);
       // text.GetComponent<Text>().text = "成绩提交中";
        //_CurrentSaveScore = score;
        _CurrentProcess = process;
        isAutoGo = autoGo;

        IDataComponent.GetInstance().addScoreSheetCode(scoreSheetCode);
        foreach (var item in score)
        {
            IDataComponent.GetInstance().addScoreItem(item.Key, item.Value);
        }
        IDataComponentDLL.IDataComponent.GetInstance().AddSubmitListen(IECommit);
        IDataComponentDLL.IDataComponent.GetInstance().sendScoreData();
        text.GetComponent<Text>().text = "成绩提交中...";
    }
    public void reCommit()
    {
        IDataComponentDLL.IDataComponent.GetInstance().sendScoreData();
    }
    void IECommit(IDataComponentDLL.ReturnDataStatus re)
    {
        

        if (re.Code == "0")
        {
            text.GetComponent<Text>().text = "成绩提交完成！";
            {
                _CurrentProcess("Ok");
                IDataComponentDLL.IDataComponent.GetInstance().resetScoreData();
            }
        }
        else
        {
            text.GetComponent<Text>().text ="由于网络原因，提交成绩出现异常";// "成绩提交失败:\n\t" + re.Desc;// 
            Debug.Log("成绩提交失败:\n\t" + re.Desc);
            _CurrentProcess("Error");
            // break;
        }
        
    }
}
