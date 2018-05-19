using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using CFramework;

/// <summary>
/// 步骤基类
/// </summary>
public class StepBase
{
    /// <summary>
    /// 导航栏
    /// </summary>
    string sid;//="";

    StepStatus state;
    /// <Summary>
    /// 镜头进入时的位置
    /// </Summary>
    public Vector3 cameraEnterPosition = Vector3.zero;

    /// <Summary>
    /// 镜头进入时的欧拉角
    /// </Summary>
    public Vector3 cameraEnterEuler = Vector3.zero;

    /// <summary>
    /// 评分点信息
    /// </summary>
    private List<string> scorepoints = new List<string>();

    /// <summary>
    /// 超时限制
    /// </summary>
    public float timeoutmax = 3f;
    /// <summary>
    /// 当前用时
    /// </summary>
    public float timeout = 0f;
    /// <summary>
    /// 步骤执行次数
    /// </summary>
    public int stepTimes = 0;

    UPageBase stepUI = null;
    private bool isAllowAddCode = false;

    /// <summary>
    /// 执行状态
    /// </summary>
    public StepStatus State
    {
        get { return state; }
        set { state = value; }
    }

    /// <summary>
    /// 步骤ID
    /// </summary>
    public string Sid
    {
        get { return sid; }
        set { sid = value; }
    }

    protected StepBase()
    {
        Sid = GetType().Name;
        stepUI = new UPageBase();
        stepUI.name = Sid + "UI";
        stepUI.rect = new Rect(0, 0, 1920, 1080);
        stepUI.SetActive(false);
    }

    protected T CreateUI<T>() where T : UPageBase
    {
        System.Type ty = typeof(T);
        object obj = MethodMaker.CreateObject(ty.FullName);
        UPageBase baseUI = (T)obj;
        baseUI.SetParent(stepUI);
        return (T)obj;
    }
    protected T CreateUI<T>(params object[] par) where T : UPageBase
    {
        System.Type ty = typeof(T);
        object obj = MethodMaker.CreateObject(ty.FullName, par);
        UPageBase baseUI = (T)obj;
        baseUI.SetParent(stepUI);
        return (T)obj;
    }
    
    /// <summary>
    /// 步骤执行前
    /// </summary>
    virtual public void EnterStep(float cameraTime = 1f)
    {
        ModelEventSystem.Instance.Add3DModelListenEvent(OnClickModel);
        //CGrade.addVideoStep(sid.ToString());
        State = StepStatus.indo;
        stepTimes++;

        ModelCtrol.Instance.CameraPath(cameraEnterPosition, cameraEnterEuler, cameraTime, () =>
        {
            CameraMoveFinished();
        }, false, Ease.Linear);
    }

    /// <summary>
    /// 步骤执行前镜头移动后
    /// </summary>
    virtual public void CameraMoveFinished()
    {
        State = StepStatus.doing;
        stepUI.SetActive(true);//显示所属UI
    }
    /// <summary>
    /// 执行当前步骤
    /// </summary>
    virtual public void StepUpdate()
    {
        //if (timeout > timeoutmax)
        //{
        //    StepEndState();
        //    timeout = -2;
        //}
        //else
        //    timeout += Time.deltaTime;
    }
    /// <summary>
    /// 后更新
    /// </summary>
    virtual public void LateUpdate()
    {

    }

    /// <summary>
    /// 鼠标点击模型
    /// </summary>
    virtual public void MouseClickModel(RaycastHit obj)
    {

    }

    /// <summary>
    /// 触发器进入
    /// </summary>
    virtual public void OnTriggerEnter(GameObject trigger, Collider collider) { }

    /// <summary>
    /// 触发器逗留
    /// </summary>
    virtual public void OnTriggerStay(GameObject trigger, Collider collider) { }
    /// <summary>
    /// 触发器离开
    /// </summary>
    virtual public void OnTriggerExit(GameObject trigger, Collider collider) { }

    virtual public void OnClickModel(RaycastHit obj) { }

    /// <summary>
    /// 步骤执行后
    /// </summary>
    virtual public void StepFinish()
    {
        //CGrade.addVideoStep(sid.ToString());
        stepUI.SetActive(false);
        ModelEventSystem.Instance.Remove3DModelListenEvent(OnClickModel);
        if (State != StepStatus.did && State != StepStatus.errorDid)
        {
            StepEndState();
        }
    }

    /// <summary>
    /// 步骤重置
    /// </summary>
    virtual public void StepReset() { State = StepStatus.undo; stepTimes = 0; timeout = 0; }

    /// <summary>
    /// 设置步骤开始状态
    /// </summary>
    virtual public void StepStartState() { }
    /// <summary>
    /// 设置步骤结束状态
    /// </summary>
    virtual public void StepEndState() { }
    /// <summary>
    /// 设置步骤结束状态
    /// </summary>
    virtual public void stepAutoFinish() { State = StepStatus.did; }

    /// <Summary>
    /// 收集此步骤所有评分编码
    /// </Summary>
    public List<string> CollectionScorePoint()
    {
        return scorepoints;
    }

    public void AddScoreItem(string pVariable, bool isForce = false)
    {
        if (!isAllowAddCode && !isForce)
        {
            Debug.Log("已禁止添加编码变量");
            return;
        }
        if (scorepoints.Contains(pVariable))
        {
            Debug.Log("评分编码变量已添加：" + pVariable);
            return;
        }
        scorepoints.Add(pVariable);
    }

    /// <Summary>
    /// 移除评分编码
    /// </Summary>
    public void RemoveScoreItem(string pVariable, bool isForce = false)
    {
        if (!this.isAllowAddCode && !isForce)
        {
            Debug.Log("已禁止添加编码变量");
            return;
        }
        if (scorepoints.Contains(pVariable))
        {
            scorepoints.Remove(pVariable);
            return;
        }
        Debug.Log("当前编码变量尚未添加：" + pVariable);
    }

    /// <Summary>
    /// 检查编码是否已添加
    /// </Summary>
    public bool IsExistCode(string pVariable)
    {
        return scorepoints.Contains(pVariable);
    }

    /// <Summary>
    /// 清除所有评分编码
    /// </Summary>
    public void ClearAllScoreItem()
    {
        scorepoints.Clear();
    }

    /// <summary>
    /// 结束添加评分
    /// </summary>
    /// <param name="isAlllow"></param>
    public void IsAllowAddCode(bool isAlllow)
    {
        isAllowAddCode = isAlllow;
    }
}