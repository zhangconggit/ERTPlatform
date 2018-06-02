using UnityEngine;
using System.Collections;
using System.Collections.Generic;//C# 包中的类
using System.Xml;
using System.Linq;
using System;


public class CGrade
{
    public static bool isInit = false;
    private static int videoStepNumber;
    public static string GradeTableId = null;
    public static bool isCommitScore = false;
    /// <summary>
    /// 评分表
    /// </summary>
    private static Dictionary<string, float> sGradingRule = new Dictionary<string, float>();
    /// <summary>
    /// 内码映射 内码-编码
    /// </summary>
    private static Dictionary<string, string> sInternalCode = new Dictionary<string, string>();
    /// <summary>
    /// 评分记录表
    /// </summary>
    private static List<string> GradeList = new List<string>();
    /// <summary>
    /// 内码表
    /// </summary>
    private static List<string> InternalList = new List<string>();
    /// <summary>
    /// 评分记录表
    /// </summary>
    private static List<string> VedioStep = new List<string>();
    /// <summary>
    /// 评分记录表 带分值
    /// </summary>
    private static Dictionary<string, float> DicGradeList = new Dictionary<string, float>();
    /// <summary>
    /// 得分
    /// </summary>
    private static float ResultsScore = 0;
    //private static string OperatorStartTime;
    /// <summary>
    /// 加载评分表
    /// </summary>
    public static void LoadGradingRuleFromLocalXml()
    {
        sGradingRule.Clear();
#if UNITY_WEBPLAYER
        GradeTableId = DB.getInstance().getConfigValue("在线");
#else
        GradeTableId = DB.getInstance().getConfigValue("智能");
#endif
        sGradingRule = DB.getInstance().getGradingRule(GradeTableId);
        Debug.Log("sGradingRule:");
        //foreach (var item in sGradingRule)
        //{
        //    Debug.Log(item);
        //}
        sInternalCode = DB.getInstance().getGradingRuleSub(GradeTableId);
        Debug.Log("sInternalCode:");
        //foreach (var item in sInternalCode)
        //{
        //    Debug.Log(item);
        //}
        isInit = true;
    }
    /// <summary>
    /// 操作记录
    /// </summary>
    private static List<string> GradeListAll = new List<string>();
    /// <summary>
    /// 取得操作记录
    /// </summary>
    /// <returns>列表</returns>
    public static List<string> getOperateRecord()
    {
        return GradeListAll;
    }
    /// <summary>
    /// 取得得分列表 补充未评分项
    /// </summary>
    /// <returns>列表</returns>
    public static List<string> getGradList()
    {
        GradeList.Clear();
        InternalList = MyCommon.RankingMethod(ref InternalList);
        List<string> gradeList = new List<string>();
        Debug.Log("内码表：");
        foreach (var item in InternalList)
        {
            //Debug.Log(item);
            gradeList.Add(sInternalCode[item]);
            // Debug.Log("编码："+ sInternalCode[item]);
        }
        string str = "";
        for (int i = InternalList.Count - 1; i >= 0; i--)
        {
            if (int.Parse(InternalList[i].Substring(0, 3)) < 121)
            {
                str = InternalList[i].Substring(0, 3) + "AA";
                break;
            }
        }
        Debug.Log("插入");
        foreach (var item in sInternalCode)
        {
            if (InternalList.Contains(item.Key))
            {
                if (item.Value.IndexOf('A') < 0 && item.Value.IndexOf('B') < 0)
                    if (!checkExist(GradeList, item.Value, 9) || item.Value.Substring(3, 6) == "102209")
                        GradeList.Add(item.Value);
            }
            else
            {
                if (item.Value.IndexOf('C') > 0 && item.Key.CompareTo(str) > 0)
                {
                    if (!checkExist(GradeList, item.Value, 6))
                        GradeList.Add(item.Value);
                }
                else if ((item.Value.IndexOf('D') > 0 || item.Value == "2122003911" || item.Value == "2121103761" || item.Value == "2121163861") && item.Key.CompareTo(str) <= 0) //漏操作项
                {
                    if (item.Value == "212102204D")
                    {
                        if (!checkExist(gradeList, "2121022050", 9) && !checkExist(gradeList, "2121022060", 9) && !checkExist(gradeList, "2121022070", 9))
                        {
                            GradeList.Add(item.Value);
                        }
                    }
                    else if (item.Value == "212103213D")
                    {
                        if (!checkExist(gradeList, "2121032140", 9) && !checkExist(gradeList, "2121032150", 9) && !checkExist(gradeList, "2121032160", 9))
                        {
                            GradeList.Add(item.Value);
                        }
                    }
                    else if (item.Value == "212201352D")
                    {
                        if (!checkExist(gradeList, "2122013530", 9)&& checkExist(gradeList, "2122013530", 6))
                        {
                            GradeList.Add(item.Value);
                        }
                    }
                    else if (item.Value.Substring(0, 6) == "212115" && checkExist(gradeList, "212115378C", 10))
                    {

                    }
                    else if (!checkExist(gradeList, item.Value, 9))
                    {
                        GradeList.Add(item.Value);
                    }
                } 
                else if(item.Value.IndexOf('C') > 0 && (item.Value.Substring(0, 6) == "212113" || item.Value.Substring(0, 6) == "212104" || item.Value.Substring(0, 6) == "212201" || item.Value.Substring(0, 6) == "212114"))//排空气体的特殊处理 术前评估 检测灌肠器 润滑钢管
                {
                    if (!checkExist(gradeList, item.Value, 6) && item.Value != "212113370C")
                    {
                        GradeList.Add(item.Value);
                    }
                    //排空气体，关闭引流器 当没有排空气体的编码是，212113370C不出现，当没有212113370编码时，212113370C出现
                    if (item.Value == "212113370C" && !checkExist(gradeList, item.Value, 9) && checkExist(gradeList, item.Value, 6))
                    {
                        GradeList.Add(item.Value);
                    }
                }
            }
        }
        Debug.Log("转换");
        foreach (var item in GradeList)
        {
            DicGradeList[item] = sGradingRule[item];
        }
        Debug.Log("返回");
        return GradeList;
    }
    /// <summary>
    /// 重置评分记录
    /// </summary>
    public static void reSetGrade()
    {
        InternalList.Clear();
        GradeListAll.Clear();
        ResultsScore = 0;
        DicGradeList.Clear();
        GradeList.Clear();
        //IDataComponent.GetInstance().addStartTime();
        VedioStep.Clear();
        videoStepNumber = -1;
        isCommitScore = false;
#if NO //放弃该方案
        IDataComponentDLL.IDataComponent.GetInstance().startRecordVideo(); 
#endif
    }
    /// <summary>
    /// 录入成绩编码
    /// </summary>
    /// <param name="id">成绩编码</param>
    public static void Input(string id, bool noCheck = false)
    {
        if (noCheck)
        {
            if (InternalList.IndexOf(id) < 0)
            {
                InternalList.Add(id);
            }
        }
        else //if (StepManager.StepClass.Instance.getStep() == StepManager."" || StepManager.StepClass.Instance.getStepInfo(StepManager.StepClass.Instance.getStep()).pStepClass.stepTimes == 1)
        {
            if (InternalList.IndexOf(id) < 0)
            {
                InternalList.Add(id);
            }
        }
    }
    /// <summary>
    /// 移除入成绩编码
    /// </summary>
    /// <param name="id">成绩编码 末尾*代替任意值</param>
    public static void Remove(string id)//,bool isNew=false
    {
        int k = id.IndexOf('*');
        for (int i = InternalList.Count - 1; i >= 0; i--)
        {
            if (k < 0)
            {
                if (InternalList[i] == id)
                {
                    InternalList.RemoveAt(i);
                }
            }
            else if (InternalList[i].Substring(0, k) == id.Substring(0, k))
            {
                InternalList.RemoveAt(i);
            }
        }
    }
    /// <summary>
    /// 检查成绩编码是否存在
    /// </summary>
    /// <param name="id">成绩编码 末尾*代替任意值</param>
    public static bool Check(string id)
    {
        bool _bl = false;
        int k = id.IndexOf('*');
        for (int i = InternalList.Count - 1; i >= 0; i--)
        {
            if (k < 0)
            {
                if (InternalList[i] == id)
                {
                    _bl = true;
                }
            }
            else if (InternalList[i].Substring(0, k) == id.Substring(0, k))
            {
                _bl = true;
            }
        }
        return _bl;
        //string str = id;
    }
    
