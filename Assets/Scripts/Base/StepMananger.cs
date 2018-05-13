using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Xml;
using CFramework;

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
    public List<StepInfo> stepList = new List<StepInfo>();
    
    /// <summary>
    /// 导航图
    /// </summary>
    Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

    StepStatus currStepState;//当前步骤状态

    UNavigation m_navgation;

    /// <summary>
    /// 操作记录
    /// </summary>
    List<string> OperatingSequence = new List<string>();

    bool endStep;
    int crrWeiQitiZhi;
    //顺序错误次数
    int m_errorTime;

    public StepStatus CurrStepState
    {
        get { return currStepState; }
        set { currStepState = value; }
    }
   

    // Use this for initialization
    StepManager()
    {

        ModelEventSystem.Instance.Add3DModelListenEvent(ClickModels);

    }

    public void Init(List<StepInfo> _stepList)
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
        gEternalClass = new EternalClass();

        List<_CLASS_CatalogProperty> list = new List<_CLASS_CatalogProperty>();
        foreach (var item in map)
        {

            _CLASS_CatalogProperty obj = new _CLASS_CatalogProperty();
            obj.id = list.Count.ToString();
            obj.order = list.Count;
            obj.catalogName = item.Key;
            obj.catalogChildStepName = item.Value.ToArray();
            obj.catalogClickStepName = item.Value[0];
            list.Add(obj);
        }
        m_navgation = new UNavigation(list);

        IntoStep(stepList[0].stepName);
    }

    public void SetStepEnd()
    {
        if (pCurrentStep != null && pCurrentStep.pStepClass != null)
        {
            pCurrentStep.pStepClass.StepFinish();
        }
    }

    public int ErrorTimes
    {
        get
        {
            return m_errorTime;
        }
    }

    public int GetNotDoStepNumber()
    {
        int i = 0;
        foreach (var item in stepList)
        {
            if (item.pStepClass.State == StepStatus.undo)
                i++;
        }
        return i;
    }
    public int GetAllStepNumber()
    {
        return stepList.Count;
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
            pCurrentStep.pStepClass.StepFinish();
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
                info.StepReset();
            }
        }
        for (int i = stepsOrderList.IndexOf(startStep); i < stepsOrderList.IndexOf(endStep); i++)
        {
            getStepInfo(stepsOrderList[i]).pStepClass.stepAutoFinish();
        }
        int index = stepsOrderList.IndexOf(endStep) - 1;
        if (index < 0 || getStepInfo(stepsOrderList[index]).isAutoGo)
            nextStep = endStep;
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, endStep.ToString() + "_Wait");
        }
        if (p_StepNumber != "")
        {
            p_StepNumber = "";
        }

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
        if (findObje != null && findObje.pStepClass.State == StepStatus.undo)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void  StartStep()
    {
        if(stepList.Count > 0)
            IntoStep(stepList[0].stepName);
    }
    // Update is called once per frame
    public void Update()
    {
        //步骤跳转
        if (nextStep != "")
        {
            string str = IntoStep(nextStep);
            if (str != "")
            {
                Debug.Log("error:" + str);
                undoStepTips();
            }
            nextStep = "";
        }
        //
        if (pCurrentStep != null)
        {
            if (pCurrentStep.pStepClass.State == StepStatus.doing)
            {
                pCurrentStep.pStepClass.StepUpdate();
                gEternalClass.StepUpdate();
                pCurrentStep.pStepClass.LateUpdate();
                gEternalClass.LateUpdate();

            }
            if (pCurrentStep.pStepClass.State == StepStatus.undo)
            {
                pCurrentStep.pStepClass.EnterStep(pCurrentStep.intoTime);
                gEternalClass.EnterStep();

            }

            currStepState = pCurrentStep.pStepClass.State;

            if (currStepState == StepStatus.did || currStepState == StepStatus.errorDid)
            {
                pCurrentStep = SetStepEnd(pCurrentStep);

                if (pCurrentStep != null && pCurrentStep.isAutoGo)
                    nextStep = pCurrentStep.stepName;
                else
                {
                    undoStepTips();
                }
            }
        }
        else
        {
            gEternalClass.StepUpdate();
            gEternalClass.LateUpdate();
        }
    }

    /// <summary>
    /// 点击模型
    /// </summary>
    /// <param name="obj"></param>
    public void ClickModels(RaycastHit obj)
    {
        if (pCurrentStep != null)
        {
            if (pCurrentStep.pStepClass.State == StepStatus.doing)
            {
                pCurrentStep.pStepClass.MouseClickModel(obj);
            }
        }
        else
        {
            foreach (var item in stepList)
            {
                if (item.triggerModelName == obj.collider.name)
                {
                    nextStep = item.stepName;
                    break;
                }
            }
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
        if (name == null || name == string.Empty)
        {
            return;
        }
        if (!map.ContainsKey(name))
        {
            map[name] = new List<string>();
        }
        map[name].Add(step);
    }

    /// <summary>
    /// 取得应该做的步骤
    /// </summary>
    /// <returns></returns>
    public string GetNeedDoStep()
    {
        foreach (var item in stepList)
        {
            if (item.pStepClass.State == StepStatus.undo)
            {
                bool bl = false;
                if (item.cannotDoStep.Count > 0)
                {
                    //后限制前
                    foreach (string name in item.cannotDoStep)
                    {
                        if (checkStepIsDid(name))
                        {
                            bl = true;//后置做过了
                            break;
                        }
                    }
                }
                if (!bl)
                {
                    return item.stepName;
                }
            }
        }
        return "";
    }

    /// <summary>
    /// 没有做的步骤提示 限制除外
    /// </summary>
    /// <returns>true 存在</returns>
    bool undoStepTips()
    {
        string str = GetNeedDoStep();
        if (str != null)
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, str + "_Wait");
            return true;
        }
        else
        {
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "allEnd");
            return false;
        }

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
    public string IntoStep(string steptype)
    {
        //返回 重置
        Debug.Log("into step " + steptype);

        if (CGlobal.mode == OperationMode.Exam)
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
                        //VoiceControlScript.Instance.AudioPlay( AudioStyle.Attentions,item + "_Wait");
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
                        //VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions,"NotDoMe");
                        return "已完成后置步骤";
                    }
                }
            }
        }
        if (CGlobal.mode == OperationMode.Practice)
        {
            if (steptype != GetNeedDoStep())
            {
                //undoStepTips();
                return "步骤顺序错误";
            }
        }
        //////  新步骤的处理
        Debug.Log("new Step:" + steptype);
        p_StepNumber = steptype;
        Debug.Log("update after Step:" + p_StepNumber);
        pCurrentStep = getStepInfo(p_StepNumber);//取得步骤信息

        if (pCurrentStep != null)
        {
            OperatingSequence.Add(p_StepNumber);
            m_navgation.OutStep(p_StepNumber);
        }
        return "";
    }

    /// <summary>
    /// 结束步骤 返回下一个步骤
    /// </summary>
    /// <param name="string">步骤名称</param>
    /// <return type="string">反馈信息</return>
    public StepInfo SetStepEnd(StepInfo steptype)
    {
        //清空处理
        if (steptype != null)
        {
            Debug.Log("Step end:" + steptype.stepName);
            steptype.pStepClass.StepFinish();
            gEternalClass.StepFinish();
            m_navgation.OutStep(steptype.stepName);

            int i = 0;
            for (; i < stepList.Count - 1; i++)
            {
                if (stepList[i].stepName == pCurrentStep.stepName)
                {
                    break;
                }
            }
            if (i != stepList.Count - 1)
                return stepList[i];

        }
        return null;
    }
    /// <summary>
    /// 结束处理
    /// </summary>
    public void setOpreateEnd()
    {
        //结束处理
        if (CGlobal.mode == OperationMode.Exam)
            return;
        if (!endStep)
            endStep = true;
        else
            return;
        if (pCurrentStep != null && pCurrentStep.pStepClass != null)
        {
            pCurrentStep.pStepClass.StepFinish();
        }


        //顺序错误
        m_errorTime = m_errorTime * stepList.Count;
        if (m_errorTime > 10)
            CGrade.Input("12104");
        else if (m_errorTime > 7)
            CGrade.Input("12103");
        else if (m_errorTime > 3)
            CGrade.Input("12102");
        else
            CGrade.Input("12101");
        //操作时间判断
        int time_s = (int)TimeManager.Instance.GetUseTime();
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
        Debug.Log("Operation Error Times:" + m_errorTime);
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
    public string stepName="";
    /// <summary>
    /// 是否自动跳转
    /// </summary>
    public bool isAutoGo=false;

    //如果不是自动进入,则需要模型触发
    public string triggerModelName="";

    /// <summary>
    /// 进入步骤的时间
    /// </summary>
    public float intoTime=0;

    /// <summary>
    /// 必须完成的步骤
    /// </summary>
    public List<string> mustDoStep=new List<string>();

    /// <summary>
    /// 不能做的步骤
    /// </summary>
    public List<string> cannotDoStep=new List<string>();

    /// <summary>
    /// 步骤功能对象
    /// </summary>
    public StepBase pStepClass = null;

    /// <summary>
    /// 导航栏
    /// </summary>
    public string navigation = "";
}

