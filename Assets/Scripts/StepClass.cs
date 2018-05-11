using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Xml;

/// <summary>
/// 步骤状态
/// </summary>
public enum StepStatus
{
    undo = 0, //未执行
    indo,//进入步骤
    doing, //正在执行
    did, //正确执行
    errorDid //错误结束
}

/// <summary>
/// 步骤控制类
/// </summary>
public class StepManager
{
    /// <summary>
    /// 将跳转的下一个步骤
    /// </summary>
    public string nextStep = "login";

    string p_StepNumber = "";

    static StepManager stepclass = null;
    List<string> stepsOrderList = new List<string>();
    public static StepManager Instance
    {
        get
        {
            if (stepclass == null)
            {
                stepclass = new StepManager();
            }
            return stepclass;
        }
    }
    public static void reCreate()
    {
        stepclass = new StepManager();
    }
    EternalClass gEternalClass;
    // StepBase p_StepClass = null;
    bool isFirst = false;

    StepInfo pCurrentStep = null;
    /// <summary>
    /// 等待镜头移动
    /// </summary>
    public bool WaitCameraMove = false;
    /// <summary>
    /// 步骤信息列表
    /// </summary>
    List<StepInfo> stepList = new List<StepInfo>();

    /// <summary>
    /// 导航列表
    /// </summary>
    Dictionary<string, string> stepNavigation = new Dictionary<string, string>();//管理步骤导航
    StepStatus currStepState;//当前步骤状态
                             /// <summary>
                             /// 操作记录
                             /// </summary>
    List<string> OperatingSequence = new List<string>();

    bool endStep;
    int crrWeiQitiZhi;
    //顺序错误次数
    int errorTime;

    public StepStatus CurrStepState
    {
        get { return currStepState; }
        set { currStepState = value; }
    }
    public void SetStepEnd()
    {
        if (pCurrentStep != null && pCurrentStep.pStepClass != null)
        {
            pCurrentStep.pStepClass.stepSetEnd();
        }
    }
    /// <summary>
    /// 跳转步骤
    /// </summary>
    /// <param name="startStep">开始步骤，将后面所有的步骤ReSet处理</param>
    /// <param name="endStep">结束步骤，从startStep开始做StepAutoFinish处理</param>
    public void SetToStepEnd(string startStep, string endStep)
    {
        if (pCurrentStep != null)
        {
            pCurrentStep.pStepClass.stepAfter();
            pCurrentStep.pStepClass.State = StepStatus.did;
            pCurrentStep = null;
        }
        StepInfo stepInfo = null;
        stepInfo = getStepInfo(startStep);
        for (int i = stepsOrderList.Count; i > stepsOrderList.IndexOf(startStep); i--)
        {
            StepBase info = getStepInfo(stepsOrderList[i - 1]).pStepClass;
            if (info.State != StepStatus.undo)
            {
                info.stepReSet();
                UpdateNavigation(info.Sid, false);
            }
        }
        for (int i = stepsOrderList.IndexOf(startStep); i < stepsOrderList.IndexOf(endStep); i++)
        {
            getStepInfo(stepsOrderList[i]).pStepClass.stepAutoFinish();
            UpdateNavigation(stepsOrderList[i], false);
        }
        int index = stepsOrderList.IndexOf(endStep) - 1;
        if (index < 0 || getStepInfo(stepsOrderList[index]).isAutoGo)
            nextStep = endStep;
        else
        {
            //Speak.Instance.playAudio(endStep.ToString() + "Wait");
        }
        if (p_StepNumber != "")
        {
            UpdateNavigation(p_StepNumber, false);
            p_StepNumber = "";
        }

    }
    // Use this for initialization
    StepManager()
    {
        gEternalClass = new EternalClass();
        Init();
    }
    /// <summary>
    /// 类型查找
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    bool IsSubClassOf(System.Type type, System.Type baseType)
    {
        var b = type.BaseType;
        while (b != null)
        {
            if (b.Equals(baseType))
            {
                return true;
            }
            b = b.BaseType;
        }
        return false;
    }
    void Init()
    {

        string projectName = fileHelper.ReadIni("Project", "name", Config.ConfigPath);

        DataLoad.Instance().LoadProject(projectName, (_stepList) =>
        {
            stepList = _stepList;
            foreach (var type in stepList)
            {
                //Debug.Log(type);
                StepBase obj = MethodMaker.CreateObject(type.stepName) as StepBase;
                if (obj != null)
                {
                    type.pStepClass = obj;
                    setStepNavigation(type.navigation, type.stepName);
                }
            }
        });
        
    }
    