    /// <summary>
    /// 计算取得成绩
    /// </summary>
    /// <param name="id">成绩</param>
    public static float getResults()
    {
        return ResultsScore;
    }
    private static bool checkExist(List<string> list, string id, int _size = 6)
    {
        string did = id.Substring(0, _size);//取出评分点
        foreach (string sid in list)
        {
            if (sid.Substring(0, _size) == did)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 取得评分点位置
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="id">评分ID</param>
    /// <returns>位置</returns>
    private static int getPointIndex(List<string> list, string id)
    {
        string did = id.Substring(0, 9);//取出评分点
        for (int i = 0; i < list.Count; i++)///string sid in list)
        {
            if (list[i].Substring(0, 9) == did)
                return i;
        }
        return -1;
    }
    /// <summary>
    /// 添加步骤时间
    /// </summary>
    /// <param name="name"></param>
    public static void addVideoStep(string name)
    {
    }
    public static void commitScore()
    {
        if (isCommitScore)
            return;
        getGradList();//补充未操作项
        
        //IDataComponent.GetInstance().addScoreSheetCode(GradeTableId);
        foreach (string str in DicGradeList.Keys)
        {
            //IDataComponent.GetInstance().addScoreItem(str, DicGradeList[str]);
        }
        StepManager.Instance.DebugOperationSeq();//打印顺序错误信息
        CsScoreManager.Instance.CommitScore();
    }
}