#define Test
using UnityEngine;
using System.Collections;
using CFramework;

public class CGlobal  {


    /// <summary>
    /// 数据库地址
    /// </summary>
    public static string url = "http://192.168.1.110:80";

    public static string videourl = "";

    /// <summary>
    /// 产品名
    /// </summary>
    public static string productName = "";

    /// <summary>
    /// 产品英文名
    /// </summary>
    public static string productNameEnglish = "";

    /// <summary>
    /// 产品编码
    /// </summary>
    public static string productCode = "";

    /// <summary>
    /// 产品ID
    /// </summary>
    public static string productid = "";

    /// <summary>
    /// 操作模式
    /// </summary>
    public static OperationMode mode = OperationMode.Teaching;

    /// <summary>
    /// 用户名
    /// </summary>
    public static string userName = "";

    /// <summary>
    /// 用户信息
    /// </summary>
    public static UserInfo user = new UserInfo();

    /// <summary>
    /// 病例
    /// </summary>
    public static _CLASS_MedicalRecord currentMedicalRecord = new _CLASS_MedicalRecord();

    /// <summary>
    /// 是否输出日志
    /// </summary>
    public static bool IsOutputLog = true;

    internal static int DefaultAnchorStepIndex = 0;

    internal static string pagehead = "";

    internal static int OpenCamearaRec = 0;

    internal static string abversion = "";

    internal static string scoresheetversion = "";

    internal static SceneInfo currentSceneInfo = new SceneInfo() { name="李雷", sex= PeopleSex.男, context="描述", demand = "要求" };

    public static Vector3 PunctureTexturePoint = new Vector3(741, 393,0);
}

/// <summary>
/// 操作模式
/// </summary>
public enum OperationMode
{
    None = -1,
    /// <summary>
    /// 教学
    /// </summary>
    Teaching,
    /// <summary>
    /// 练习
    /// </summary>
    Practice,
    /// <summary>
    /// 考试
    /// </summary>
    Exam
}