    /// <summary>
    /// 一直工作的方法
    /// </summary>
    public void Working()
    {
        gEternalClass.stepRunner();
    }
    public string GetLastStep()
    {
        if (OperatingSequence.Count > 0)
            return OperatingSequence[OperatingSequence.Count - 1];
        return "";
    }
    public void AddStepSequence(string step)
    {
        OperatingSequence.Add(step);
    }
    /// <summary>
    /// 取得步骤信息
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public StepInfo getStepInfo(string step)
    {
        StepInfo findObje = stepList.Find(name =>
        {
            if (name.stepName == step)
                return true;
            else
                return false;
        });
        return findObje;
    }
    /// <summary>
    /// 检查步骤是否做完
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public bool checkStepFlished(string step)
    {
        StepInfo findObje = stepList.Find(name =>
        {
            if (name.stepName == step)
                return true;
            else
                return false;
        });
        if (findObje == null || findObje.pStepClass == null || findObje.pStepClass.State == StepStatus.did)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 检查步骤是否做过
    /// </summary>
    /// <param name="step"></param>
    /// <returns></returns>
    public bool checkStepIsDid(string step)
    {
        StepInfo findObje = stepList.Find(name =>
        {
            if (name.stepName == step)
                return true;
            else
                return false;
        });
        if (findObje != null && findObje.pStepClass != null && findObje.pStepClass.State == StepStatus.undo)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    // Update is called once per frame
    public void Update()
    {
        //步骤跳转
        if (nextStep != "")
        {
            string re = setStep(nextStep);
            if (re != "跳转场景")
                nextStep = "";
            else if (re != "")
            {
                Debug.Log("setStep error :" + re);
            }
        }
        //
        if (pCurrentStep != null)
        {
            if (!WaitCameraMove)
            {
                if (isFirst)
                {
                    pCurrentStep.pStepClass.stepMoveAfter();
                    gEternalClass.stepMoveAfter();
                    isFirst = false;
                    //Speak.Instance.playAudio(pCurrentStep.pStepClass.Sid.ToString());
                }
                pCurrentStep.pStepClass.stepRunner();
            }

            currStepState = pCurrentStep.pStepClass.State;
        }
    }
    /// <summary>
    /// 触发器进入
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(GameObject trigger, Collider collider)
    {
        gEternalClass.OnTriggerEnter(trigger, collider);
        if (pCurrentStep != null)
        {
            pCurrentStep.pStepClass.OnTriggerEnter(trigger, collider);
        }
    }
    /// <summary>
    /// 触发器逗留
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerStay(GameObject trigger, Collider collider)
    {
        gEternalClass.OnTriggerStay(trigger, collider);
        if (pCurrentStep != null)
        {
            pCurrentStep.pStepClass.OnTriggerStay(trigger, collider);
        }
    }
    /// <summary>
    /// 触发器离开
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(GameObject trigger, Collider collider)
    {
        gEternalClass.OnTriggerExit(trigger, collider);
        if (pCurrentStep != null)
        {
            pCurrentStep.pStepClass.OnTriggerExit(trigger, collider);
        }
    }
    /// <summary>
    /// 设置导航步骤
    /// </summary>
    /// <param name="name">导航栏步骤名字</param>
    /// <param name="step">执行步骤</param>
    public void setStepNavigation(string name, string step)
    {
        if (stepNavigation.ContainsKey(step))
        {

        }
        else
        {
            stepNavigation[step] = name;
        }
    }
    /// <summary>
    /// 更新导航栏状态
    /// </summary>
    /// <param name="step">步骤名称</param>
    /// <param name="running">正在执行</param>
    public void UpdateNavigation(string step, bool running)
    {
        if (SceneManager.GetActiveScene().name != "bingfang")
            return;
        if (stepNavigation.ContainsKey(step))
        {
            string name = stepNavigation[step];
            if (running)
                ObjectManager.Instance.GetObject("catalog", "UIpublic").GetComponent<progressControl.CProgressControl>().twinklingStepIconByDesc(name);
            else
            {
                foreach (string st in stepNavigation.Keys)
                {
                    if (stepNavigation[st] == name)
                    {
                        if (!checkStepIsDid(st))
                        {
                            ObjectManager.Instance.GetObject("catalog", "UIpublic").GetComponent<progressControl.CProgressControl>().setStepSourceByDesc(name);
                            return;
                        }
                    }
                }
                ObjectManager.Instance.GetObject("catalog", "UIpublic").GetComponent<progressControl.CProgressControl>().setStepHightLightByDesc(name);
            }
        }
    }
    /// <summary>
    /// 没有做的步骤提示 限制除外
    /// </summary>
    /// <returns>true 存在</returns>
    bool undoStepTips()
    {
        int step = 0;
        //while (step <=10)
        //{

        //    //步骤以执行 或 后置步骤以执行
        //    if (getStepInfo(step) == null || getStepInfo(step).pStepClass == null || getStepInfo(step).pStepClass.State == StepStatus.did )// || getStepInfo(step).pStepClass.State == StepStatus.errorDid || checkStepIsDid(getStepInfo(step).nextStep))
        //    {
        //        step++;
        //    }
        //    else
        //    {
        //        bool bl = false;
        //        //限制步骤  
        //        if (getStepInfo(step) != null && getStepInfo(step).cannotDoStep.Count > 0)
        //        {
        //            //后限制前
        //            foreach (string item in getStepInfo(step).cannotDoStep)
        //            {
        //                if (checkStepIsDid(item))
        //                {
        //                    step++;
        //                    bl = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if(!bl)
        //        {
        //            Speak.Instance.playAudio(step.ToString() + "Wait");
        //            return true;
        //        }
        //    }
        //}
        if (GlobalClass.g_OperatorSchema != OperatorSchema.teachModel)
        {
            //Speak.Instance.playAudio("allEnd");//完成
        }
        else
        {
            //Speak.Instance.playAudio("allEndt");//完成
        }
        return false;
    }
    public string getStep()
    {
        return p_StepNumber;
    }
    /// <summary>
    /// 修改当前步骤
    /// </summary>
    /// <param name="string">步骤名称</param>
    /// <return type="string">反馈信息</return>
    public string setStep(string steptype)
    {
        Debug.Log("string = " + steptype);
        //返回 重置

        if (GlobalClass.g_OperatorSchema == OperatorSchema.examModel && steptype == "cannula")
        {

        }
        else
        {
            //前置检查
            if (getStepInfo(steptype) != null && getStepInfo(steptype).mustDoStep.Count > 0)
            {
                //有前置限制
                foreach (string item in getStepInfo(steptype).mustDoStep)
                {
                    if (!checkStepFlished(item))
                    {
                        //Speak.Instance.playAudio(item.ToString() + "Undo");
                        return "未正确操作前置步骤";
                    }
                }
            }
            //限制步骤  
            if (getStepInfo(steptype) != null && getStepInfo(steptype).cannotDoStep.Count > 0)
            {
                //后限制前
                foreach (string item in getStepInfo(steptype).cannotDoStep)
                {
                    if (checkStepIsDid(item))
                    {
                        //Speak.Instance.playAudio(item.ToString() + "Did");
                        return "已完成后置步骤";
                    }
                }
            }
        }
        //清空处理
        string returnInfo = "";
        if (p_StepNumber != "")
        {
            Debug.Log("Step end:" + p_StepNumber);
            pCurrentStep.pStepClass.stepAfter();
            gEternalClass.stepAfter();
            UpdateNavigation(p_StepNumber, false);
            //步骤正常结束的下一步语音提示
            if (!pCurrentStep.isAutoGo && steptype == "")
                undoStepTips();
            returnInfo = pCurrentStep.pStepClass.OutInfo;
        }
        if (GlobalClass.g_OperatorSchema == OperatorSchema.practiceModel)
        {
            if (steptype != "")
            {

                if (true)
                {
                    undoStepTips();
                    return "步骤顺序错误";
                }
                else
                {
                    undoStepTips();
                    return "步骤顺序错误";
                }
            }
        }
        //////  新步骤的处理
        Debug.Log("new Step:" + steptype);
        p_StepNumber = steptype;
        if (p_StepNumber == "")
        {
            if (pCurrentStep != null && pCurrentStep.pStepClass != null && pCurrentStep.pStepClass.State == StepStatus.did && pCurrentStep.isAutoGo)
            {
                // p_StepNumber = pCurrentStep.nextStep;
            }
        }
        Debug.Log("update after Step:" + p_StepNumber);
        pCurrentStep = getStepInfo(p_StepNumber);//取得步骤信息

        if (pCurrentStep != null)
        {
            OperatingSequence.Add(p_StepNumber);
            //操作顺序检查
            //顺序错误

            isFirst = true;
            UpdateNavigation(p_StepNumber, true);
            pCurrentStep.pStepClass.stepBefore();
            gEternalClass.stepBefore();
            //步骤前动画

        }
        return returnInfo;
    }


    /// <summary>
    /// 结束处理
    /// </summary>
    public void setOpreateEnd()
    {
        //结束处理
        if (GlobalClass.g_OperatorSchema == OperatorSchema.teachModel)
            return;
        if (!endStep)
            endStep = true;
        else
            return;
        if (pCurrentStep != null && pCurrentStep.pStepClass != null)
        {
            pCurrentStep.pStepClass.stepAfter();
        }


        //顺序错误
        errorTime = errorTime * stepList.Count;
        if (errorTime > 10)
            CGrade.Input("12104");
        else if (errorTime > 7)
            CGrade.Input("12103");
        else if (errorTime > 3)
            CGrade.Input("12102");
        else
            CGrade.Input("12101");
        //操作时间判断
        int time_s = GlobalClass.getOperatorTime();
        if (time_s > 60 * 7)
        {
            CGrade.Input("12203");
        }
        else if (time_s < 60 * 5)
            CGrade.Input("12202");
        else
            CGrade.Input("12201");


        List<string> score = CGrade.getGradList();

        Debug.Log("成绩列表：");
        foreach (string str in score)
        {
            Debug.Log(str);
        }
        Debug.Log("得分：" + CGrade.getResults());


        CGrade.commitScore();//提交成绩
    }
    public void DebugOperationSeq()
    {
        Debug.Log("Operation Error Times:" + errorTime);
        Debug.Log("Operation sequence:");
        foreach (string str in OperatingSequence)
        {
            Debug.Log(str);
        }

    }
}
/// <summary>
/// 步骤信息 
/// </summary>
[System.Serializable]
public class StepInfo
{
    /// <summary>
    /// 当前步骤
    /// </summary>
    public string stepName;
    /// <summary>
    /// 是否自动跳转
    /// </summary>
    public bool isAutoGo;

    public float intoTime;
    /// <summary>
    /// 必须完成的步骤
    /// </summary>
    public List<string> mustDoStep;
    /// <summary>
    /// 不能做的步骤
    /// </summary>
    public List<string> cannotDoStep;
    /// <summary>
    /// 步骤功能对象
    /// </summary>
    public StepBase pStepClass;

    public string navigation;
}

