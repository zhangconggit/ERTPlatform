using UnityEngine;
using System.Collections;
using System;
using IDataComponentDLL;
using System.Collections.Generic;

/// <summary>
/// 模式
/// </summary>
public enum OperatorSchema
{
    isNULL = 0,
    /// <summary>
    /// 教学模式
    /// </summary>
    teachModel,
    /// <summary>
    /// 练习模式
    /// </summary>
    practiceModel,
    /// <summary>
    /// 考试模式
    /// </summary>
    examModel
}
/// <summary>
/// 灌肠包类型
/// </summary>
public enum enemaBag
{
    isNULL = -1, //未选择
    ok, //正确
    typeError, //型号错误
    timeError, // 时间错误
    error //破损
}
/// <summary>
/// 用户信息
/// </summary>
public struct GUserInfo
{
    public string accountName; //用户名
    public string chineseName; //姓名
    public string sex;// n v
    public bool isLogin;
    public UserStatus status;
}
/// <summary>
/// 并发症信息
/// </summary>
public struct ComplicationInfo
{
    //肠道出血
    public float deepNo1; //深度
    public float oddsNo1; //几率
    //肠道受阻
    public float deepNo2; //深度
    public float oddsNo2; //几率
    //肠道破裂
    public float timesNo3; //次数
    public float deepNo3; //高度
    public float speedNo3; //高度
    public float oddsNo3; //几率
    //灌肠停止
    public float oddsNo4; //几率
}
/// <summary>
/// 病例附加信息
/// </summary>
public struct OverheadInfo
{
    /// <summary>
    /// 温度
    /// </summary>
    public float temperatureMin;
    public float temperatureMax;

}
/// <summary>
/// 版本模式
/// </summary>
public enum SYS
{
    /// <summary>
    /// 模拟智能版
    /// </summary>
    pc = 0,
    /// <summary>
    /// 硬件智能版
    /// </summary>
    pc_hard,
    /// <summary>
    /// 在线版
    /// </summary>
    web,
    /// <summary>
    /// 模拟病床版
    /// </summary>
    sickbed,
    /// <summary>
    /// 硬件病床版
    /// </summary>
    sickbed_hard
}
public enum TipsType
{
    line,
    board
}
public enum UserStatus
{
    isNULL,
    /// <summary>
    /// 用户
    /// </summary>
    user,
    /// <summary>
    /// 服务人员
    /// </summary>
    server,
    /// <summary>
    /// 游客
    /// </summary>
    visitor
}
/// <summary>
/// 并发症种类
/// </summary>
public enum ComplicationType
{
    isNULL, //没有
    /// <summary>
    /// 肠道出血
    /// </summary>
    intestinalBleeding,
    /// <summary>
    /// 插管受阻
    /// </summary>
    intubationBlocked,
    /// <summary>
    /// 肠道破裂
    /// </summary>
    intestinalRupture,
    /// <summary>
    /// 灌肠停止
    /// </summary>
    enemaStop
}
public class GlobalClass
{
    /// <summary>
    /// 模式
    /// </summary>
#if UNITY_WEBPLAYER
    public static SYS sys = SYS.web;
#else
    public static SYS sys = SYS.pc_hard;
#endif
    /// <summary>
    /// 是否生成语音
    /// </summary>
    public static bool isMarkAudio = false;
    /// <summary>
    /// 下一步场景名
    /// </summary>
    public static string nextScene = "bingfang";
    /// <summary>
    /// 用户信息
    /// </summary>
    public static GUserInfo user = new GUserInfo() { sex = "v" };
    /// <summary>
    /// 提示方式
    /// </summary>
    public static TipsType tipsType = TipsType.line;

    /// <summary>
    /// 提示方式
    /// </summary>
    public static enemaBag currentEnemaBag = enemaBag.isNULL;

    public static bool showScore = false;
    /// <summary>
    /// 术后判断
    /// </summary>
    public static bool bRecordCheck = false;
    /// <summary>
    /// 灌肠包信息信息
    /// </summary>
    public static Dictionary<string, string> enemaBagInfo = new Dictionary<string, string>();
    /// <summary>
    /// 围帘是否打开
    /// </summary>
    public static bool isWeiLianOpen = true;
    /// <summary>
    /// 是否排尽空气
    /// </summary>
    public static bool isPaiJingQiTi = false;
    /// <summary>
    /// 杯中水量ml  0~1000ml
    /// </summary>
    public static int cupWater = 500;
    /// <summary>
    /// 杯容量  1000ml
    /// </summary>
    public static int cupWaterMax = 2000;

