#define WIN_PC2
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
//using System.Net.NetworkInformation;

namespace IDataComponentDLL
{
    #region 提交成绩结构

    /// <summary>
    /// 步骤信息数组
    /// </summary>
    class SStepInformation
    {
        /// <summary>
        /// 步骤ID
        /// </summary>
        private string stepID;
        public string StepID
        {
            get { return stepID; }
            set { stepID = value; }
        }

        /// <summary>
        /// 步骤描述
        /// </summary>
        private string stepDesc;
        public string StepDesc
        {
            get { return stepDesc; }
            set { stepDesc = value; }
        }

        /// <summary>
        /// 步骤开始时间
        /// </summary>
        private string stepStartTime;
        public string StepStartTime
        {
            get { return stepStartTime; }
            set { stepStartTime = value; }
        }

        /// <summary>
        /// 步骤结束时间
        /// </summary>
        private string stepEndTime;
        public string StepEndTime
        {
            get { return stepEndTime; }
            set { stepEndTime = value; }
        }

        private List<SItemInfomation> itemInfomationList = new List<SItemInfomation>();

        /// <summary>
        /// 评分表信息
        /// </summary>
        public List<SItemInfomation> ItemInfomationList
        {
            get { return itemInfomationList; }
        }
    }

    class SItemInfomation
    {
        /// <summary>
        /// 评分时的时间
        /// </summary>
        private string itemTime;
        public string ItemTime
        {
            get { return itemTime; }
            set { itemTime = value; }
        }

        /// <summary>
        /// 评分结束的时间
        /// </summary>
        private string itemEndTime;
        public string ItemEndTime
        {
            get { return itemEndTime; }
            set { itemEndTime = value; }
        }

        /// <summary>
        /// 评分项编码
        /// </summary>
        private string itemNo;
        public string ItemNo
        {
            get { return itemNo; }
            set { itemNo = value; }
        }
    }

    class SSubmitScore
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        /// <summary>
        /// 评分表编码
        /// </summary>
        private string scoreSheetCode;
        public string ScoreSheetCode
        {
            get { return scoreSheetCode; }
            set { scoreSheetCode = value; }
        }

        /// <summary>
        /// 总分
        /// </summary>
        private float totleScore;
        public float TotleScore
        {
            get { return totleScore; }
            set { totleScore = value; }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        private string operatorName;
        public string OperatorName
        {
            get { return operatorName; }
            set { operatorName = value; }
        }



        /// <summary>
        /// 用户认证token
        /// </summary>
        private string accessToken;
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// 操作结束时间
        /// </summary>
        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// 评分数据
        /// </summary>
        private string scoreData;
        public string ScoreData
        {
            get { return scoreData; }
            set { scoreData = value; }
        }

        /// <summary>
        /// 步骤数据
        /// </summary>
        private Dictionary<string,SStepInformation> stepInformation = new Dictionary<string, SStepInformation>();
        public Dictionary<string,SStepInformation> StepInformation
        {
            get { return stepInformation; }
        }

        public SSubmitScore()
        {
            totleScore = 0;
        }
        public void Clear()
        {
            stepInformation.Clear();
            totleScore = 0;
        }

    }
    #endregion

    /// <summary>
    /// 返回数据状态
    /// </summary>
    public class ReturnDataStatus
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        private string code;
        /// <summary>
        /// 属性-返回编码
        /// </summary>
        public string Code { get { return code; } set { code = value; } }

