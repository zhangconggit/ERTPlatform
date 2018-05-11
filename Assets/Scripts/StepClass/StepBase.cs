using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

    /// <summary>
    /// 步骤基类
    /// </summary>
    public class StepBase
    {
        /// <summary>
        /// 步骤信息
        /// </summary>
        public StepInfo stepInfo = null;
        /// <summary>
        /// 导航栏
        /// </summary>
        public string sNavigation = "";
        string sid;//="";
        string outInfo;
        StepStatus state;
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
        /// <summary>
        /// 步骤编码
        /// </summary>
        public string stepNumber = "";
        /// <summary>
        /// 执行状态
        /// </summary>
        public StepStatus State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 反馈信息
        /// </summary>
        public string OutInfo
        {
            get { return outInfo; }
            set { outInfo = value; }
        }
        /// <summary>
        /// 步骤ID
        /// </summary>
        public string Sid
        {
            get { return sid; }
            set { sid = value; }
        }
        /// <summary>
        /// 步骤执行前
        /// </summary>
        virtual public void stepBefore()
        {
            CGrade.addVideoStep(sid.ToString());
            State = StepStatus.doing;
            stepTimes++;
            if (GameObject.Find("Canvas").transform.Find("Knowledge_Part") != null)
            {
                if (GlobalClass.g_OperatorSchema == OperatorSchema.examModel)
                {
                    if (GameObject.Find("Canvas").transform.Find("Knowledge_Part").gameObject.activeSelf)
                        GameObject.Find("Canvas").transform.Find("Knowledge_Part").gameObject.SetActive(false);
                }

            }
        }
        /// <summary>
        /// 步骤执行前镜头移动后
        /// </summary>
        virtual public void stepMoveAfter()
        {
            if (ObjectManager.Instance != null)
            {
                if (ObjectManager.Instance.GetObject(sid.ToString(), "UI" + sid.ToString()) != null)
                    ObjectManager.Instance.GetObject(sid.ToString(), "UI" + sid.ToString()).SetActive(true);//显示所属UI
            }
        }
        /// <summary>
        /// 执行当前步骤
        /// </summary>
        virtual public void stepRunner()
        {
            if (timeout > timeoutmax)
            {
                stepSetEnd();
                timeout = -2;
            }
            else
                timeout += Time.deltaTime;
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

        /// <summary>
        /// 步骤执行后
        /// </summary>
        virtual public void stepAfter()
        {
            CGrade.addVideoStep(sid.ToString());
            if (ObjectManager.Instance != null)
            {
                if (ObjectManager.Instance.GetObject(sid.ToString(), "UI" + sid.ToString()) != null)
                    ObjectManager.Instance.GetObject(sid.ToString(), "UI" + sid.ToString()).SetActive(false);
            }
            if (GlobalClass.isClickedLookButton)
            {
                stepSetEnd();
                GlobalClass.isClickedLookButton = false;
            }
            if (State != StepStatus.did && State != StepStatus.errorDid)
            {
                stepSetEnd();
            }
        }

        /// <summary>
        /// 步骤重置
        /// </summary>
        virtual public void stepReSet() { State = StepStatus.undo; stepTimes = 0; timeout = 0; }
        /// <summary>
        /// 设置步骤结束状态
        /// </summary>
        virtual public void stepSetEnd() { }
        /// <summary>
        /// 设置步骤结束状态
        /// </summary>
        virtual public void stepAutoFinish() { State = StepStatus.did; }
    }