    /// <summary>
    /// 袋中水量ml  0~1000ml
    /// </summary>
    public static float bagWater = 0;
    /// <summary>
    /// 袋容量  1000ml
    /// </summary>
    public static int bagWaterMax = 1000;
    /// <summary>
    /// 当前灌肠包高度
    /// </summary>
    public static float enemaBagHight = 100;
    /// <summary>
    /// 当前病例需要灌肠的量
    /// </summary>
    public static int enemaValue = 400;
    /// <summary>
    /// 当前实际灌肠的量
    /// </summary>
    public static float currentEnemaValue = 0;
    /// <summary>
    /// 是否提交成绩
    /// </summary>
    public static bool isCommiting = false;//
    /// <summary>
    /// 是否点击下一步按钮
    /// </summary>
    public static bool isClickedNextButton = false;
    /// <summary>
    /// 是否点击视角按钮
    /// </summary>
    public static bool isClickedLookButton = false;
    /// <summary>
    /// 当前选择的体位
    /// </summary>
    public static string TiWei = "仰卧位";
    /// <summary>
    /// 并发症信息
    /// </summary>
    public static ComplicationInfo sComplicationInfo = new ComplicationInfo();
    /// <summary>
    /// 病例附加信息
    /// </summary>
    public static OverheadInfo sOverheadInfo = new OverheadInfo();
    /// <summary>
    /// 高亮颜色
    /// </summary>
    public static Color hightColor = Color.white;
    /// <summary>
    /// 插管深度
    /// </summary>
    public static float cannulaDeep = 0;
    /// <summary>
    /// 爆发并发症
    /// </summary>
    public static bool bStartComplication = false;
    /// <summary>
    /// 开始监听并发症
    /// </summary>
    public static ComplicationType bListeningComplication = ComplicationType.isNULL;

    //private long OperatorStartTime;
    static TimeSpan ts1;
    /// <summary>
    /// 操作模式
    /// </summary>
    public static OperatorSchema g_OperatorSchema;//操作模式
    /// <summary>
    /// 当前病例信息
    /// </summary>
    public static MedicalRecord g_CurrentRecord;
    /// <summary>
    /// 当前病人信息
    /// </summary>
    public static MedicalRecord g_CurrentSickInfo;
    //public static SickInfo g_CurrentRecord =new SickInfo();//当前病历
    /// <summary>
    /// 操作结束标志
    /// </summary>
    public static bool operatorIsEnd;
    /// <summary>
    /// main loading
    /// </summary>
    public static bool stIsLoading = true;
    /// <summary>
    /// 等待
    /// </summary>
    public static bool sMainIsWait = false;
    public static bool sWebInitWait = true;
    public static void ReSet()
    {
        //g_OperatorSchema = OperatorSchema.isNULL;
        g_CurrentRecord = null;
        operatorIsEnd = false;
        isWeiLianOpen = true;
        isCommiting = false;
        isClickedNextButton = false;
        TiWei = "";
        cannulaDeep = 0;
        enemaBagHight = 100;
        bStartComplication = false;
    }
    /// <summary>
    /// 检查是否选择病历
    /// </summary>
    /// <returns>是</returns>
    public static bool CheckChooseFlish()
    {
        if (g_OperatorSchema == OperatorSchema.isNULL || g_CurrentRecord == null)
        {
            return false;
        }
        else
            return true;
    }
    /// <summary>
    /// 记录当前时间
    /// </summary>
    public static void MarkTime()
    {
        //OperatorStartTime = System.DateTime.Now.Ticks;
        IDataComponent.GetInstance().addStartTime();
        ts1 = new TimeSpan(System.DateTime.Now.Ticks);
    }
    /// <summary>
    /// 取得操作时间
    /// </summary>
    /// <returns>秒</returns>
    public static int getOperatorTime()
    {
        TimeSpan ts2 = new TimeSpan(System.DateTime.Now.Ticks);
        TimeSpan ts = ts1.Subtract(ts2).Duration();//.TotalSeconds();
        int i = (int)ts.TotalSeconds;// (int)(System.DateTime.Now.Ticks - OperatorStartTime);
        return i;
    }
}