        /// <summary>
        /// 返回描述
        /// </summary>
        private string desc;
        /// <summary>
        /// 属性-返回描述
        /// </summary>
        public string Desc { get { return desc; } set { desc = value; } }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="pDesc"></param>
        public ReturnDataStatus(string pCode, string pDesc)
        {
            this.code = pCode;
            this.desc = pDesc;
        }
    }

    /// <summary>
    /// 登陆信息
    /// </summary>
    public class LoginInfomation
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token = "";
        /// <summary>
        /// token_type
        /// </summary>
        public string token_type = "";
        /// <summary>
        /// expires_in
        /// </summary>
        public string expires_in = "";
        /// <summary>
        /// refresh_token
        /// </summary>
        public string refresh_token = "";

        /// <summary>
        /// 清理
        /// </summary>
        internal void Clear()
        {
            access_token = "";
            token_type = "";
            expires_in = "";
            refresh_token = "";
        }
    }
    /// <summary>
    /// 数据组件
    /// </summary>
    public class IDataComponent : MonoBehaviour
    {
        /// <summary>
        /// 定义返回委托
        /// </summary>
        /// <param name="pReturn"></param>
        public delegate void ReturnDelegate(ReturnDataStatus pReturn);


        static string submitScoreSubUrl = "";

        /// <summary>
        /// 标准评分码，读取本地数据存储
        /// </summary>
        static SSubmitScore standardScoreSheet;

        /// <summary>
        /// 存储一次操作的评分数据
        /// </summary>
        static SSubmitScore submitScore;

        /// <summary>
        /// 登陆信息对象
        /// </summary>
        public static LoginInfomation loginInfomation;

        /// <summary>
        /// 服务器地址
        /// </summary>
        static string toURL = "";

        /// <summary>
        /// 查询返回信息
        /// </summary>
        ReturnDataStatus returnDataStatus;

        string serverTimeErrCode = "";
        string serverTimeErrdesc = "";

        /// <summary>
        /// pc智能产品获取的token
        /// </summary>
        static string pcLoginToken = "";

        /// <summary>
        /// 智能产品跳转个人中心对应的ID
        /// </summary>
        static string gy_trainId = "";

        /// <summary>
        /// 登录时的用户ID
        /// </summary>
        static string userId = "";

        /// <summary>
        /// 账户中文名-web用
        /// </summary>
        static string userName = "";

        /// <summary>
        /// 用户登录（pc、web）时的账户
        /// </summary>
        string loginAccount = "";

        /// <summary>
        /// web的TaskID
        /// </summary>
        string taskid = "";

        string tokens = "";

        string currentServerTime = "";

        TimeSpan timeDValue = TimeSpan.Zero;

        WWW www;

        /// <summary>
        /// 正在发送数据
        /// </summary>
        bool isSending = false;

        /// <summary>
        /// 计时
        /// </summary>
        float mTimeCount = 0.0f;

        /// <summary>
        /// 开始计时标志
        /// </summary>
        bool startTimeFlag = false;

        /// <summary>
        /// 推送数据是否超时标志
        /// </summary>
        bool isOverTime = false;

        /// <summary>
        /// 提交成绩回调事件
        /// </summary>
        ReturnDelegate SubmitEvent;

        string ScoreCommand = "/api/score/submitresult";

        string LoginCommand = "/oauth/access_token";

        #region 创建单例

        void Awake()
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
            submitScore = new SSubmitScore();
            loginInfomation = new LoginInfomation();
        }

        void Start()
        {
            gameObject.name = "DataComponent";
            userName = "";
            returnDataStatus = new ReturnDataStatus("", "");
        }

        void Update()
        {

            //调用html里面的函数
            if (string.IsNullOrEmpty(submitScore.AccessToken))
            {
                Application.ExternalCall("getAccessToken");
            }

            if (string.IsNullOrEmpty(submitScore.OperatorName))
            {
                Application.ExternalCall("getOperatorName");
            }
            if (startTimeFlag == true)
            {
                mTimeCount += Time.deltaTime;
                if (mTimeCount >= 5.0f)
                {
                    returnDataStatus.Desc = "由于网络原因，提交成绩出现异常，请重新操作！";
                    isOverTime = true;
                    startTimeFlag = false;
                    mTimeCount = 0.0f;
                }
            }
        }

        //控制本模块的变量
        private volatile static IDataComponent m_instance;

        private IDataComponent()
        {

        }


        /// <summary>
        /// 创建单例
        /// </summary>
        /// <returns></returns>
        public static IDataComponent GetInstance()
        {
            return m_instance;
        }
        #endregion

        /// <summary>
        /// 为提交成绩增加监听
        /// </summary>
        /// <param name="callback"></param>
        public void AddSubmitListen(ReturnDelegate callback)
        {
            SubmitEvent += callback;
        }

        /// <summary>
        /// 为提交成绩移除监听
        /// </summary>
        /// <param name="callback"></param>
        public void RemoveSubmitListen(ReturnDelegate callback)
        {
            SubmitEvent -= callback;
        }

        /// <summary>
        /// 组提交成绩包
        /// </summary>
        /// <returns></returns>
        string getSubmitScorePacket()
        {
            submitScore.EndTime = DateTime.Now;// + timeDValue).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");

            JsonData data = new JsonData();
            //data["1"] = new JsonData();
            
            data["name"] = submitScore.ProductName;
            data["operation_item"] = submitScore.ScoreSheetCode;//token
            data["score"] = submitScore.TotleScore;//
            data["creattime"] = submitScore.StartTime.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
            data["totaltime"] = (submitScore.EndTime - submitScore.StartTime).TotalMinutes;
            data["data"] = new JsonData();
            data["access_token"] = loginInfomation.access_token;

 
            foreach (var si in submitScore.StepInformation)
            {
                data["data"][si.Key] = new JsonData();
                foreach (var item in si.Value.ItemInfomationList)
                {
                    JsonData content = new JsonData();
                    content["scoreitem_number"] = item.ItemNo;
                    content["time"] = item.ItemTime;
                    data["data"][si.Key].Add(content);
                }

            }
            return data.ToJson();
        }

        /// <summary>
        /// 组登录包
        /// </summary>
        /// <returns></returns>
        string getLoginPacket(string pAccountName, string pPassword)
        {
            JsonData data = new JsonData();
            data = new JsonData();
            data["username"] = pAccountName;
            data["password"] = pPassword;
            data["grant_type"] = "password";
            data["client_id"] = "a5dcca9fc81daea7e70da4792fae190da90e89f0";
            data["client_secret"] = "46adc1588999a8e172a9bf2a71ba71758df96c4e";

            return data.ToJson();
        }

        /// <summary>
        /// 组请求产品ID包
        /// </summary>
        /// <param name="pSheetCode">评分表编码</param>
        /// <returns></returns>
        string getQueryTrainIDPacket(string pSheetCode)
        {
            JsonData data = new JsonData();
            data = new JsonData();
            //data["command"] = "grade/querylearnidbyscoresheetcode";
            data["scoresheetcode"] = pSheetCode;
            return data.ToJson();
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        string getServerTimePacket()
        {
            JsonData data = new JsonData();
            data["1"] = new JsonData();
            data["1"]["command"] = "grade/gettraininfo";
            data["1"]["operatoraccount"] = "";
            data["1"]["operatorsessionid"] = "";
            return data.ToJson();
        }
        #region 发送数据包

        IEnumerator sendQuireTimePacket()
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getServerTimePacket();
            //UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";
            WWW www2 = new WWW(toURL, rawData, headers);
            yield return www2;

            if (www2.error != null)
            {
                serverTimeErrCode = www2.error;
                //UnityEngine.Debug.Log("error:" + www2.error);
            }
            else
            {
                //UnityEngine.Debug.Log("receive data is succeed: " + www2.text);
                //解析数据
                JsonData data = JsonMapper.ToObject(www2.text);
                string funName = data["1"]["command"].ToString();
                serverTimeErrCode = data["1"]["errcode"].ToString();
                serverTimeErrdesc = data["1"]["errdesc"].ToString();

                if (((IDictionary)data["1"]).Contains("currenttime"))
                {
                    currentServerTime = data["1"]["currenttime"].ToString();
                    if (currentServerTime != "")
                    {
                        //UnityEngine.Debug.Log("获取服务器成功，时间为：" + currentServerTime);
                        timeDValue = DateTime.Parse(currentServerTime) - DateTime.Parse(DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss"));
                        //UnityEngine.Debug.Log("timeDvalue=" + timeDValue);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("获取服务器失败,将使用本地系统时间,Code=:" + serverTimeErrCode + ",Desc=" + serverTimeErrdesc);
                    }

                }
                else
                {
                    UnityEngine.Debug.Log("未查到指定字段：currenttime");
                }
            }

            yield return 0;
            #endregion
        }

        /// <summary>
        /// 发送成绩数据
        /// </summary>
        /// <param name="pOpenURL">默认为"",表示web版；否则为智能产品直接打开连接：例http:127.0.0.1:80</param>
        /// <param name="openURL">是否打开报表</param>
        /// <returns></returns>
        IEnumerator sendScorePacket(string pOpenURL, bool openURL = true)
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getSubmitScorePacket();
            UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";
            //UnityEngine.Debug.Log("ToURL="+toURL);
            startTimeFlag = true;
            www = new WWW(toURL+ScoreCommand, rawData, headers);
            yield return www;

            if (www.error != null)
            {
                returnDataStatus.Code = "999";
                returnDataStatus.Desc = www.error;
                isSending = false;
                UnityEngine.Debug.Log("error:" + www.error);
            }
            else
            {
                isSending = false;
                if (isOverTime == false)
                {
                    UnityEngine.Debug.Log("receive data is succeed: " + www.text);
                    //解析数据
                    string scoreid = "";

                    JsonData data = JsonMapper.ToObject(www.text);
                    returnDataStatus.Code = data["code"].ToString();
                    returnDataStatus.Desc = data["msg"].ToString();
                    if (((IDictionary)data).Contains("data"))
                    {
                        scoreid = data["data"]["id"].ToString();
                        if (returnDataStatus.Code == "1")
                        {

                            notifyUnityScoreResult(scoreid);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("发送失败,Code=:" + returnDataStatus.Code + ",Desc=" + returnDataStatus.Desc);
                    }
                }
                else
                {
                    returnDataStatus.Desc = "由于网络原因，提交成绩出现异常，请重新操作！";
                }
                SubmitEvent.Invoke(returnDataStatus);

            }

            yield return 0;
            #endregion
        }

        /// <summary>
        /// 发送登录数据
        /// </summary>
        /// <param name="pAccountName"></param>
        /// <param name="pPassword"></param>
        /// <returns></returns>
        IEnumerator sendLoginPacket(string pAccountName, string pPassword)
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getLoginPacket(pAccountName, pPassword);
            UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";

            www = new WWW(toURL +  LoginCommand, rawData, headers);
            yield return www;
            if (www.error != null)
            {
                isSending = false;
                returnDataStatus.Code = "999";
                returnDataStatus.Desc = www.error;
                UnityEngine.Debug.Log("error:" + www.error);
            }
            else
            {
                isSending = false;
                UnityEngine.Debug.Log("receive data is succeed: " + www.text);
                //解析数据
                JsonData data = JsonMapper.ToObject(www.text);
                returnDataStatus.Code = data["code"].ToString();
                returnDataStatus.Desc = data["msg"].ToString();
                if (((IDictionary)data).Contains("data"))
                {
                    loginInfomation.access_token = data["data"]["access_token"].ToString();
                    //submitScore.OperatorSessionid = pcLoginToken;
    
                    loginInfomation.access_token = data["data"]["access_token"].ToString();
                    loginInfomation.refresh_token = data["data"]["refresh_token"].ToString();
                    loginInfomation.token_type = data["data"]["token_type"].ToString();
                    loginInfomation.expires_in = data["data"]["access_token"].ToString();

                }

                if (returnDataStatus.Code == "0")
                {
                    UnityEngine.Debug.Log("发送成功！");
                    UnityEngine.Debug.Log("Token=" + pcLoginToken);
                    //submitScore.OperatorAccount = pAccountName;
                }
                else
                {
                    UnityEngine.Debug.Log("发送失败,Code=:" + returnDataStatus.Code + ",Desc=" + returnDataStatus.Desc);
                }
            }

            yield return 0;
            #endregion
        }

        /// <summary>
        /// 发送查询产品ID数据
        /// </summary>
        /// <param name="pSheetCode"></param>
        /// <returns></returns>
        IEnumerator queryTrainIDPacket(string pSheetCode)
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getQueryTrainIDPacket(pSheetCode);
            UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";

            www = new WWW(toURL, rawData, headers);
            yield return www;
            if (www.error != null)
            {
                isSending = false;
                returnDataStatus.Code = "999";
                returnDataStatus.Desc = www.error;
                UnityEngine.Debug.Log("error:" + www.error);
            }
            else
            {
                isSending = false;
                UnityEngine.Debug.Log("receive data is succeed: " + www.text);
                //解析数据
                JsonData data = JsonMapper.ToObject(www.text);
                string funName = data["1"]["command"].ToString();
                returnDataStatus.Code = data["1"]["errcode"].ToString();
                returnDataStatus.Desc = data["1"]["errdesc"].ToString();
                if (((IDictionary)data["1"]).Contains("learnid"))
                {
                    gy_trainId = data["1"]["learnid"].ToString();
                }

                if (returnDataStatus.Code == "0")
                {
                    UnityEngine.Debug.Log("发送成功！");
                    UnityEngine.Debug.Log("gy_trainId=" + gy_trainId);
                }
                else
                {
                    UnityEngine.Debug.Log("发送失败,Code=:" + returnDataStatus.Code + ",Desc=" + returnDataStatus.Desc);
                }
            }

            yield return 0;
            #endregion
        }
        #endregion

        #region 网页消息通信
        /// <summary>
        /// 通知网页已发送数据
        /// </summary>
        /// <returns></returns>
        void notifyUnityScoreResult(string id)
        {
            Application.ExternalCall("goScoreResult", id);
        }

        /// <summary>
        /// 通知WEB发送数据失败
        /// </summary>
        /// <param name="desc"></param>
        void notifyHTMLSendDataFailed(string desc)
        {
            Application.ExternalCall("sendDataFailed", "数据发送失败，描述为:" + desc);
        }

       

        /// <summary>
        /// 从Web获取账户名对应的中文名
        /// </summary>
        /// <param name="pName"></param>
        void notifyUnityAccountChineseName(string pName)
        {
            submitScore.OperatorName = pName;
            userName = pName;
        }

        /// <summary>
        /// 从WEB获取TaskID-不可调用
        /// </summary>
        /// <param name="pId"></param>
        void notifyUnityAccessToken(string pToken)
        {
            submitScore.AccessToken = pToken;
        }

        #endregion

        ////////////////////////////////////////////////现使用 通信接口///////////////////////////////////////////////////

        #region 组数据包
        /// <summary>
        /// 增加开始操作时间
        /// </summary>
        public void addStartTime()
        {
            submitScore.StartTime = DateTime.Now;
            UnityEngine.Debug.Log("时间-开始：" + submitScore.StartTime);
        }

        /// <summary>
        /// 增加结束操作时间--提交时自动添加
        /// </summary>
        void addEndTime()
        {
            submitScore.EndTime = DateTime.Now;
            UnityEngine.Debug.Log("时间-结束：" + submitScore.EndTime);
        }

        /// <summary>
        /// 增加评分表编码--表示用的是哪套评分表
        /// </summary>
        /// <param name="pScoreSheetCode"></param>
        public void addScoreSheetCode(string pScoreSheetCode)
        {
            submitScore.ScoreSheetCode = pScoreSheetCode;
        }

        /// <summary>
        /// 添加项目名称
        /// </summary>
        /// <param name="pProductName"></param>
        public void addProductName(string pProductName)
        {
            submitScore.ProductName = pProductName;
        }
        /// <summary>
        /// 增加评分数据,注意，目前要求在程序一运行就调用开始录屏，结束时调用停止
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="pScore"></param>
        /// <param name="pStepID">大步骤ID</param>
        public void addScoreItem(string pCode, float pScore, string pStepID)
        {
            submitScore.TotleScore += pScore;
            if(!submitScore.StepInformation.ContainsKey(pStepID))
            {
                submitScore.StepInformation[pStepID] = new SStepInformation();
                submitScore.StepInformation[pStepID].StepID = pStepID;
            }
            SItemInfomation sf = new SItemInfomation();
            sf.ItemNo = pCode;
            sf.ItemTime = (DateTime.Now).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
            sf.ItemEndTime = DateTime.Now.AddSeconds(2).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");

            foreach (SItemInfomation si in submitScore.StepInformation[pStepID].ItemInfomationList)
            {
                if (si.ItemNo == pCode)
                {
                    return;
                }
            }
            submitScore.StepInformation[pStepID].ItemInfomationList.Add(sf);
        }

        /// <summary>
        /// 为创建的步骤添加开始时间
        /// </summary>
        /// <param name="pStepId">设置为您创建的步骤ID</param>
        /// <param name="isUpdate">如果已经设置时间，是否更新为最新的时间</param>
        private void setStepStartTime(int pStepId, bool isUpdate = true)
        {
            //if (pStepId < 0 || pStepId >= submitScore.StepInfoArray.Count)
            //{
            //    UnityEngine.Debug.LogError("参数pStepId不能得到正确的索引。");
            //    return;
            //}
            //if (submitScore.StepInfoArray[pStepId].StepStartTime == "" || submitScore.StepInfoArray[pStepId].StepStartTime == null || isUpdate == true)
            //    submitScore.StepInfoArray[pStepId].StepStartTime = (DateTime.Now + timeDValue).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
        }

        /// <summary>
        /// 为创建的步骤添加结束时间
        /// </summary>
        /// <param name="pStepId">设置为您创建的步骤ID</param>
        /// <param name="isUpdate">如果已经设置时间，是否更新为最新的时间</param>
        private void setStepEndTime(int pStepId, bool isUpdate = true)
        {
            //if (pStepId < 0 || pStepId >= submitScore.StepInfoArray.Count)
            //{
            //    UnityEngine.Debug.LogError("参数pStepId不能得到正确的索引。");
            //    return;
            //}
            //if (submitScore.StepInfoArray[pStepId].StepEndTime == "" || submitScore.StepInfoArray[pStepId].StepEndTime == null || isUpdate == true)
            //    submitScore.StepInfoArray[pStepId].StepEndTime = (DateTime.Now + timeDValue).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
        }

        #endregion

        /// <summary>
        /// 从本地或者服务器加载评分表简化版
        /// </summary>
        void getScoreSheet()
        {
            
        }

        /// <summary>
        /// 当开始新一轮计分时，复原成绩数据。
        /// </summary>
        public void resetScoreData()
        {
            submitScore.Clear();
            returnDataStatus.Code = "";
            returnDataStatus.Desc = "";
        }

        /// <summary>
        /// 设置推送数据的服务器地址
        /// </summary>
        /// <param name="pURL">例如：http://192.168.1.165:8086</param>
        public void setURL(string pURL)
        {
            toURL = pURL;
        }
        
        /// <summary>
        /// 推送成绩数据--web和智能化产品
        /// </summary>
        /// <param name="pOpenURL">例：http://192.168.1.201；智能设备：如果此参数不为空，发送成功后将打开连接；WEb：参数为空</param>
        /// <param name="isOpenURL">是否打开报表</param>
        public void sendScoreData(string pOpenURL = "", bool isOpenURL = true)
        {
            if (isSending == false)
            {
                isSending = true;
                returnDataStatus.Code = "";
                returnDataStatus.Desc = "";
                startTimeFlag = false;
                mTimeCount = 0.0f;
                isOverTime = false;
                StartCoroutine(sendScorePacket(pOpenURL, isOpenURL));
            }
        }
        /// <summary>
        /// 推送成绩数据--web和智能化产品
        /// </summary>
        /// <param name="pUsername">用户名</param>
        /// <param name="pPassword">密码</param>
        public void sendLogin(string pUsername = "", string pPassword = "")
        {
            if (isSending == false)
            {
                isSending = true;
                returnDataStatus.Code = "";
                returnDataStatus.Desc = "";
                startTimeFlag = false;
                mTimeCount = 0.0f;
                isOverTime = false;
                loginInfomation.Clear();
                StartCoroutine(sendLoginPacket(pUsername, pPassword));
            }
        }

        /// <summary>
        /// 同步服务器时间系统
        /// </summary>
        public void synchronizationServerTimeSystem()
        {
            currentServerTime = "";
            timeDValue = TimeSpan.Zero;
            StartCoroutine(sendQuireTimePacket());
        }

        /// <summary>
        /// 发送查询产品数据
        /// </summary>
        /// <param name="pSheetCode">当前产品评分表编码</param>
        public void sendQueryTrainID(string pSheetCode)
        {
            if (isSending == false)
            {
                isSending = true;
                gy_trainId = "";
                returnDataStatus.Code = "";
                returnDataStatus.Desc = "";
                StartCoroutine(queryTrainIDPacket(pSheetCode));
            }
        }

        ///// <summary>
        ///// 推送成绩后，可在Update里调用此接口检测是否发送成功。
        ///// </summary>
        ///// <returns>返回数据状态结构类，Code表示返回编码：0表示成功，否则失败，默认为“”表示查询中；Desc为返回描述</returns>
        ////public ReturnDataStatus checkSendScoreDataIsSucceed()
        ////{
        ////    return returnDataStatus;
        ////}

        /// <summary>
        /// 发送登录请求后，可在update里检测是否登录成功。
        /// </summary>
        /// <returns>返回数据状态结构类，Code表示返回编码：0表示成功，否则失败，默认为“”表示查询中；Desc为返回描述</returns>
        public ReturnDataStatus checkLoginIsSucceed()
        {
            return returnDataStatus;
        }

        /// <summary>
        /// 查询训练ID后，可在Update里检测是否查询成功
        /// </summary>
        /// <returns>返回数据状态结构类，Code表示返回编码：0表示成功，否则失败，默认为“”表示查询中；Desc为返回描述</returns>
        public ReturnDataStatus checkQueryTrainIDIsSucceed()
        {
            return returnDataStatus;
        }

        /// <summary>
        /// 从web端获取训练者中文名
        /// </summary>
        /// <returns></returns>
        public string getWebAccountChineseName()
        {
            return userName;
        }


        /// <summary>
        /// 导出已经存在的评分项并返回一个评分项列表
        /// </summary>
        /// <returns></returns>
        public List<string> exportAllScoreCode()
        {
            List<string> scoreCodeL = new List<string>();
            foreach (var item in submitScore.StepInformation)
            {
                foreach (var info in item.Value.ItemInfomationList)
                {
                    scoreCodeL.Add(info.ItemNo);
                }
            }
            return scoreCodeL;
        }

        /// <summary>
        /// 导出当前的评分表编码
        /// </summary>
        /// <returns></returns>
        public string exportScoreSheetCode()
        {
            return submitScore.ScoreSheetCode;
        }

        ////////////////////////////////////////////////与HTML进行交互///////////////////////////////////////
        /// <summary>
        /// 通知网页操作已结束,将提示训练已结束。
        /// </summary>
        /// <returns></returns>
        void notifyHTMLFinished()
        {
            Application.ExternalCall("operatorEnd", "训练已结束！");
        }
    }
}
